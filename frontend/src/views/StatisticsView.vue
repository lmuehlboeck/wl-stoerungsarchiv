<template>
  <header>
    <HeaderNav :active-index="1" />
  </header>
  <main class="q-my-xl q-mx-md">
    <q-card class="q-mx-auto col-8 bg-white q-py-md" style="max-width: 800px">
      <q-card-section>
        <div class="text-h3">Statistik</div>
      </q-card-section>
      <q-card-section>
        <div class="text-grey">Noch in Arbeit...</div>
      </q-card-section>
    </q-card>
  </main>
</template>

<script>
import HeaderNav from '@/components/HeaderNav.vue'

export default {
  name: 'StatisticsView',

  components: {
    HeaderNav
  }
}
</script>

<style>

</style>
