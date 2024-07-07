<template>
  <div class="q-pa-md">
    <q-card class="bg-white q-py-md">
      <q-card-section>
        <div class="text-h5">Störungen</div>
      </q-card-section>
      <q-card-section>
        <div v-if="loading">
          <q-card flat bordered v-for="n in 15" :key="n" class="q-my-sm">
            <q-card-section>
              <div class="row justify-between items-center">
                <div class="row">
                  <q-skeleton square type="QChip" width="40px" class="q-mr-xs" v-for="n in 3" :key="n" />
                </div>
                <q-skeleton type="text" width="100px" />
              </div>
              <div>
                <q-skeleton type="text" class="text-subtitle1" width="200px" />
              </div>
            </q-card-section>
            <q-separator inset />
            <q-skeleton square height="150px" />
          </q-card>
        </div>
        <div v-else>
          <div v-if="disturbances.length === 0" class="text-center text-grey">
            Keine Störungen passend zum gesetzten Filter gefunden
          </div>
          <!-- <q-virtual-scroll virtual-scroll-item-size="205" virtual-scroll-slice-size="50" :items="disturbances" v-slot="{ item }" :scroll-target="disturbances.length > 15 ? 'document.body' : undefined" ref="disturbanceScroll">
            <div :key="item.id" @click="item.descriptions.length > 1 && toggleDisturbance(item.id)">
              <DisturbanceDetails :disturbance="item" :expandable="item.descriptions.length > 1"
                :expand="expandedDisturbances.includes(item.id)" :getLineColor="getLineColor" :showLink="true" />
            </div>
          </q-virtual-scroll> -->
          <q-infinite-scroll @load="onLoad">
            <div v-for="item in shownDisturbances" :key="item.id" @click="item.descriptions.length > 1 && toggleDisturbance(item.id)">
              <DisturbanceDetails :disturbance="item" :expandable="item.descriptions.length > 1"
                :expand="expandedDisturbances.includes(item.id)" :getLineColor="getLineColor" :showLink="true" />
            </div>
          </q-infinite-scroll>
        </div>
      </q-card-section>
    </q-card>
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
    onLoad (index, done) {
      this.shownDisturbances.push(...this.disturbances.slice(index * 50, (index + 1) * 50))
      done()
    },

    toggleDisturbance (id) {
      if (this.expandedDisturbances.includes(id)) {
        this.expandedDisturbances.splice(this.expandedDisturbances.indexOf(id), 1)
      } else {
        this.expandedDisturbances.push(id)
      }
    },

    async update (params) {
      this.lastScrollPosition = window.html.scrollTop
      this.loading = true
      this.disturbances = await this.fetchDisturbances(params)
      this.shownDisturbances = this.disturbances.slice(0, 50)
    },

    async fetchDisturbances (params) {
      if (params.types.length === 0) {
        return []
      }
      try {
        // date parsing
        const fromDateArr = params.from.split('.')
        const fromDate = `${fromDateArr[2]}-${fromDateArr[1]}-${fromDateArr[0]}`
        const toDateArr = params.to.split('.')
        const toDate = `${toDateArr[2]}-${toDateArr[1]}-${toDateArr[0]}`
        const order = ['', 'desc=false&', 'order=end&', 'desc=false&order=end&'][params.order]
        let url = `/disturbances?from=${fromDate}&to=${toDate}&${order}types=${params.types.toString()}`
        if (params.lines.length > 0) {
          url += `&lines=${params.lines.toString()}`
        }
        if (params.onlyClosedDisturbances) {
          url += '&active=false'
        }
        return (await this.$globals.fetch(url)).disturbances
      } catch (err) {
        console.log(err)
      }
      return []
    }
  },

  data () {
    return {
      disturbances: [],
      shownDisturbances: [],
      loading: false,
      expandedDisturbances: ref([]),
      lastScrollPosition: 0
    }
  },

  watch: {
    disturbances () {
      this.loading = false
    }
  }
}
</script>

<style scoped>

</style>
