<template>
  <div class="q-pa-md">
    <div class="bg-white shadow-4 rounded-borders q-pa-md">
      <h2>Störungen</h2>
      <div v-if="loading" class="row justify-center">
          <q-spinner size="md" color="primary" />
      </div>
      <div v-if="!loading">
        <div v-for="disturbance in disturbances" :key="disturbance.id" class="rounded-borders q-pa-sm q-my-xs cursor-pointer" style="border: 2px solid #E0E0E0">
          <div class="row justify-between">
            <h4>{{disturbance.title}}</h4>
            <q-btn flat round icon="expand_more" />
          </div>
          <div class="row justify-between items-center">
            <div class="row"><q-chip square :color="getLineColor(line.id, line.type)" text-color="white" v-for="line in disturbance.lines" :key="line.id">{{line.id}}</q-chip></div>
            <div class="text-primary bold">{{displayDate(disturbance.start_time, disturbance.end_time)}}</div>
          </div>
          <div class="q-my-sm overflow-hidden" style="height: 63px">{{disturbance.descriptions[0].description}}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue'

export default {
  name: 'FilterSortPanel',

  props: {
    getLineColor: Function
  },

  methods: {
    displayDate (from, to) {
      const fromDateRaw = from.split(' ')[0].split('-')
      const fromDate = `${fromDateRaw[2]}.${fromDateRaw[1]}.${fromDateRaw[0]}`
      const fromTimeRaw = from.split(' ')[1].split(':')
      const fromTime = `${fromTimeRaw[0]}:${fromTimeRaw[1]}`

      if (to == null) {
        return `seit ${fromTime}`
      } else {
        const toDateRaw = to.split(' ')[0].split('-')
        const toDate = `${toDateRaw[2]}.${toDateRaw[1]}.${toDateRaw[0]}`
        const toTimeRaw = to.split(' ')[1].split(':')
        const toTime = `${toTimeRaw[0]}:${toTimeRaw[1]}`

        if (fromDate === toDate) {
          return `${fromDate}: ${fromTime} bis ${toTime}`
        } else {
          return `${fromDate} ${fromTime} bis ${toDate} ${toTime}`
        }
      }
    },

    async update (params) {
      this.loading = true
      this.disturbances = await this.fetchDisturbances(params)
      this.loading = false
    },

    async fetchDisturbances (params) {
      try {
        // date parsing
        const fromDateArr = params.fromDate.split('.')
        const fromDate = `${fromDateArr[2]}-${fromDateArr[1]}-${fromDateArr[0]}`
        const toDateArr = params.toDate.split('.')
        const toDate = `${toDateArr[2]}-${toDateArr[1]}-${toDateArr[0]}`

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
      disturbances: ref([]),
      loading: false
    }
  }
}
</script>

<style scoped>

</style>
