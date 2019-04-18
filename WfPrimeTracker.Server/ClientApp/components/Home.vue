<template>
    <div data-component="home">
        <div v-if="primeItems == null">Loading...</div>
        <PrimeItemsContainerView v-else :primeItems="primeItems" />
    </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import GlobalModule from '../stores/GlobalModule';
import { PrimeItem } from '../models/PrimeItem';
import PrimeItemsContainerView from './PrimeItemsContainerView.vue';

@Component({
    components: {
        PrimeItemsContainerView,
    },
})
export default class Home extends Vue {
    private globalModule = getModule(GlobalModule);

    private primeItems: PrimeItem[] | null = null;

    public mounted() {
        this.loadData();
    }

    private async loadData(): Promise<void> {
        try {
            this.primeItems = await this.globalModule.loadService.load();
        }
        catch (err) {
            console.error(err);
            throw err;
        }
    }

}
</script>