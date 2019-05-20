import Vue from 'vue';
import Vuex, { StoreOptions } from 'vuex';
import GlobalModule from '@/stores/GlobalModule';
import DiModule from '@/stores/DiModule';

Vue.use(Vuex);

const options: StoreOptions<{}> = {
    strict: process.env.NODE_ENV !== 'production',
    modules: {
        GlobalModule,
        DiModule,
    }
};

export default new Vuex.Store(options);
