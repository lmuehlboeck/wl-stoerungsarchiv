<template>
  <router-view/>
</template>

<script>
import { ref } from 'vue'

export default {
  name: 'LayoutDefault',

  setup () {
    return {
      leftDrawerOpen: ref(false)
    }
  }
}
</script>

<style>
@import url('https://fonts.googleapis.com/css2?family=Montserrat&display=swap');

* {
  font-family: "Montserrat";
}

html {
  background-color: #F8F8F8;
  overflow-x: hidden;
}
</style>
