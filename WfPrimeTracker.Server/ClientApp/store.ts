import Vue from 'vue';
import Vuex, { StoreOptions } from 'vuex';
import GlobalModule from './stores/GlobalModule';

Vue.use(Vuex);

const options: StoreOptions<{}> = {
    strict: process.env.NODE_ENV !== 'production',
    modules: {
        GlobalModule,
    }
};

export default new Vuex.Store(options);
