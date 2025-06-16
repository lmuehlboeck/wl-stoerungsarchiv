<template>
  <div
    class="row justify-between items-center q-px-lg q-py-sm bg-white shadow-up-2"
  >
    <div class="column items-center">
      <span
        class="text-uppercase text-primary text-bold text-h6 full-width text-center"
        >Wiener Linien</span
      >
      <span class="text-uppercase text-subtitle1">Störungsarchiv</span>
    </div>
    <nav>
      <ul
        class="gt-sm text-uppercase text-subtitle1 row"
        style="list-style: none"
        ref="desktopLinkList"
      >
        <li>
          <RouterLink class="q-py-sm q-px-md q-ml-xs" to="/"
            >Störungen</RouterLink
          >
        </li>
        <li>
          <RouterLink class="q-py-sm q-px-md q-ml-xs" to="/statistik"
            >Statistik</RouterLink
          >
        </li>
        <li>
          <RouterLink class="q-py-sm q-px-md q-ml-xs" to="/infos"
            >Infos</RouterLink
          >
        </li>
        <li>
          <RouterLink class="q-py-sm q-px-md q-ml-xs" to="/impressum"
            >Impressum</RouterLink
          >
        </li>
      </ul>
      <q-btn
        flat
        round
        :icon="showMenu ? 'close' : 'menu'"
        size="lg"
        class="lt-md"
        @click="showMenu = !showMenu"
      />
    </nav>
  </div>
  <Transition>
    <div
      class="z-max lt-md absolute full-width"
      style="background-color: rgba(255, 255, 255, 0.96)"
      v-show="showMenu"
    >
      <ul
        class="text-uppercase text-subtitle1 column q-pa-none q-ma-none text-center"
        style="list-style: none"
        ref="mobileLinkList"
      >
        <li>
          <RouterLink class="q-py-lg full-width block" to="/"
            >Störungen</RouterLink
          >
        </li>
        <li>
          <RouterLink class="q-py-lg full-width block" to="/statistik"
            >Statistik</RouterLink
          >
        </li>
        <li>
          <RouterLink class="q-py-lg full-width block" to="/infos"
            >Infos</RouterLink
          >
        </li>
        <li>
          <RouterLink class="q-py-lg full-width block" to="/impressum"
            >Impressum</RouterLink
          >
        </li>
      </ul>
    </div>
  </Transition>
</template>

<script>
export default {
  name: "HeaderNav",

  props: {
    activeIndex: Number,
  },

  data() {
    return {
      showMenu: false,
    };
  },

  mounted() {
    if (this.activeIndex != null) {
      const desktopLis = this.$refs.desktopLinkList.children;
      const mobileLis = this.$refs.mobileLinkList.children;

      desktopLis[this.activeIndex].firstChild.classList.add("bg-primary");
      desktopLis[this.activeIndex].firstChild.classList.add("text-white");
      mobileLis[this.activeIndex].firstChild.classList.add("bg-primary");
      mobileLis[this.activeIndex].firstChild.classList.add("text-white");
    }
  },
};
</script>

<style scoped>
a {
  color: inherit;
  text-decoration: none;
  transition: 0.3s;
}
a:hover {
  background-color: var(--q-primary);
  color: white;
}

/* menu transition */
.v-enter-active,
.v-leave-active {
  transition: transform 0.3s;
}

.v-enter-from,
.v-leave-to {
  transform: translateX(100%);
}

.v-enter-to,
.v-leave-from {
  transform: translateX(0);
}
</style>
