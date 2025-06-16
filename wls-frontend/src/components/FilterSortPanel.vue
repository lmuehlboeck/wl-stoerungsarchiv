<template>
  <div class="q-pa-md">
    <q-card class="bg-white q-py-md">
      <q-card-section
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
      </q-card-section>
      <q-card-section :class="expand ? '' : 'gt-sm'">
        <q-checkbox
          v-model="onlyActive"
          label="Nur offene Störungen"
          @update:model-value="emitData()"
        />
        <q-select
          v-model="orderBy"
          :options="ORDER_OPTIONS"
          label="Sortieren nach"
          class="q-mb-md"
          @update:model-value="emitData()"
          filled
        />
        <div class="text-h6 q-mb-sm">Datum</div>
        <DateRangePicker
          :value="dates"
          class="col-6"
          @update:modelValue="updateDates"
        />
        <div class="row justify-between q-mb-sm">
          <div class="text-h6">Störungstypen</div>
          <q-btn-group rounded>
            <q-btn
              text-color="black"
              size="sm"
              label="Alle"
              @click="selectAllTypes()"
              class="col-6"
            />
            <q-btn
              textColor="black"
              size="sm"
              label="Keine"
              @click="deselectAllTypes()"
              class="col-6"
            />
          </q-btn-group>
        </div>
        <q-option-group
          v-model="types"
          :options="TYPE_OPTIONS"
          type="checkbox"
          class="q-ml-sm q-mb-sm"
          @update:model-value="emitData()"
          dense
        />
        <div class="row justify-between q-mt-md q-mb-sm">
          <div class="text-h6">Linien</div>
          <q-btn-group rounded>
            <q-btn
              text-color="black"
              size="sm"
              label="Alle"
              @click="selectLines()"
              class="col-6"
            />
            <q-btn
              textColor="black"
              size="sm"
              label="Keine"
              @click="deselectLines()"
              class="col-6"
            />
          </q-btn-group>
        </div>
        <div v-if="lineOptions == null" class="text-center text-grey">
          Keine Verbindung zur API
        </div>
        <div v-for="type in LINE_TYPES" :key="type.type_id">
          <div class="row justify-between q-mt-md q-mb-sm">
            <div class="text-subtitle1">{{ type.title }}</div>
            <q-btn-group flat rounded>
              <q-btn
                text-color="black"
                size="sm"
                label="Alle"
                @click="selectLines(type.type_id)"
                class="col-6"
              />
              <q-btn
                textColor="black"
                size="sm"
                label="Keine"
                @click="deselectLines(type.type_id)"
                class="col-6"
              />
            </q-btn-group>
          </div>
          <div v-if="!linesLoading && lineOptions != null">
            <q-btn
              unelevated
              v-for="line in lineOptions.filter((l) => l.type === type.type_id)"
              :key="line.id"
              :label="line.displayName"
              class="q-ml-xs q-mt-xs"
              :style="`width: 50px; background: ${$globals.getLineColor(
                line
              )}; color: ${
                lines.includes(line.id) ? 'white' : $globals.getLineColor(line)
              };`"
              :outline="!lines.includes(line.id)"
              @click="toggleLine(line.id)"
            />
          </div>
          <div class="row" v-else>
            <q-skeleton
              type="QBtn"
              style="width: 50px; height: 36px"
              class="q-ml-xs q-mt-xs"
              v-for="n in 5"
              :key="n"
            />
          </div>
        </div>
        <div class="row q-mt-lg">
          <q-btn
            label="Zurücksetzen"
            color="primary"
            class="col"
            @click="reset"
            rounded
          />
        </div>
      </q-card-section>
    </q-card>
  </div>
</template>

<script>
import DateRangePicker from "./DateRangePicker.vue";
import { ref } from "vue";

const LINE_TYPES = [
  {
    type_id: "Metro",
    title: "U-Bahn",
  },
  {
    type_id: "Tram",
    title: "Straßenbahn",
  },
  {
    type_id: "Bus",
    title: "Bus",
  },
  {
    type_id: "Night",
    title: "Nightline",
  },
  {
    type_id: "Misc",
    title: "Eingestellt & andere",
  },
];
const ORDER_OPTIONS = [
  {
    label: "Startzeit - neueste zuerst",
    order_id: "StartedAtDesc",
  },
  {
    label: "Startzeit - älteste zuerst",
    order_id: "StartedAtAsc",
  },
  {
    label: "Endzeit - neueste zuerst",
    order_id: "EndedAtDesc",
  },
  {
    label: "Endzeit - älteste zuerst",
    order_id: "EndedAtAsc",
  },
];
const TYPE_OPTIONS = [
  {
    label: "Verspätungen",
    value: "Delay",
  },
  {
    label: "Verkehrsunfälle",
    value: "Accident",
  },
  {
    label: "Rettungseinsätze",
    value: "AmbulanceOperation",
  },
  {
    label: "Feuerwehreinsätze",
    value: "FireDepartmentOperation",
  },
  {
    label: "Polizeieinsätze",
    value: "PoliceOperation",
  },
  {
    label: "Falschparker",
    value: "ParkingOffender",
  },
  {
    label: "Schadhafte Fahrzeuge",
    value: "DefectiveVehicle",
  },
  {
    label: "Fahrleitungsschäden",
    value: "CatenaryDamage",
  },
  {
    label: "Gleisschäden",
    value: "TrackDamage",
  },
  {
    label: "Signalstörungen",
    value: "SignalDamage",
  },
  {
    label: "Weichenstörungen",
    value: "SwitchDamage",
  },
  {
    label: "Bauarbeiten",
    value: "ConstructionWork",
  },
  {
    label: "Demonstrationen",
    value: "Demonstration",
  },
  {
    label: "Veranstaltungen",
    value: "Event",
  },
  {
    label: "Witterungsbedingt",
    value: "Weather",
  },
  {
    label: "Sonstiges",
    value: "Misc",
  },
];

export default {
  name: "FilterSortPanel",
  inject: ["$globals"],

  props: {
    defaultParams: Object,
  },

  components: {
    DateRangePicker,
  },

  methods: {
    validateDate(date) {
      return /^(((0?[1-9]|[12]\d|3[01])\.(0[13578]|[13578]|1[02])\.((1[6-9]|[2-9]\d)\d{2}))|((0?[1-9]|[12]\d|30)\.(0[13456789]|[13456789]|1[012])\.((1[6-9]|[2-9]\d)\d{2}))|((0?[1-9]|1\d|2[0-8])\.0?2\.((1[6-9]|[2-9]\d)\d{2}))|(29\.0?2\.((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$/.test(
        date
      );
    },

    updateDates(newDates) {
      if (this.validateDate(newDates.from) && this.validateDate(newDates.to)) {
        this.dates = newDates;
        this.emitData();
      }
    },

    selectAllTypes() {
      this.types = ref(this.TYPE_OPTIONS.map((t) => t.value));
      this.emitData();
    },

    deselectAllTypes() {
      this.types = ref([]);
    },

    toggleLine(id) {
      if (this.lines.includes(id)) {
        this.lines.splice(this.lines.indexOf(id), 1);
      } else {
        this.lines.push(id);
      }
      this.emitData();
    },

    selectLines(type) {
      let lines = this.lines;
      if (type === undefined) {
        lines = lines.concat(this.lineOptions.map((l) => l.id));
      } else {
        lines = lines.concat(
          this.lineOptions.filter((l) => l.type === type).map((l) => l.id)
        );
      }
      this.lines = [...new Set(lines)];
      this.emitData();
    },

    deselectLines(type) {
      if (type === undefined) {
        this.lines = [];
      } else {
        const toRemove = this.lineOptions
          .filter((l) => l.type === type)
          .map((l) => l.id);
        this.lines = this.lines.filter((l) => !toRemove.includes(l));
        this.emitData();
      }
    },

    emitData() {
      const data = {
        orderBy: this.orderBy.order_id,
        onlyActive: this.onlyActive,
        fromDate: this.dates.from,
        toDate: this.dates.to,
        types: this.types,
        lines: this.lines,
      };
      this.$emit("change", data);
    },

    async fetchLines() {
      try {
        return await this.$globals.fetch("/lines");
      } catch (err) {
        console.log(err);
      }
      return null;
    },

    reset() {
      this.orderBy = this.ORDER_OPTIONS[0];
      this.onlyActive = false;
      this.dates.from = this.$globals.defaultDate;
      this.dates.to = this.$globals.defaultDate;
      this.types = this.TYPE_OPTIONS.map((t) => t.value);
      this.lines = [];
      this.emitData();
    },
  },

  async created() {
    this.LINE_TYPES = LINE_TYPES;
    this.ORDER_OPTIONS = ORDER_OPTIONS;
    this.TYPE_OPTIONS = TYPE_OPTIONS;

    this.lineOptions = await this.fetchLines();
    if (this.defaultParams !== undefined) {
      if ("orderBy" in this.defaultParams)
        this.orderBy = this.ORDER_OPTIONS.find(
          (option) => option.order_id === this.defaultParams.orderBy
        );
      if ("onlyActive" in this.defaultParams)
        this.onlyActive = this.defaultParams.onlyActive === "true";
      if ("fromDate" in this.defaultParams)
        this.dates.from = this.defaultParams.fromDate;
      if ("toDate" in this.defaultParams)
        this.dates.to = this.defaultParams.toDate;
      if ("types" in this.defaultParams) this.types = this.defaultParams.types;
      if ("lines" in this.defaultParams) this.lines = this.defaultParams.lines;
    }
    if (this.lineOptions != null) {
      this.emitData();
    }
    this.linesLoading = false;
  },

  emits: ["change"],

  data() {
    return {
      expand: false,
      orderBy: ORDER_OPTIONS[0],
      onlyActive: false,
      dates: { from: this.$globals.defaultDate, to: this.$globals.defaultDate },
      types: TYPE_OPTIONS.map((t) => t.value),
      lineOptions: [],
      linesLoading: true,
      lines: [],
    };
  },
};
</script>

<style scoped></style>
