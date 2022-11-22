<template>
  <header>
    <HeaderNav :active-index="2" />
  </header>
  <main class="q-my-xl q-mx-md">
    <div class="q-mx-auto col-8 bg-white shadow-4 rounded-borders q-pa-md" style="max-width: 800px">
      <div class="full-width">
        <h1>Infos</h1>
        <p>Die Daten werden automatisch aus der <a href="https://www.wienerlinien.at/open-data">Open-Data-API der Wiener Linien</a>
        in eine Datenbank gespeichert und über eine eigene API zur Verfügung gestellt. Die API ist öffentlich zugänglich.
        Der Source-Code und eine Dokumentation zum gesamten Projekt sowie zur API befindet sich auf <a href="https://github.com/lmuehlboeck/wl-stoerungsarchiv">GitHub</a>.</p>
        <h2>Motivation</h2>
        <p>
          Die Idee dieses Projekts hatte ich schon lange, jedoch ist es mir erst recht spät in den Sinn gekommen, das ganze selbst umzusetzen.
          Ähnliche Websites exestieren bereits, allerdings haben sie entweder keine Filterfunktion, sind schlecht aufgebaut oder haben ein - naja - gewöhnungsbedürftiges Design.
          Das wollte ich ändern (wobei natürlich auch auf dieser Website viel Luft nach oben ist).
        </p>
        <p>
          Die Wiener Linien haben, so wie alle anderen ÖPNV-Betreiber auch, Störungen. Diese sind übersichtlich und aktuell auf der Website oder in der App einsehbar.
          Doch nur solange sie aktuell sind - danach werden sie gelöscht. Es gibt Situationen, wo ein Einsehen in vergangene Störungen sehr nütlich sein kann.
          Diese Funktionalität soll das Wiener Linien Störungsarchiv zur Verfügung stellen.
        </p>
      </div>
    </div>
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
