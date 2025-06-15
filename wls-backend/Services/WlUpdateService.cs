
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Text.Json.Nodes;
using wls_backend.Data;
using wls_backend.Models.Domain;
using wls_backend.Models.Enums;

namespace wls_backend.Services
{
    public class WlUpdateService : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly HttpClient _httpClient;
        private readonly ILogger<WlUpdateService> _logger;

        public WlUpdateService(IConfiguration config, IServiceScopeFactory scopeFactory, IHttpClientFactory httpClientFactory, ILogger<WlUpdateService> logger)
        {
            _config = config;
            _scopeFactory = scopeFactory;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int period = _config.GetValue<int>("WlUpdate:Period");
            using PeriodicTimer timer = new(TimeSpan.FromSeconds(period));
            do
            {
                await UpdateDb();
                try
                {
                    
                    _logger.LogInformation("Database updated successfully at {Time}", DateTime.Now);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error updating database: {ex.Message}");
                }
            } while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken));
        }

        private async Task UpdateDb()
        {
            string uri = _config["WlUpdate:Uri"]!;
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to fetch data from {uri}. Status code: {response.StatusCode}");
            }
            var content = await response.Content.ReadAsStringAsync();

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var openDisturbances = await context.Disturbance
                .Include(d => d.Descriptions)
                .Where(d => d.EndedAt == null)
                .ToListAsync();
            var processedDisturbanceIds = new List<string>();

            var rootNode = JsonNode.Parse(content) ?? throw new Exception("Failed to parse JSON response");
            var responseDisturbances = rootNode["data"]!["trafficInfos"]!
                .AsArray()
                .Select(n => DisturbanceFromJsonNode(n!, context));

            foreach (var responseDisturbance in responseDisturbances)
            {
                if (processedDisturbanceIds.Any(i => i == responseDisturbance.Id))
                    continue; // already processed this disturbance
                processedDisturbanceIds.Add(responseDisturbance.Id);

                var dbDisturbance = openDisturbances
                    .FirstOrDefault(d => d.Id == responseDisturbance.Id);

                var relatedDisturbances = responseDisturbances
                    .Where(d => d.Id == responseDisturbance.Id)
                    .ToList();
                if (relatedDisturbances.Count > 1)
                {
                    responseDisturbance.Lines = relatedDisturbances.SelectMany(d => d.Lines).Distinct().ToList();
                    responseDisturbance.Descriptions.Last().Text = string.Join(" / ", relatedDisturbances.Select(d => d.Descriptions.Last().Text));
                }

                if (dbDisturbance == null)  // new disturbance
                {
                    context.Disturbance.Add(responseDisturbance);
                    continue;
                }
                else // existing disturbance (single)
                {
                    if (dbDisturbance.Title != responseDisturbance.Title)
                    {
                        dbDisturbance.Title = responseDisturbance.Title;
                        dbDisturbance.Type = DisturbanceTypeHelper.FromTitle(dbDisturbance.Title);
                    }
                    if (dbDisturbance.Descriptions.LastOrDefault()?.Text != responseDisturbance.Descriptions.Last().Text)
                    {
                        dbDisturbance.Descriptions.Add(new DisturbanceDescription()
                        {
                            DisturbanceId = dbDisturbance.Id,
                            CreatedAt = DateTime.Now,
                            Text = responseDisturbance.Descriptions.Last().Text
                        });
                    }
                }
                
            }

            // close disturbances that are no longer present in the response
            foreach (var closedDisturbance in openDisturbances.Where(d => !processedDisturbanceIds.Contains(d.Id)))
            {
                closedDisturbance.EndedAt = DateTime.Now;
            }

            await context.SaveChangesAsync();
        }

        private Disturbance DisturbanceFromJsonNode(JsonNode node, AppDbContext context)
        {
            var id = node["name"]!.GetValue<string>();
            if (id.Count(id => id == '-') > 1)
            {
                id = id.Remove(id.LastIndexOf('-'));
            }
            var title = node["title"]!.GetValue<string>();
            var description = node["description"]!.GetValue<string>();
            var startedAt = DateTime.Parse(node["time"]!["start"]!.GetValue<string>());
            var lines = node["attributes"]!["relatedLineTypes"]!
                .AsObject()
                .Select(l =>
                {
                    var dbLine = context.Line.Find(l.Key);
                    if (dbLine == null)
                    {
                        dbLine = new Line
                        {
                            Id = l.Key,
                            Type = LineTypeHelper.FromType(l.Value!.GetValue<string>()),
                            DisplayName = l.Key
                        };
                        context.Line.Add(dbLine);
                    }
                    return dbLine;
                })
                .ToList();
            return new Disturbance
            {
                Id = id,
                Title = title,
                Type = DisturbanceTypeHelper.FromTitle(title),
                StartedAt = startedAt,
                Descriptions = [new DisturbanceDescription()
                        {
                            DisturbanceId = id,
                            CreatedAt = startedAt,
                            Text = description
                        }],
                Lines = lines
            };
        }
    }
}
