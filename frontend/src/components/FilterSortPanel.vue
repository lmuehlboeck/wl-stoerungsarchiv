<template>
  <div class="q-pa-md">
    <div class="bg-white shadow-4 rounded-borders q-pa-md">
      <h2>
        <q-icon name="tune" color="primary" class="q-mr-sm" />
        Filtern & Sortieren
      </h2>
      <q-checkbox v-model="onlyOpenDisturbances" label="Nur offene Störungen anzeigen" />
      <h3>Datum</h3>
      <div class="row">
        <DatePicker label="Von" :defaultDate="getDate()" class="col-6" />
        <DatePicker label="Bis" :defaultDate="getDate()" class="col-6" />
      </div>
      <h3>Störungstypen</h3>
      <q-option-group v-model="types" :options="typeOptions" type="checkbox" />
    </div>
  </div>
</template>

<script>
import DatePicker from './DatePicker.vue'
import { ref } from 'vue'

export default {
  name: 'FilterSortPanel',

  components: {
    DatePicker
  },

  methods: {
    getDate () {
      const date = new Date(Date.now()).toLocaleDateString('de-DE', { dateStyle: 'medium' })
      console.log(date)
      return date
    }
  },

  data () {
    return {
      onlyOpenDisturbances: ref(false),
      types: ref(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13']),
      typeOptions: [
        {
          label: 'Verspätungen',
          value: '0'
        },
        {
          label: 'Verkehrsunfälle',
          value: '1'
        },
        {
          label: 'Schadhafte Fahrzeuge',
          value: '2'
        },
        {
          label: 'Gleisschäden',
          value: '3'
        },
        {
          label: 'Weichenstörungen',
          value: '4'
        },
        {
          label: 'Fahrleitungsgebrechen',
          value: '5'
        },
        {
          label: 'Signalstörungen',
          value: '6'
        },
        {
          label: 'Rettungseinsätze',
          value: '7'
        },
        {
          label: 'Polizeieinsätze',
          value: '8'
        },
        {
          label: 'Feuerwehreinsätze',
          value: '9'
        },
        {
          label: 'Falschparker',
          value: '10'
        },
        {
          label: 'Demonstrationen',
          value: '11'
        },
        {
          label: 'Veranstaltungen',
          value: '12'
        },
        {
          label: 'Sonstiges',
          value: '13'
        }
      ]
    }
  }
}
</script>

<style scoped>

</style>
