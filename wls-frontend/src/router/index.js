import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import StatisticsView from '../views/StatisticsView.vue'
import InfosView from '../views/InfosView.vue'
import ImprintView from '../views/ImprintView.vue'
import DisturbanceView from '../views/DisturbanceView.vue'

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/statistik',
    name: 'statistics',
    component: StatisticsView
  },
  {
    path: '/infos',
    name: 'infos',
    component: InfosView
  },
  {
    path: '/impressum',
    name: 'imprint',
    component: ImprintView
  },
  {
    path: '/stoerung/:id',
    name: 'disturbance',
    component: DisturbanceView
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
