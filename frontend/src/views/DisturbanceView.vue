<template>
  <header>
    <HeaderNav />
  </header>
  <main class="q-my-lg">
    <div class="q-mx-auto col-8 bg-white shadow-4 rounded-borders q-pa-md" style="max-width: 1100px">
      <div class="full-width">
        <h2>Störung {{$route.params.id}}</h2>
        <div v-if="loading" class="row justify-center">
            <q-spinner size="md" color="primary" />
        </div>
        <div v-if="!loading && disturbance == null" class="text-center text-grey">
          Diese Störung wurde nicht gefunden
        </div>
        <DisturbanceDetails v-if="!loading && disturbance !== null" :disturbance="disturbance" :expand="true" :getLineColor="getLineColor" />
      </div>
    </div>
  </main>
</template>

<script>
import DisturbanceDetails from '@/components/DisturbanceDetails.vue'
import HeaderNav from '@/components/HeaderNav.vue'

export default {
  name: 'HomeView',

  components: {
    DisturbanceDetails,
    HeaderNav
  },

  methods: {
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
    },

    async fetchDisturbance (id) {
      try {
        const res = await fetch(`https://wls.byleo.net/api/disturbances/${id}`)
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

  async created () {
    this.loading = true
    this.disturbance = await this.fetchDisturbance(this.$route.params.id)
    this.loading = false
  },

  data () {
    return {
      loading: false,
      disturbance: null
    }
  }
}
</script>

<style>

</style>
