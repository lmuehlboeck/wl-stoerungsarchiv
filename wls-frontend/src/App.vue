<template>
  <router-view />
</template>

<script>
import { ref } from "vue";

export default {
  name: "LayoutDefault",

  setup() {
    return {
      leftDrawerOpen: ref(false),
    };
  },
};
</script>

<style>
@font-face {
  font-family: "Montserrat";
  src: local("Montserrat"),
    url(../public/fonts/Montserrat-Regular.ttf) format("truetype");
}

@font-face {
  font-family: "Montserrat bold";
  src: local("Montserrat bold"),
    url(../public/fonts/Montserrat-SemiBold.ttf) format("truetype");
}

* {
  font-family: "Montserrat";
}

html {
  background-color: #f8f8f8;
  overflow-x: hidden;
}

.bold {
  font-family: "Montserrat bold";
}
</style>
