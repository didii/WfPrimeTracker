import Vue from 'vue';

Vue.config.performance = true;

import store from './store';
import router from './router';
import './modules';

import App from './App.vue';

new Vue({
    store,
    router,
    render: h => h(App),
}).$mount("#app");
