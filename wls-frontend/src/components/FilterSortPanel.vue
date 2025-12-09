<template>
  <div class="q-pa-md">
    <q-card class="bg-white q-pa-sm">
      <q-card-section>
        <div
          class="row no-wrap justify-between items-center"
          @click="expand = !expand"
        >
          <div class="text-h5">
            <q-icon name="tune" color="primary" class="q-mr-sm" />
            Sortieren & Filtern
          </div>
          <q-btn
            flat
            round
            icon="expand_more"
            size="lg"
            :class="expand ? 'lt-md rotate-180' : 'lt-md'"
            style="transition: 0.2s"
          />
        </div>
        <div :class="expand ? 'q-mt-lg' : 'gt-sm q-mt-lg'">
          <q-checkbox
            :model-value="!!settings.onlyActive"
            label="Nur offene Störungen"
            @update:model-value="toggleOnlyActive()"
          />
          <q-select
            v-model="settings.orderBy"
            :options="$globals.ORDER_OPTIONS"
            label="Sortieren nach"
            class="q-mb-md"
            @update:model-value="emitData()"
            filled
          />
          <div class="text-h6 q-mb-sm">Datum</div>
          <DateRangePicker
            :from="settings.fromDate"
            :to="settings.toDate"
            class="col-6"
            @update:modelValue="updateDates"
          />
          
          <TypeSelect v-model:types="settings.types" />

          <LineSelect
            :lineOptions="lineOptions"
            :linesLoading="linesLoading"
            v-model:lines="settings.lines"
          />

          <div class="row q-mt-lg">
            <q-btn
              label="Zurücksetzen"
              color="primary"
              class="col"
              @click="reset"
              rounded
            />
          </div>
        </div>
      </q-card-section>
    </q-card>
  </div>
</template>

<script>
import DateRangePicker from "./DateRangePicker.vue";
import LineSelect from "./LineSelect.vue";
import TypeSelect from "./TypeSelect.vue";

export default {
  name: "FilterSortPanel",
  inject: ["$globals"],

  props: {
    defaultSettings: Object,
    lineOptions: Array,
    linesLoading: Boolean,
  },

  components: {
    DateRangePicker,
    TypeSelect,
    LineSelect,
  },

  methods: {
    toggleOnlyActive() {
      this.settings.onlyActive = !this.settings.onlyActive;
    },

    validateDate(date) {
      return /^(((0?[1-9]|[12]\d|3[01])\.(0[13578]|[13578]|1[02])\.((1[6-9]|[2-9]\d)\d{2}))|((0?[1-9]|[12]\d|30)\.(0[13456789]|[13456789]|1[012])\.((1[6-9]|[2-9]\d)\d{2}))|((0?[1-9]|1\d|2[0-8])\.0?2\.((1[6-9]|[2-9]\d)\d{2}))|(29\.0?2\.((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$/.test(
        date
      );
    },

    updateDates(newDates) {
      if (this.validateDate(newDates.from) && this.validateDate(newDates.to)) {
        this.settings.fromDate = newDates.from;
        this.settings.toDate = newDates.to;
      }
    },

    emitData() {
      if (this.settings.types.length === 0) return;
      this.$emit("change", {
        ...this.settings,
        orderBy: this.settings.orderBy.order_id,
      });
    },

    setSettings(settings) {
      if (!settings) return;
      this.settings = {
        orderBy: this.$globals.ORDER_OPTIONS[0],
        onlyActive: false,
        fromDate: this.$globals.defaultDate,
        toDate: this.$globals.defaultDate,
        types: this.$globals.TYPE_OPTIONS.map((t) => t.value),
        lines: [],
        ...settings,
        ...("orderBy" in settings && {
          orderBy: this.$globals.ORDER_OPTIONS.find((o) => o.order_id === settings.orderBy),
        }),
      };
    },

    reset() {
      this.setSettings({});
      this.emitData();
    },
  },

  emits: ["change"],

  data() {
    return {
      expand: false,
      settings: {
        orderBy: this.$globals.ORDER_OPTIONS[0],
        onlyActive: false,
        fromDate: this.$globals.defaultDate,
        toDate: this.$globals.defaultDate,
        types: this.$globals.TYPE_OPTIONS.map((t) => t.value),
        lines: [],
      },
    };
  },

  watch: {
    defaultSettings(newVal) {
      this.setSettings(newVal);
      this.emitData();
    },
    settings: {
      handler() {
        this.emitData();
      },
      deep: true,
    },
  },
};
</script>

<style scoped></style>
