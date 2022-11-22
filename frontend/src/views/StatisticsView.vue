<template>
  <header>
    <HeaderNav :active-index="1" />
  </header>
  <main class="q-my-xl q-mx-md">
    <div class="q-mx-auto col-8 bg-white shadow-4 rounded-borders q-pa-md" style="max-width: 800px">
      <div class="full-width">
        <h1>Statistik</h1>
        <p class="text-grey">Noch in Arbeit...</p>
      </div>
    </div>
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
