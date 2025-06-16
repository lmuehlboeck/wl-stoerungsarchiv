<template>
  <header>
    <HeaderNav :activeIndex="0" />
  </header>
  <main class="q-my-lg">
    <div class="q-mx-auto col-8 row" style="max-width: 1100px">
      <FilterSortPanel
        class="col-md-4 col-12"
        :defaultParams="FILTER_SORT_PARAMS"
        @change="updateDisturbances"
      />
      <DisturbancesPanel class="col-md-8 col-12" ref="distPanel" />
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
    updateDisturbances(params) {
      this.$router.push({
        query: {
          ...(params.orderBy !== 0 && { orderBy: params.order }),
          ...(params.onlyClosed && { onlyClosed: params.onlyClosed }),
          ...(params.fromDate !== this.$globals.defaultDate && {
            fromDate: params.fromDate,
          }),
          ...(params.toDate !== this.$globals.defaultDate && {
            toDate: params.toDate,
          }),
          ...(params.types.length < 14 &&
            params.types.length > 0 && { types: params.types.toString() }),
          ...(params.lines.length > 0 && { lines: params.lines.toString() }),
        },
      });
      this.$refs.distPanel.update(params);
    },
  },

  created() {
    const q = this.$route.query;
    this.FILTER_SORT_PARAMS = {
      ...q,
      ...("types" in q && { types: q.types.split(",") }),
      ...("lines" in q && { lines: q.lines.split(",") }),
    };
  },

  data() {
    return {
      disturbances: null,
    };
  },
};
</script>

<style></style>
