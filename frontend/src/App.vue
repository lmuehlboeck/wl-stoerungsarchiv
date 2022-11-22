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
@font-face {
  font-family: "Montserrat";
  src: local("Montserrat"),   url(../public/fonts/Montserrat-Regular.ttf) format("truetype");
}

@font-face {
  font-family: "Montserrat bold";
  src: local("Montserrat bold"),   url(../public/fonts/Montserrat-SemiBold.ttf) format("truetype");
}

* {
  font-family: "Montserrat";
}

html {
  background-color: #F8F8F8;
  overflow-x: hidden;
}

.bold {
  font-family: "Montserrat bold";
}

h1 {
  font-size: 2rem !important;
  margin: 0;
}
h2 {
  font-size: 1.5rem !important;
  margin: 0 !important;
}
h3 {
  font-size: 1.125rem !important;
  margin-bottom: 0;
}
h4 {
  font-size: 1rem !important;
  margin: 0 !important;
  padding: 0 !important;
  font-family: "Montserrat bold";
}
</style>
