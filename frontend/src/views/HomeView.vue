<template>
  <header>
    <HeaderNav :activeIndex="0" />
  </header>
  <main class="q-my-lg">
    <div class="q-mx-auto col-8 row" style="max-width: 1100px">
      <FilterSortPanel class="col-md-4 col-12" :getLineColor="getLineColor" @change="updateDisturbances" />
      <DisturbancesPanel class="col-md-8 col-12" :getLineColor="getLineColor" ref="distPanel" />
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
      this.$refs.distPanel.update(params)
    },

    getLineColor (id, type) {
      const colors = ['blue', 'red', 'grey', 'purple']
      if (type === 2) {
        if (id.includes('U1')) { return 'red' }
        if (id.includes('U2')) { return 'pink' }
        if (id.includes('U3')) { return 'orange' }
        if (id.includes('U4')) { return 'green' }
        if (id.includes('U5')) { return 'teal' }
        if (id.includes('U6')) { return 'brown' }
      }
      return colors[type]
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

</style>
