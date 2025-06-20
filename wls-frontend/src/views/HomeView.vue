<template>
  <header>
    <HeaderNav :activeIndex="0" />
  </header>
  <main class="q-my-lg">
    <div class="q-mx-auto col-8 row" style="max-width: 1100px">
      <FilterSortPanel
        class="col-md-4 col-12"
        :default-settings="filterSortSettings"
        :line-options="lines"
        :lines-loading="linesLoading"
        @change="updateDisturbances"
      />
      <DisturbancesPanel
        class="col-md-8 col-12"
        :disturbances="disturbances"
        :loading="disturbancesLoading"
      />
    </div>
  </main>
</template>

<script>
import HeaderNav from "@/components/HeaderNav.vue";
import FilterSortPanel from "@/components/FilterSortPanel.vue";
import DisturbancesPanel from "@/components/DisturbancesPanel.vue";

export default {
  name: "HomeView",
  inject: ["$globals"],

  components: {
    HeaderNav,
    FilterSortPanel,
    DisturbancesPanel,
  },

  methods: {
    async updateDisturbances(params) {
      this.$router.push({
        query: {
          ...(params.orderBy !== "StartedAtDesc" && {
            orderBy: params.orderBy,
          }),
          ...(params.onlyActive && { onlyActive: params.onlyActive }),
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
      await this.fetchDisturbances(params);
    },

    async fetchDisturbances(params) {
      this.disturbancesLoading = true;
      if (params.types.length === 0) {
        this.disturbances = [];
        return;
      }
      try {
        // date parsing
        const fromDateArr = params.fromDate.split(".");
        const fromDate = `${fromDateArr[2]}-${fromDateArr[1]}-${fromDateArr[0]}`;
        const toDateArr = params.toDate.split(".");
        const toDate = `${toDateArr[2]}-${toDateArr[1]}-${toDateArr[0]}`;
        let url = `/disturbances?fromDate=${fromDate}&toDate=${toDate}&orderBy=${
          params.orderBy
        }&types=${params.types.toString()}`;
        if (params.lines.length > 0) {
          url += `&lines=${params.lines.toString()}`;
        }
        if (params.onlyActive) {
          url += "&onlyActive=true";
        }
        this.disturbances = (await this.$globals.fetch(url)).map((d) => ({
          ...d,
          lines: this.orderLines(d.lines),
        }));
      } catch (err) {
        console.log(err);
        this.disturbances = [];
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
    this.filterSortSettings = {
      ...q,
      ...("types" in q && { types: q.types.split(",") }),
      ...("lines" in q && { lines: q.lines.split(",") }),
    };
  },

  data() {
    return {
      filterSortSettings: null,
      disturbances: [],
      disturbancesLoading: false,
      lines: [],
      linesLoading: false,
    };
  },

  watch: {
    disturbances() {
      this.disturbancesLoading = false;
    },

    $route(from, to) {
      const q = this.$route.query;
      this.filterSortSettings = {
        ...q,
        ...("types" in q && { types: q.types.split(",") }),
        ...("lines" in q && { lines: q.lines.split(",") }),
      };
    }
  },
};
</script>

<style></style>
