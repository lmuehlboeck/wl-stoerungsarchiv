<template>
  <div class="q-pa-md">
    <div class="bg-white shadow-4 rounded-borders q-pa-md">
      <h2>
        <q-icon name="tune" color="primary" class="q-mr-sm" />
        Sortieren & Filtern
      </h2>
      <q-checkbox v-model="onlyOpenDisturbances" label="Nur offene Störungen anzeigen" @update:model-value="emitData()" />
      <q-select v-model="sort" :options="sortOptions" label="Sortieren nach" class="q-mb-md" @update:model-value="emitData()" filled />
      <h3>Datum</h3>
      <div class="row">
        <DatePicker label="Von" :value="fromDate" class="col-6" @input="updateFromDate" ref="fromDate" />
        <DatePicker label="Bis" :value="toDate" class="col-6" @input="updateToDate" ref="toDate" />
      </div>
      <h3>Störungstypen</h3>
      <q-btn-group class="full-width q-mb-sm">
        <q-btn text-color="black" size="sm" label="Alle auswählen" @click="selectAllTypes(); emitData()" class="col-6" />
        <q-btn textColor="black" size="sm" label="Alle abwählen" @click="deselectAllTypes(); emitData()" class="col-6" />
      </q-btn-group>
      <q-option-group v-model="types" :options="typeOptions" type="checkbox" class="q-ml-sm q-mb-sm" @update:model-value="emitData()" dense />
      <h3>Linien</h3>
      <q-btn-group class="full-width q-mb-sm">
        <q-btn text-color="black" size="sm" label="Alle auswählen" @click="selectAllLines(); emitData()" class="col-6" />
        <q-btn textColor="black" size="sm" label="Alle abwählen" @click="deselectAllLines(); emitData()" class="col-6" />
      </q-btn-group>
      <div v-if="linesLoading" class="row justify-center">
        <q-spinner size="md" color="primary" />
      </div>
      <div class="row justify-center q-mr-xs" v-if="!linesLoading">
        <q-btn unelevated v-for="line in lineOptions" :key="line.id" :label="line.id" class="q-ml-xs q-mt-xs" style="width: 50px"
          :color="lines.includes(line.id) ? getLineColor(line.id, line.type) : 'grey-4'" @click="toggleLine(line.id); emitData()" />
      </div>
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
    updateFromDate () {
      console.log(this.$refs.fromDate.date)
      if (this.$refs.fromDate.validate()) {
        this.fromDate = this.$refs.fromDate.date
        this.emitData()
      }
    },

    updateToDate () {
      if (this.$refs.toDate.validate()) {
        this.toDate = this.$refs.toDate.date
        this.emitData()
      }
    },

    selectAllTypes () {
      this.types = ref(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13'])
    },

    deselectAllTypes () {
      this.types = ref([])
    },

    toggleLine (id) {
      if (this.lines.includes(id)) {
        this.lines.splice(this.lines.indexOf(id), 1)
      } else {
        this.lines.push(id)
      }
    },

    selectAllLines () {
      this.lines = []
      for (const line of this.lineOptions) {
        this.lines.push(line.id)
      }
    },

    deselectAllLines () {
      this.lines = []
    },

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

    emitData () {
      const data = {
        sort: this.sort,
        onlyOpenDisturbances: this.onlyOpenDisturbances,
        fromDate: this.fromDate,
        toDate: this.toDate,
        types: this.types,
        lines: this.lines
      }
      this.$emit('change', data)
    },

    async fetchLines () {
      try {
        const res = await fetch('https://wls.byleo.net/api/lines')
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
    this.lineOptions = await this.fetchLines()
    this.selectAllLines()
    this.linesLoading = false
    this.emitData()
  },

  emits: ['change'],

  data () {
    const defaultDate = new Date(Date.now()).toLocaleDateString('de-DE', { dateStyle: 'medium' })
    return {
      sort: ref({ label: 'Startzeit - neueste zuerst', value: '' }),
      sortOptions: [
        {
          label: 'Startzeit - neueste zuerst',
          value: ''
        },
        {
          label: 'Startzeit - älteste zuerst',
          value: 'desc=false&'
        },
        {
          label: 'Endzeit - neueste zuerst',
          value: 'sort=end&'
        },
        {
          label: 'Endzeit - älteste zuerst',
          value: 'sort=end&desc=false&'
        }
      ],
      onlyOpenDisturbances: ref(false),
      fromDate: ref(defaultDate),
      toDate: ref(defaultDate),
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
      ],
      lineOptions: [],
      linesLoading: true,
      lines: []
    }
  }
}
</script>

<style scoped>

</style>
