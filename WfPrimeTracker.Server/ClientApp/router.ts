import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import Home from '@/components/Home.vue';
import About from '@/components/About.vue';

Vue.use(VueRouter);

const routes: RouteConfig[] = [
    { path: '/', redirect: '/home' },
    { path: '/home', component: Home },
    { path: '/about', component: About },
    { path: '*', redirect: '/home' },
];

export default new VueRouter({ mode: 'history', routes: routes });