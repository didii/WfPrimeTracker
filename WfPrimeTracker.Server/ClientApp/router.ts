import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
// import Home from './components/Home.vue';
// import Mockup from './components/Mockup.vue';
import Test from './components/Test.vue';

Vue.use(VueRouter);

const routes: RouteConfig[] = [
    { path: '/', redirect: '/home' },
    { path: '/home', component: Test }
    // { path: '/', component: Home },
    // { path: '/mockup', component: Mockup },
    // { path: '/test', component: Test }
];

export default new VueRouter({ mode: 'history', routes: routes });