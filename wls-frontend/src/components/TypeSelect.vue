<template>
  <div class="row justify-between q-mb-sm">
    <div class="text-h6">Störungstypen</div>
    <q-btn-group outline rounded>
      <q-btn
        outline
        text-color="black"
        size="sm"
        label="Alle"
        @click="selectAllTypes()"
        class="col-6"
      />
      <q-btn
        outline
        textColor="black"
        size="sm"
        label="Keine"
        @click="deselectAllTypes()"
        class="col-6"
      />
    </q-btn-group>
  </div>
  <q-option-group
    :model-value="types"
    :options="$globals.TYPE_OPTIONS.filter(
      (t) => !exclude.includes(t.value)
    )"
    type="checkbox"
    class="q-ml-sm q-mb-sm"
    @update:model-value="update"
    dense
  />
</template>

<script>
export default {
  name: "TypeSelect",
  inject: ["$globals"],

  props: {
    types: {
      type: Object,
      required: true,
    },
    exclude: {
      type: Array,
      default: () => [],
    },
  },

  methods: {
    selectAllTypes() {
      const types = this.$globals.TYPE_OPTIONS.map((t) => t.value);
      this.$emit("update:types", types);
    },

    deselectAllTypes() {
      this.$emit("update:types", []);
    },

    update(types) {
      this.$emit("update:types", types);
    },
  },

  emits: ["update:types"],
}
</script>
