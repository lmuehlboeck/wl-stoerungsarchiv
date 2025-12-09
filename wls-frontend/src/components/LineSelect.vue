<template>
  <div class="row justify-between q-mt-md q-mb-sm">
    <div class="text-h6">Linien</div>
    <q-btn-group outline rounded>
      <q-btn
        outline
        text-color="black"
        size="sm"
        label="Alle"
        @click="selectLines()"
        class="col-6"
      />
      <q-btn
        outline
        textColor="black"
        size="sm"
        label="Keine"
        @click="deselectLines()"
        class="col-6"
      />
    </q-btn-group>
  </div>
  <div v-if="lineOptions.length === 0" class="text-center text-grey">
    Keine Verbindung zur API
  </div>
  <div v-for="type in $globals.LINE_TYPES" :key="type.type_id">
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
</template>

<script>
export default {
  name: "LineSelect",
  inject: ["$globals"],

  props: {
    lineOptions: {
      type: Array,
      required: true,
    },
    linesLoading: {
      type: Boolean,
      required: true,
    },
    lines: {
      type: Array,
      required: true,
    },
  },

  methods: {
    toggleLine(id) {
      const lines = this.lines;
      if (this.lines.includes(id)) {
        lines.splice(lines.indexOf(id), 1);
      } else {
        lines.push(id);
      }
      this.$emit("update:lines", lines);
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
      this.$emit("update:lines", [...new Set(lines)]);
    },
    deselectLines(type) {
      let lines = this.lines;
      if (type === undefined) {
        lines = [];
      } else {
        const toRemove = this.lineOptions
          .filter((l) => l.type === type)
          .map((l) => l.id);
        lines = this.lines.filter((l) => !toRemove.includes(l));
      }
      this.$emit("update:lines", lines);
    },
  },

  emits: ["update:lines"],
};
</script>
