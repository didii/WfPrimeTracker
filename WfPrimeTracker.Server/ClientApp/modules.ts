// Libraries
import 'bootstrap';
import '@fortawesome/fontawesome-free';
import '@fortawesome/fontawesome-free/js/solid';
import '@fortawesome/fontawesome-free/js/brands';
import * as AOS from 'aos';
import 'aos/dist/aos.css';
AOS.init({
    offset: 0,
});

// Vue modules
import Vue from 'vue';
import VueLazyload from 'vue-lazyload';

Vue.use(VueLazyload, {
    listenEvents: ['scroll', 'wheel', 'mousewheel', 'resize', 'keyup']
});