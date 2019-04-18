import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import Home from './components/Home.vue';
// import Mockup from './components/Mockup.vue';

Vue.use(VueRouter);

const routes: RouteConfig[] = [
    { path: '/', redirect: '/home' },
    { path: '/home', component: Home }
];

export default new VueRouter({ mode: 'history', routes: routes });