<template>
    <div data-component="primeitemscontainerview" class="items-container py-2">
        <PrimeItemsOptions :globalOptions="saveData.globalOptions" class="options" />
        <div class="row">
            <div
                v-for="primeItem in filteredPrimeItems"
                :key="primeItem.id"
                :id="primeItem.id"
                class="col-lg-6 col-12 p-2"
            >
                <PrimeItemView
                    :primeItem="primeItem"
                    :saveData="primeItemSaveData(primeItem)"
                />
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import GlobalModule from '@/stores/GlobalModule';
import { ISaveData, IPrimeItemSaveData } from '@/services/LoadService';
import { PrimeItem } from '@/models/PrimeItem';
import PrimeItemView from './PrimeItemView.vue';
import PrimeItemsOptions from './PrimeItemsOptions.vue';

@Component({
    components: {
        PrimeItemsOptions,
        PrimeItemView,
    }
})
export default class PrimeItemsContainerView extends Vue {
    private globalModule = getModule(GlobalModule);

    @Prop({ type: Array, required: true }) public primeItems!: PrimeItem[];
    @Prop({ type: Object, required: true }) public saveData!: ISaveData;

    private get filteredPrimeItems(): PrimeItem[] {
        let result = this.primeItems;
        if (this.saveData.globalOptions.hideCompleted) {
            result = result.filter(i => !this.saveData.primeItems[i.id].isChecked);
        }
        if (this.globalModule.searchQuery) {
            result = result.filter(i => new RegExp(this.globalModule.searchQuery, 'i').test(i.name));
        }
        return result;
    }

    private get primeItemSaveData(): (primeItem: PrimeItem) => IPrimeItemSaveData {
        return item => this.saveData.primeItems[item.id];
    }
}
</script>

<style lang="scss" scoped>
.options {
    position: fixed;
    right: 0.5rem;
    z-index: 5;
}
</style>