<template>
  <HeaderNav :activeIndex="0" />
  <h1 class="text-primary">hallo</h1>
</template>

<script>
import HeaderNav from '../components/HeaderNav.vue'

export default {
  name: 'HomeView',

  components: {
    HeaderNav
  }
}
</script>

<style>

</style>
