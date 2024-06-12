import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { Quasar } from 'quasar'
import quasarUserOptions from './quasar-user-options'
import { globals } from './globals'

createApp(App)
  .provide('$globals', globals)
  .use(Quasar, quasarUserOptions)
  .use(router).mount('#app')
