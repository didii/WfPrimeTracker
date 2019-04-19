<template>
    <div
        data-component="primeitemscontainerview"
        class="items-container py-2 row"
    >
        <div
            v-for="primeItem in filteredPrimeItems"
            :key="primeItem.id"
            :id="primeItem.id"
            class="col-lg-6 col-12 p-2"
        >
            <PrimeItemView :primeItem="primeItem" :saveData="primeItemSaveData(primeItem)" />
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

@Component({
    components: {
        PrimeItemView,
    }
})
export default class PrimeItemsContainerView extends Vue {
    private globalModule = getModule(GlobalModule);

    @Prop({ type: Array, required: true }) public primeItems!: PrimeItem[];
    @Prop({ type: Object, required: true }) public saveData!: ISaveData;

    private get filteredPrimeItems(): PrimeItem[] {
        if (this.globalModule.searchQuery) {
            return this.primeItems.filter(i => new RegExp(this.globalModule.searchQuery, 'i').test(i.name));
        }
        return this.primeItems;
    }

    private get primeItemSaveData(): (primeItem: PrimeItem) => IPrimeItemSaveData {
        return item => this.saveData.primeItems[item.id];
    }
}
</script>