<template>
  <header>
    <HeaderNav :activeIndex="1" />
  </header>
  <main class="q-my-lg">
    <div class="q-mx-auto col-8 row" style="max-width: 1100px">
      <StatisticsConfPanel
        class="col-md-4 col-12"
        :default-settings="statisticsSettings"
        :line-options="lines"
        :lines-loading="linesLoading"
        @change="updateDisturbances"
      />
      <div class="col-md-8 col-12 q-pa-md">
        <q-card class="bg-white q-pa-sm q-mb-md">
          <q-card-section>
            <Bar
              :data="{ datasets: [{ data: statistics.byTime }] }"
              :options="{
                responsive: true,
                backgroundColor: 'Tomato',
                plugins: {
                  legend: {
                    display: false,
                  },
                  title: {
                    display: true,
                    text: 'Nach Zeitraum',
                    font: {
                      size: 18,
                    },
                  },
                },
              }"
            />
          </q-card-section>
        </q-card>
        <q-card class="bg-white q-pa-sm q-mb-md">
          <q-card-section>
            <Pie
              :data="{
                datasets: [{ data: statistics.byType.map((d) => d.y) }],
                labels: statistics.byType.map(
                  (d) =>
                    $globals.TYPE_OPTIONS.find((o) => o.value === d.x).label
                ),
              }"
              :options="{
                responsive: true,
                plugins: {
                  legend: {
                    position: 'right',
                  },
                  title: {
                    display: true,
                    text: 'Nach Typ',
                    font: {
                      size: 18,
                    },
                  },
                },
                backgroundColor: [
                  'Tomato',
                  'Aquamarine',
                  'Aqua',
                  'Khaki',
                  'LightSalmon',
                  'MediumPurple',
                  'RosyBrown',
                  'Gainsboro',
                  'Cyan',
                  'Lime',
                  'AliceBlue',
                  'Thistle',
                  'Olive',
                  'Plum',
                  'Silver',
                  'Gold',
                ],
              }"
            />
          </q-card-section>
        </q-card>
        <q-card class="bg-white q-pa-sm">
          <q-card-section :style="{ height: (statistics.byLine.length * 30 + 100).toString() + 'px' }">
            <Bar
              :data="{
                datasets: [{ data: statistics.byLine.map((d) => d.y) }],
                labels: statistics.byLine.map((d) => d.x),
              }"
              :options="{
                responsive: true,
                indexAxis: 'y',
                backgroundColor: 'Tomato',
                maintainAspectRatio: false,
                plugins: {
                  legend: {
                    display: false,
                  },
                  title: {
                    display: true,
                    text: 'Nach Linie',
                    font: {
                      size: 18,
                    },
                  },
                },
              }"
            />
          </q-card-section>
        </q-card>
      </div>
    </div>
  </main>
</template>

<script>
import HeaderNav from "@/components/HeaderNav.vue";
import StatisticsConfPanel from "@/components/StatisticsConfPanel.vue";
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale,
  ArcElement,
} from "chart.js";
import { Bar, Pie } from "vue-chartjs";

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  ArcElement,
  Title,
  Tooltip,
  Legend
);

export default {
  name: "HomeView",
  inject: ["$globals"],

  components: {
    HeaderNav,
    StatisticsConfPanel,
    Bar,
    Pie,
  },

  methods: {
    async updateDisturbances(params) {
      this.$router.push({
        query: {
          ...(params.valueAxis !== "Count" && {
            valueAxis: params.valueAxis,
          }),
          ...(params.timeFrame !== "Year" && {
            timeFrame: params.timeFrame,
          }),
          ...(params.fromDate !== this.$globals.defaultDate && {
            fromDate: params.fromDate,
          }),
          ...(params.toDate !== this.$globals.defaultDate && {
            toDate: params.toDate,
          }),
          ...(params.types.length < 16 &&
            params.types.length > 0 && { types: params.types.toString() }),
          ...(params.lines.length > 0 && { lines: params.lines.toString() }),
        },
      });
      await this.fetchStatistics(params);
    },

    async fetchStatistics(params) {
      this.statisticsLoading = true;
      if (params.types.length === 0) {
        return;
      }
      try {
        // date parsing
        const fromDateArr = params.fromDate.split(".");
        const fromDate = `${fromDateArr[2]}-${fromDateArr[1]}-${fromDateArr[0]}`;
        const toDateArr = params.toDate.split(".");
        const toDate = `${toDateArr[2]}-${toDateArr[1]}-${toDateArr[0]}`;
        let url = `/statistics?fromDate=${fromDate}&toDate=${toDate}&valueAxis=${
          params.valueAxis
        }&timeFrame=${params.timeFrame}&types=${params.types.toString()}`;
        if (params.lines.length > 0) {
          url += `&lines=${params.lines.toString()}`;
        }
        const statistics = await this.$globals.fetch(url);

        if (params.timeFrame === "Hour") {
          statistics.byTime = [...Array(24).keys()].map((h) => {
            const stat = statistics.byTime.find((s) => s.x === h.toString());
            const formatH = `${h.toString().padStart(2, "0")}:00`;
            return stat ? { ...stat, x: formatH } : { x: formatH, y: 0 };
          });
        } else if (params.timeFrame === "Weekday") {
          statistics.byTime = this.$globals.WEEKDAY_NAMES.map((name, index) => {
            const stat = statistics.byTime.find(
              (s) => s.x === index.toString()
            );
            return stat ? { ...stat, x: name } : { x: name, y: 0 };
          });
          statistics.byTime.push(statistics.byTime.shift());
        } else {
          statistics.byTime.sort((a, b) => a.x - b.x);
        }
        if (params.timeFrame === "Month") {
          statistics.byTime = statistics.byTime.map((s) => ({
            ...s,
            x: this.$globals.MONTH_NAMES[parseInt(s.x) - 1],
          }));
        }

        statistics.byType.sort((a, b) => b.y - a.y);

        statistics.byLine.sort((a, b) => b.y - a.y);

        this.statistics = statistics;
      } catch (err) {
        console.log(err);
        this.statistics = [];
      }
    },

    async fetchLines() {
      this.linesLoading = true;
      try {
        this.lines = await this.$globals.fetch("/lines");
      } catch (err) {
        console.log(err);
      }
      this.linesLoading = false;
    },

    orderLines(lines) {
      return this.lines.filter((line) => lines.some((l) => l.id === line.id));
    },
  },

  async created() {
    await this.fetchLines();
    const q = this.$route.query;
    this.statisticsSettings = {
      ...q,
      ...("types" in q && { types: q.types.split(",") }),
      ...("lines" in q && { lines: q.lines.split(",") }),
    };
  },

  data() {
    return {
      statisticsSettings: null,
      statistics: { byTime: [], byType: [], byLine: [] },
      statisticsLoading: false,
      lines: [],
      linesLoading: false,
    };
  },

  watch: {
    statistics() {
      this.statisticsLoading = false;
    },

    $route(from, to) {
      const q = this.$route.query;
      this.statisticsSettings = {
        ...q,
        ...("types" in q && { types: q.types.split(",") }),
        ...("lines" in q && { lines: q.lines.split(",") }),
      };
    },
  },
};
</script>

<style></style>
