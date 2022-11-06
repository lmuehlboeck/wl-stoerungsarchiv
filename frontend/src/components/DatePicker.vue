<template>
  <div class="q-px-xs">
    <q-input filled @input="(event) => $emit('input')" v-model="date" mask="##.##.####" :label="label" ref="input"
      :rules="[v => /^(((0?[1-9]|[12]\d|3[01])\.(0[13578]|[13578]|1[02])\.((1[6-9]|[2-9]\d)\d{2}))|((0?[1-9]|[12]\d|30)\.(0[13456789]|[13456789]|1[012])\.((1[6-9]|[2-9]\d)\d{2}))|((0?[1-9]|1\d|2[0-8])\.0?2\.((1[6-9]|[2-9]\d)\d{2}))|(29\.0?2\.((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$/.test(v)]" lazy-rules>
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
    value: String
  },

  emits: ['update:model-value'],

  methods: {
    validate () {
      return this.$refs.input.validate()
    }
  },

  data () {
    return {
      date: ref(this.value)
    }
  }
}
</script>

<style>

</style>
