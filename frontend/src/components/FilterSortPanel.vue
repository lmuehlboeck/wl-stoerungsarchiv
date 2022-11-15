<template>
  <div class="q-pa-md">
    <div class="bg-white shadow-4 rounded-borders q-pa-md">
      <div class="row justify-between"  @click="expand = !expand">
        <h2>
          <q-icon name="tune" color="primary" class="q-mr-sm" />
          Sortieren & Filtern
        </h2>
        <q-btn flat round :icon="expand ? 'expand_less' : 'expand_more'" size="lg" class="lt-md" @click="expand = !expand" />
      </div>
      <div :class="expand ? '' : 'gt-sm'">
        <q-checkbox v-model="onlyOpenDisturbances" label="Nur offene Störungen anzeigen" @update:model-value="emitData()" />
        <q-select v-model="sort" :options="sortOptions" label="Sortieren nach" class="q-mb-md" @update:model-value="emitData()" filled />
        <h3>Datum</h3>
        <div class="row">
          <DatePicker label="Von" :value="fromDate" class="col-6" @update:modelValue="updateFromDate" />
          <DatePicker label="Bis" :value="toDate" class="col-6" @update:modelValue="updateToDate"/>
        </div>
        <h3>Störungstypen</h3>
        <q-btn-group class="full-width q-mb-sm">
          <q-btn text-color="black" size="sm" label="Alle auswählen" @click="selectAllTypes(); emitData()" class="col-6" />
          <q-btn textColor="black" size="sm" label="Alle abwählen" @click="deselectAllTypes()" class="col-6" />
        </q-btn-group>
        <q-option-group v-model="types" :options="typeOptions" type="checkbox" class="q-ml-sm q-mb-sm" @update:model-value="emitData()" dense />
        <h3>Linien</h3>
        <q-btn-group class="full-width q-mb-sm">
          <q-btn text-color="black" size="sm" label="Alle auswählen" @click="selectAllLines(); emitData()" class="col-6" />
          <q-btn textColor="black" size="sm" label="Alle abwählen" @click="deselectAllLines()" class="col-6" />
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
  </div>
</template>

<script>
import DatePicker from './DatePicker.vue'
import { ref } from 'vue'

export default {
  name: 'FilterSortPanel',

  props: {
    getLineColor: Function
  },

  components: {
    DatePicker
  },

  methods: {
    validateDate (date) {
      return /^(((0?[1-9]|[12]\d|3[01])\.(0[13578]|[13578]|1[02])\.((1[6-9]|[2-9]\d)\d{2}))|((0?[1-9]|[12]\d|30)\.(0[13456789]|[13456789]|1[012])\.((1[6-9]|[2-9]\d)\d{2}))|((0?[1-9]|1\d|2[0-8])\.0?2\.((1[6-9]|[2-9]\d)\d{2}))|(29\.0?2\.((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$/.test(date)
    },

    updateFromDate (newValue) {
      if (this.validateDate(newValue) && newValue !== this.fromDate) {
        this.fromDate = newValue
        this.emitData()
      }
    },

    updateToDate (newValue) {
      if (this.validateDate(newValue) && newValue !== this.toDate) {
        this.toDate = newValue
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
      expand: false,
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
