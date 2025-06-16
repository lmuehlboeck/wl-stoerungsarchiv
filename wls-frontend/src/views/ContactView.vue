<template>
  <header>
    <HeaderNav :active-index="2" />
  </header>
  <main class="q-my-xl q-mx-md">
    <q-card class="q-mx-auto col-8 q-pa-md" style="max-width: 800px">
      <q-card-section>
        <h1 class="text-h3">Kontakt</h1>
      </q-card-section>
      <q-card-section>
        Leo Mühlböck<br />
        1210 Wien <br />
        Österreich <br />
        E-Mail: <a href="mailto:mail@byleo.net">mail@byleo.net</a>
      </q-card-section>
      <q-card-section>
        <h2 class="text-h4 q-mb-md">Funktionsweise</h2>
        Die Daten werden automatisch aus der
        <a href="https://www.wienerlinien.at/open-data"
          >Open-Data-API der Wiener Linien</a
        >
        aufgezeichnet, um sie auch später noch abrufen zu können. <br />
        Diese Anwendung ist Open Source, der Quellcode befindet sich auf
        <a href="https://github.com/lmuehlboeck/wl-stoerungsarchiv">Github</a>.
        <br />
        Keine Garantie auf Vollständigkeit, Richtigkeit oder Integrität der
        Daten.
      </q-card-section>
      <q-card-section>
        <h2 class="text-h4 q-mb-md">Datenschutzhinweis</h2>
        <p>
          Diese Website verwendet <b>keine</b> Cookies, weder Tracking-Cookies
          noch technische Cookies.
        </p>
      </q-card-section>
    </q-card>
  </main>
</template>

<script>
import HeaderNav from "@/components/HeaderNav.vue";

export default {
  name: "ContactView",

  components: {
    HeaderNav,
  },
};
</script>

<style></style>
