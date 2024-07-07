<template>
  <header>
    <HeaderNav :active-index="3" />
  </header>
  <main class="q-my-xl q-mx-md">
    <q-card class="q-mx-auto col-8 q-py-md" style="max-width: 800px">
      <q-card-section >
        <div class="text-h3">Impressum</div>
      </q-card-section>
      <q-card-section>
        <div class="text-subtitle1">
          Leo Mühlböck<br>
          1210 Wien <br>
          Österreich <br>
          E-Mail: <a href="mailto:byleo@gmx.at">byleo@gmx.at</a>
        </div>
      </q-card-section>
      <q-card-section>
        <div class="text-h4">Datenschutzhinweis</div>
        <p>Diese Website verwendet <b>keine</b> Cookies, weder Tracking-Cookies noch technische Cookies.</p>
      </q-card-section>
      <q-card-section>
        <div class="text-h4">Datenquelle</div>
        Die Daten werden automatisch aus der <a href="https://www.wienerlinien.at/open-data">Open-Data-API der Wiener Linien</a> aufgezeichnet. <br>
        Keine Garantie auf Vollständigkeit, Richtigkeit oder Integrität der Daten.
      </q-card-section>
    </q-card>
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
