<template>
  <header>
    <HeaderNav :active-index="3" />
  </header>
  <main class="q-my-xl q-mx-md">
    <div class="q-mx-auto col-8 bg-white shadow-4 rounded-borders q-pa-md" style="max-width: 800px">
      <div class="full-width">
        <h1>Impressum</h1>
        <p class="text-subtitle1">Leo Mühlböck<br>
        1210 Wien <br>
        Österreich</p>
        <p class="text-subtitle1">E-Mail: <a href="mailto:mail@byleo.net">mail@byleo.net</a></p>
        <h2>Datenschutzhinweis</h2>
        <p>Diese Website verwendet <b>keine</b> Cookies, weder Tracking-Cookies noch technische Cookies.</p>
        <h2>Datenquelle</h2>
        <p>Die Daten werden automatisch aus der <a href="https://www.wienerlinien.at/open-data">Open-Data-API der Wiener Linien</a> aufgezeichnet.</p>
        <p>Keine Garantie auf Vollständigkeit, Richtigkeit oder Integrität der Daten.</p>
      </div>
    </div>
  </main>
</template>

<script>
import HeaderNav from '@/components/HeaderNav.vue'

export default {
  name: 'ImprintView',

  components: {
    HeaderNav
  }
}
</script>

<style>

</style>
