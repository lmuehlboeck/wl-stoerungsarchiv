<template>
  <div class="q-pa-md">
    <q-card class="bg-white q-pa-sm">
      <q-card-section>
        <h1 class="text-h5 q-mb-lg">Störungen</h1>
        <div v-if="loading">
          <q-card flat bordered v-for="n in 15" :key="n" class="q-my-sm">
            <q-card-section>
              <div class="row justify-between items-center">
                <div class="row">
                  <q-skeleton
                    square
                    type="QChip"
                    width="40px"
                    class="q-mr-xs"
                    v-for="n in 3"
                    :key="n"
                  />
                </div>
                <q-skeleton type="text" width="100px" />
              </div>
              <div>
                <q-skeleton type="text" class="text-subtitle1" width="200px" />
              </div>
              <q-skeleton square height="100px" />
            </q-card-section>
          </q-card>
        </div>
        <div>
          <div v-if="disturbances.length === 0" class="text-center text-grey">
            Keine Störungen passend zum gesetzten Filter gefunden
          </div>
          <q-infinite-scroll @load="onLoad">
            <div
              v-for="item in shownDisturbances"
              :key="item.id"
              @click="
                item.descriptions.length > 1 && toggleDisturbance(item.id)
              "
            >
              <DisturbanceDetails
                :disturbance="item"
                :expandable="item.descriptions.length > 1"
                :expand="expandedDisturbances.includes(item.id)"
                :showLink="true"
              />
            </div>
          </q-infinite-scroll>
        </div>
      </q-card-section>
    </q-card>
  </div>
</template>

<script>
import { ref } from "vue";
import DisturbanceDetails from "./DisturbanceDetails.vue";

export default {
  name: "FilterSortPanel",
  inject: ["$globals"],

  props: {
    loading: Boolean,
    disturbances: Array
  },

  components: {
    DisturbanceDetails,
  },

  methods: {
    onLoad(index, done) {
      this.shownDisturbances.push(
        ...this.disturbances.slice(index * 50, (index + 1) * 50)
      );
      done();
    },

    toggleDisturbance(id) {
      if (this.expandedDisturbances.includes(id)) {
        this.expandedDisturbances.splice(
          this.expandedDisturbances.indexOf(id),
          1
        );
      } else {
        this.expandedDisturbances.push(id);
      }
    },
  },

  data() {
    return {
      shownDisturbances: [],
      expandedDisturbances: ref([]),
    };
  },

  watch: {
    disturbances() {
      this.shownDisturbances = this.disturbances.slice(0, 50);
    },
  },
};
</script>

<style scoped></style>
