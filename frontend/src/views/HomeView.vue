<template>
  <header>
    <HeaderNav :activeIndex="0" />
  </header>
  <main class="q-my-lg">
    <div class="q-mx-auto col-8 row" style="max-width: 1100px">
      <FilterSortPanel class="col-md-4 col-12" />
      <DisturbancesPanel class="col-md-8 col-12" />
    </div>
  </main>
</template>

<script>
import HeaderNav from '@/components/HeaderNav.vue'
import FilterSortPanel from '@/components/FilterSortPanel.vue'
import DisturbancesPanel from '@/components/DisturbancesPanel.vue'

export default {
  name: 'HomeView',

  components: {
    HeaderNav,
    FilterSortPanel,
    DisturbancesPanel
  }
}
</script>

<style>
h2 {
  font-size: 1.5rem !important;
  margin: 0 !important;
  display: flex !important;
  align-items: center !important;
}
h3 {
  font-size: 1.125rem !important;
  margin: 0 !important;
}
</style>
