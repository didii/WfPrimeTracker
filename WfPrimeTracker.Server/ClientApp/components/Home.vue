<template>
    <div data-component="home">
        <div v-if="primeItems == null">Loading...</div>
        <PrimeItemsContainerView
            v-else
            :primeItems="primeItems"
            :saveData="saveData"
        />
    </div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import GlobalModule from '@/stores/GlobalModule';
import { ISaveData } from '@/services/LoadService';
import { PrimeItem } from '@/models/PrimeItem';
import PrimeItemsContainerView from './PrimeItemsContainerView.vue';

@Component({
    components: {
        PrimeItemsContainerView,
    },
})
export default class Home extends Vue {
    private globalModule = getModule(GlobalModule);

    private primeItems: PrimeItem[] | null = null;
    private saveData: ISaveData | null = null;
    private previousShowIngredients: boolean | null = null;

    public mounted() {
        this.loadData().catch(err => console.error(err));
    }

    private async loadData(): Promise<void> {
        this.primeItems = await this.globalModule.primeItemService.getAll();
        this.saveData = this.globalModule.loadService.load(this.primeItems);
    }

    @Watch('saveData.globalOptions.showIngredients')
    private onShowIngredientsChanged(show: boolean) {
        if (this.previousShowIngredients == null || this.previousShowIngredients == show) {
            console.log('Skipping: ' + this.previousShowIngredients + ' vs ' + show);
            this.previousShowIngredients = show;
            return;
        }

        console.log('Updating: ' + this.previousShowIngredients + ' vs ' + show);
        for (const primeItemId in this.saveData!.primeItems) {
            this.saveData!.primeItems[primeItemId].showIngredients = show;
        }
        this.previousShowIngredients = show;
    }

    @Watch('saveData', { deep: true })
    private onSaveDataChanged(saveData: ISaveData) {
        this.globalModule.loadService.save(saveData);
    }
}
</script>