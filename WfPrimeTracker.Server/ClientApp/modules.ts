// Libraries
import 'bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import '@fortawesome/fontawesome-free';
import '@fortawesome/fontawesome-free/js/solid';

// Vue modules
import Vue from 'vue';
import VueLazyload from 'vue-lazyload';

Vue.use(VueLazyload, {
    listenEvents: ['scroll', 'wheel', 'mousewheel', 'resize', 'keyup']
});