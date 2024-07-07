<template>
  <div class="row q-gutter-sm q-mb-md">
    <q-input filled class="col" @update:modelValue="onInputChange"
       v-model="dates.from" mask="##.##.####" label="Von" :rules="inputRules" lazy-rules hide-bottom-space/>
    <q-input filled class="col" @update:modelValue="onInputChange"
       v-model="dates.to" mask="##.##.####" label="Bis" :rules="inputRules" lazy-rules hide-bottom-space/>
    <q-btn rounded icon="event" >
      <q-popup-proxy cover transition-show="scale" transition-hide="scale">
        <q-date range today-btn v-model="pickerDates" @update:modelValue="onPickerChange" mask="DD.MM.YYYY">
          <div class="row items-center justify-end">
            <q-btn v-close-popup label="Schließen" color="primary" flat />
          </div>
        </q-date>
      </q-popup-proxy>
    </q-btn>
  </div>
</template>

<script>
import { ref } from 'vue'

export default {
  name: 'DateRangePicker',

  props: {
    value: Object
  },

  emits: ['update:modelValue'],

  methods: {
    onInputChange (newDates) {
      if (newDates.from === newDates.to) {
        this.pickerDates = newDates.from
      } else {
        this.pickerDates = newDates
      }
      this.$emit('update:modelValue', newDates)
    },
    onPickerChange (newDates) {
      if (newDates === null) return
      if (typeof newDates === 'string') {
        this.dates = { from: newDates, to: newDates }
      } else {
        this.dates = newDates
      }
      this.$emit('update:modelValue', this.dates)
    }
  },

  data () {
    return {
      dates: ref(this.value),
      pickerDates: null,
      inputRules: [v => /^(((0?[1-9]|[12]\d|3[01])\.(0[13578]|[13578]|1[02])\.((1[6-9]|[2-9]\d)\d{2}))|((0?[1-9]|[12]\d|30)\.(0[13456789]|[13456789]|1[012])\.((1[6-9]|[2-9]\d)\d{2}))|((0?[1-9]|1\d|2[0-8])\.0?2\.((1[6-9]|[2-9]\d)\d{2}))|(29\.0?2\.((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$/.test(v)]
    }
  }
}
</script>

<style>

</style>
