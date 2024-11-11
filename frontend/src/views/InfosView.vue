<template>
  <header>
    <HeaderNav :active-index="2" />
  </header>
  <main class="q-my-xl q-mx-md">
    <q-card class="q-mx-auto col-8 bg-white q-py-md" style="max-width: 800px">
      <q-card-section>
        <div class="text-h3">Infos</div>
      </q-card-section>
      <q-card-section>
        <div>Die Daten werden automatisch aus der <a href="https://www.wienerlinien.at/open-data">Open-Data-API der Wiener Linien</a>
        in eine Datenbank gespeichert und über eine eigene API zur Verfügung gestellt. Die API ist öffentlich zugänglich.
        Der Source-Code und eine Dokumentation zum gesamten Projekt sowie zur API befindet sich auf <a href="https://github.com/lmuehlboeck/wl-stoerungsarchiv">GitHub</a>.</div>
      </q-card-section>
      <q-card-section>
        <div class="text-h4">Motivation</div>
        <div>
          Die Idee dieses Projekts hatte ich schon lange, jedoch ist es mir erst recht spät in den Sinn gekommen, das ganze selbst umzusetzen.
          Ähnliche Websites existieren bereits, allerdings wollte ich das ganze mit meinen eigenen Vorstellungen (Filtern & Sortieren) umsetzen.
        </div>
        <p>
          Die Wiener Linien haben, so wie alle anderen ÖPNV-Betreiber auch, Störungen. Diese sind übersichtlich und aktuell auf der Website oder in der App einsehbar.
          Doch nur solange sie aktuell sind - danach werden sie gelöscht. Es gibt Situationen, wo ein Einsehen in vergangene Störungen sehr nützlich sein kann.
          Diese Funktionalität soll das Wiener Linien Störungsarchiv zur Verfügung stellen.
        </p>
      </q-card-section>
    </q-card>
  </main>
</template>

<script>
import HeaderNav from '@/components/HeaderNav.vue'

export default {
  name: 'InfosView',

  components: {
    HeaderNav
  }
}
</script>

<style>

</style>
