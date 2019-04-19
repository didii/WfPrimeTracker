<template>
    <div data-component="home" class="container-fluid">
        <div v-if="primeItems == null" class="loading-container">
            <i class="fas fa-spinner loading-icon"></i>
            Loading...
        </div>
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
            this.previousShowIngredients = show;
            return;
        }

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

<style lang="scss" scoped>
@keyframes spin {
    from {
        transform: rotate(0deg);
    }
    to {
        transform: rotate(360deg);
    }
}

.loading-container {
    text-align: center;
    padding: 1rem;
    .loading-icon {
        animation: spin 2s infinite linear;
    }
}
</style>