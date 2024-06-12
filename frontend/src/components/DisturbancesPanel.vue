<template>
  <div class="q-pa-md">
    <div class="bg-white shadow-4 rounded-borders q-pa-md">
      <h2>Störungen</h2>
      <div v-if="loading" class="row justify-center">
          <q-spinner size="md" color="primary" />
      </div>
      <div v-if="!loading">
        <div v-if="disturbances.length === 0" class="text-center text-grey">
          Keine Störungen passend zum gesetzten Filter gefunden
        </div>
        <div v-for="disturbance in disturbances" :key="disturbance.id" @click="toggleDisturbance(disturbance.id)">
          <DisturbanceDetails :disturbance="disturbance" :expand="expandedDisturbances.includes(disturbance.id)" :getLineColor="getLineColor" />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue'
import DisturbanceDetails from './DisturbanceDetails.vue'

export default {
  name: 'FilterSortPanel',
  inject: ['$globals'],

  props: {
    getLineColor: Function
  },

  components: {
    DisturbanceDetails
  },

  methods: {
    toggleDisturbance (id) {
      if (this.expandedDisturbances.includes(id)) {
        this.expandedDisturbances.splice(this.expandedDisturbances.indexOf(id), 1)
      } else {
        this.expandedDisturbances.push(id)
      }
    },

    async update (params) {
      this.loading = true
      this.disturbances = await this.fetchDisturbances(params)
      this.loading = false
    },

    async fetchDisturbances (params) {
      if (params.types.length === 0 || params.lines.length === 0) {
        return []
      }
      try {
        // date parsing
        const fromDateArr = params.fromDate.split('.')
        const fromDate = `${fromDateArr[2]}-${fromDateArr[1]}-${fromDateArr[0]}`
        const toDateArr = params.toDate.split('.')
        const toDate = `${toDateArr[2]}-${toDateArr[1]}-${toDateArr[0]}`
        let url = `/disturbances?from=${fromDate}&to=${toDate}&${params.sort.value}types=${params.types.toString()}&lines=${params.lines.toString()}`
        if (params.onlyOpenDisturbances) {
          url += '&active=true'
        }
        return this.$globals.fetch(url)
      } catch (err) {
        console.log(err)
      }
      return []
    }
  },
  data () {
    return {
      disturbances: ref([]),
      loading: false,
      expandedDisturbances: ref([])
    }
  }
}
</script>

<style scoped>

</style>
