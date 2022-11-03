<template>
  <div class="q-px-xs">
    <q-input filled v-model="date" mask="##.##.####" :label="label">
      <template v-slot:append>
        <q-icon name="event" class="cursor-pointer">
          <q-popup-proxy cover transition-show="scale" transition-hide="scale">
            <q-date v-model="date" mask="DD.MM.YYYY">
              <div class="row items-center justify-end">
                <q-btn v-close-popup label="Close" color="primary" flat />
              </div>
            </q-date>
          </q-popup-proxy>
        </q-icon>
      </template>
    </q-input>
  </div>
</template>

<script>
import { ref } from 'vue'

export default {
  name: 'DatePicker',

  props: {
    label: String,
    defaultDate: String
  },

  data () {
    return {
      date: ref(this.defaultDate)
    }
  }
}
</script>

<style>

</style>
