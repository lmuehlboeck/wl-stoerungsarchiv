using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using wls_backend.Data;
using wls_backend.Services;

var builder = WebApplication.CreateBuilder(args);
var AllowLocalOrigins = "AllowLocalOrigins";


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var connectionString = string.Format(
            "Server={0};User Id={1};Password={2};TrustServerCertificate=True;Database={3};",
            builder.Configuration["Db:Host"],
            builder.Configuration["Db:User"],
            builder.Configuration["Db:Password"],
            builder.Configuration["Db:Database"]
        );
        options.UseNpgsql(connectionString);
    }
);

builder.Services.AddTransient<DisturbanceService>();
builder.Services.AddTransient<LineService>();

builder.Services.AddHttpClient();
builder.Services.AddHostedService<WlUpdateService>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(AllowLocalOrigins,
            builder => builder.WithOrigins("http://localhost:8080", "http://localhost:8081")
                              .AllowAnyMethod()
                              .AllowAnyHeader());
    });
}


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(AllowLocalOrigins);

app.UseAuthorization();

app.MapControllers();

using (var Scope = app.Services.CreateScope())
{
    var context = Scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

app.Run();
