<template>
  <div :class="'rounded-borders q-pa-sm q-my-xs' + (disturbance.descriptions.length > 1 ? ' cursor-pointer' : '')"
    style="border: 2px solid #E0E0E0;">
    <div class="row justify-between">
      <div class="row items-center">
        <h4>{{disturbance.title}}</h4>
        <q-btn flat round icon="launch" size="sm" color="primary" :to="'/stoerung/' + disturbance.id" />
      </div>
      <q-btn flat round icon="expand_more" v-if="disturbance.descriptions.length > 1"
        :class="expand ? 'rotate-180' : ''" style="transition: .2s;" />
    </div>
    <div class="row justify-between items-center">
      <div class="row"><q-chip square :color="getLineColor(line.id, line.type)" text-color="white" v-for="line in disturbance.lines" :key="line.id">{{line.id}}</q-chip></div>
      <div class="text-primary" style="font-family:'Montserrat bold'">{{displayDate(disturbance.start_time, disturbance.end_time)}}</div>
    </div>
    <div class="q-my-sm">{{disturbance.descriptions[0].description}}</div>
    <div class="q-ml-md q-my-sm" v-if="expand">
      <div class="q-my-sm" v-for="description in disturbance.descriptions.slice(1)" :key="description">
        <span class="text-primary">Update {{convertTime(description.time)}}:</span>
        <p>{{description.description}}</p>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'DisturbanceDetails',

  props: {
    disturbance: Object,
    expand: Boolean,
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

    convertTime (time) {
      const timeRaw = time.split(' ')[1].split(':')
      return `${timeRaw[0]}:${timeRaw[1]}`
    }
  }
}
</script>

<style>

</style>
