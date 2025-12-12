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
            
          </q-card-section>
        </q-card>
        <q-card class="bg-white q-pa-sm q-mb-md">
          <q-card-section>

          </q-card-section>
        </q-card>
        <q-card class="bg-white q-pa-sm">
          <q-card-section>

          </q-card-section>
        </q-card>
      </div>
    </div>
  </main>
</template>

<script>
import HeaderNav from "@/components/HeaderNav.vue";
import StatisticsConfPanel from "@/components/StatisticsConfPanel.vue";

export default {
  name: "HomeView",
  inject: ["$globals"],

  components: {
    HeaderNav,
    StatisticsConfPanel,
  },

  methods: {
    async updateDisturbances(params) {
      this.$router.push({
        query: {
          ...(params.valueAxis !== "Count" && {
            valueAxis: params.valueAxis,
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
        }&types=${params.types.toString()}`;
        if (params.lines.length > 0) {
          url += `&lines=${params.lines.toString()}`;
        }
        this.statistics = (await this.$globals.fetch(url));
        console.log(this.statistics);
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
      statistics: [],
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
    }
  },
};
</script>

<style></style>
