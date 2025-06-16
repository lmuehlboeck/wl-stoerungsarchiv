<template>
  <header>
    <HeaderNav />
  </header>
  <main class="q-my-xl q-mx-md">
    <q-card class="q-mx-auto col-8 q-py-md" style="max-width: 800px">
      <q-card-section class="full-width">
        <div class="text-h5">Störung {{ $route.params.id }}</div>
        <div v-if="loading" class="row justify-center">
          <q-spinner size="md" color="primary" />
        </div>
        <div
          v-if="!loading && disturbance == null"
          class="text-center text-grey"
        >
          Diese Störung wurde nicht gefunden
        </div>
        <DisturbanceDetails
          v-if="!loading && disturbance !== null"
          :disturbance="disturbance"
          :expand="true"
          :showLink="false"
          :expandable="false"
        />
      </q-card-section>
    </q-card>
  </main>
</template>

<script>
import DisturbanceDetails from "@/components/DisturbanceDetails.vue";
import HeaderNav from "@/components/HeaderNav.vue";

export default {
  name: "HomeView",
  inject: ["$globals"],

  components: {
    DisturbanceDetails,
    HeaderNav,
  },

  methods: {
    async fetchDisturbance(id) {
      try {
        return await this.$globals.fetch(`/disturbances/${id}`);
      } catch (err) {
        console.log(err);
      }
      return null;
    },
  },

  async created() {
    this.loading = true;
    this.disturbance = await this.fetchDisturbance(this.$route.params.id);
    this.loading = false;
  },

  data() {
    return {
      loading: false,
      disturbance: null,
    };
  },
};
</script>

<style></style>
