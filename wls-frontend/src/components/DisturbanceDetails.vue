<template>
  <q-card flat bordered :class="'q-my-sm' + (disturbance.descriptions.length > 1 ? ' cursor-pointer' : '')">
    <q-card-section>
      <div class="row justify-between items-center">
        <div>
          <q-icon name="warning" color="primary" size="24px" class="q-mr-sm" v-if="disturbance.end_time === null" />
          <q-chip square :style="`background: ${$globals.getLineColor(line)}; margin-top: 0; margin-left: 0;`" class="q-mr-xs" text-color="white" v-for="line in disturbance.lines" :key="line.id">
            {{ line.display_name }}
          </q-chip>
        </div>
        <div class="text-primary" style="font-family:'Montserrat bold'">{{displayDate(disturbance.start_time, disturbance.end_time)}}</div>
      </div>
      <div class="no-wrap row justify-between items-center">
        <div class="row items-center">
          <div class="text-subtitle1 bold">{{displayTitle(disturbance.title)}}</div>
          <q-btn flat round icon="launch" size="sm" color="primary" :to="'/stoerung/' + disturbance.id" v-if="showLink" />
        </div>
        <q-btn flat round icon="expand_more" v-if="expandable"
          :class="expand ? 'rotate-180' : ''" style="transition: .2s;" />
      </div>
    </q-card-section>
    <q-separator inset />
    <q-card-section>{{disturbance.descriptions[0].description}}</q-card-section>
    <q-card-section v-if="expand" class="q-pa-0" style="padding-bottom: 0 !important">
      <q-timeline style="margin: 0 !important">
        <q-timeline-entry
          :subtitle="'Update ' + convertTime(description.time)"
          v-for="description in disturbance.descriptions.slice(1)" :key="description">
          <div>
            {{ description.description }}
          </div>
        </q-timeline-entry>
      </q-timeline>
    </q-card-section>
  </q-card>
</template>

<script>
export default {
  name: 'DisturbanceDetails',
  inject: ['$globals'],

  props: {
    disturbance: Object,
    expandable: Boolean,
    expand: Boolean,
    showLink: Boolean
  },

  methods: {
    displayTitle (title) {
      if (!title.includes(':')) return title
      return title.substring(title.indexOf(':') + 1)
    },

    displayDate (from, to) {
      const fromDateRaw = from.split(' ')[0].split('-')
      let fromDate = `${fromDateRaw[2]}.${fromDateRaw[1]}.${fromDateRaw[0]}`
      if (fromDate === this.$globals.defaultDate) fromDate = 'heute'
      const fromTimeRaw = from.split(' ')[1].split(':')
      const fromTime = `${fromTimeRaw[0]}:${fromTimeRaw[1]}`
      if (to == null) {
        return `seit ${fromDate} ${fromTime}`
      } else {
        const toDateRaw = to.split(' ')[0].split('-')
        let toDate = `${toDateRaw[2]}.${toDateRaw[1]}.${toDateRaw[0]}`
        if (toDate === this.$globals.defaultDate) toDate = 'heute'
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
