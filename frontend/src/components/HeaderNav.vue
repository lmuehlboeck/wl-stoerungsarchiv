<template>
  <div class="row justify-between items-center q-px-xl q-py-sm bg-white">
    <div class="column items-center">
      <span class="text-uppercase text-primary text-bold text-h6 full-width text-center">Wiener Linien</span>
      <span class="text-uppercase text-subtitle1">Störungshistorie</span>
    </div>
    <nav>
      <ul class="gt-sm text-uppercase text-subtitle1 row" style="list-style: none" ref="desktopLinkList">
        <li><a class="q-py-sm q-px-md" href="/">Störungen</a></li>
        <li><a class="q-py-sm q-px-md" href="/statistik">Statistik</a></li>
        <li><a class="q-py-sm q-px-md" href="/infos">Infos</a></li>
        <li><a class="q-py-sm q-px-md" href="/impressum">Impressum</a></li>
      </ul>
      <q-btn flat round icon="menu" size="lg" class="lt-md" @click="showMenu = !showMenu" v-show="!showMenu" />
      <q-btn flat round icon="close" size="lg" class="lt-md" @click="showMenu = !showMenu" v-show="showMenu" />
    </nav>
  </div>
  <Transition>
    <div class="z-max lt-md absolute full-width" style="background-color:rgba(255, 255, 255, 0.9)" v-show="showMenu">
      <ul class="text-uppercase text-subtitle1 column q-pa-none q-ma-none text-center" style="list-style: none" ref="mobileLinkList">
        <li><a class="q-py-lg full-width block" href="/">Störungen</a></li>
        <li><a class="q-py-lg full-width block" href="/statistik">Statistik</a></li>
        <li><a class="q-py-lg full-width block" href="/infos">Infos</a></li>
        <li><a class="q-py-lg full-width block" href="/impressum">Impressum</a></li>
      </ul>
    </div>
  </Transition>
</template>

<script>
export default {
  name: 'HeaderNav',

  props: {
    activeIndex: Number
  },

  data () {
    return {
      showMenu: false
    }
  },

  mounted () {
    const desktopLis = this.$refs.desktopLinkList.children
    const mobileLis = this.$refs.mobileLinkList.children

    desktopLis[this.activeIndex].firstChild.classList.add('bg-primary')
    desktopLis[this.activeIndex].firstChild.classList.add('text-white')
    mobileLis[this.activeIndex].firstChild.classList.add('bg-primary')
    mobileLis[this.activeIndex].firstChild.classList.add('text-white')
  }
}
</script>

<style scoped>
a {
    color: inherit;
    text-decoration: none;
    transition: .3s;
}
a:hover {
    background-color: var(--q-primary);
    color: white;
}

/* menu transition */
.v-enter-active,
.v-leave-active {
  transition: transform .3s;
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
