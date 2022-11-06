<template>
  <header>
    <HeaderNav :activeIndex="0" />
  </header>
  <main class="q-my-lg">
    <div class="q-mx-auto col-8 row" style="max-width: 1100px">
      <FilterSortPanel class="col-md-4 col-12" @change="fetchDisturbances" />
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
  },

  methods: {
    updateDisturbances (params) {

    },

    async fetchDisturbances (params) {
      try {
        // date parsing
        const fromDateArr = params.fromDate.split('.')
        const fromDate = `${fromDateArr[2]}-${fromDateArr[1]}-${fromDateArr[0]}}`
        const toDateArr = params.toDate.split('.')
        const toDate = `${toDateArr[2]}-${toDateArr[1]}-${toDateArr[0]}}`

        let url = `https://wls.byleo.net/api/disturbances?from=${fromDate}&to=${toDate}&${params.sort.value}type=${params.types.toString()}&line=${params.lines.toString()}`
        if (params.onlyOpenDisturbances) {
          url += '&active=true'
        }

        const res = await fetch(url)
        const data = await res.json()
        if (!('error' in data)) {
          return data.data
        }
      } catch (err) {
        console.log(err)
      }
      return null
    }
  },

  data () {
    return {
      disturbances: null
    }
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
