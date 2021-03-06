<template>
    <div data-component="primeitemscontainerview" class="items-container py-2">
        <PrimeItemsOptions
            :globalOptions="saveData.globalOptions"
            @saveAnonymous="saveAnonymous"
            class="options"
        />
        <SummaryView :primeItems="primeItems" :saveData="saveData.primeItems" />
        <div v-if="primeItems.length" class="row">
            <div
                v-for="primeItem in primeItems"
                :key="primeItem.id"
                :id="primeItem.id"
                :class="['col-lg-6', 'col-12', 'p-2', isFiltered(primeItem) ? 'd-none' : '']"
                data-aos="fade-in"
            >
                <PrimeItemView
                    :primeItem="primeItem"
                    :saveData="primeItemSaveData(primeItem)"
                />
            </div>
        </div>
        <div v-else class="nothing">
            Nothing found!
        </div>
        <ToTheTop class="to-the-top" />
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import AOS from 'aos';
import GlobalModule from '@/stores/GlobalModule';
import { PrimeItem } from '@/models/PrimeItem';
import { ISaveData, IPrimeItemSaveData } from '@/models/SaveData';
import PrimeItemsOptions from './PrimeItemsOptions.vue';
import SummaryView from './SummaryView.vue';
import PrimeItemView from './PrimeItemView.vue';
import ToTheTop from './ToTheTop.vue';

@Component({
    components: {
        PrimeItemsOptions,
        SummaryView,
        PrimeItemView,
        ToTheTop,
    }
})
export default class PrimeItemsContainerView extends Vue {
    private globalModule = getModule(GlobalModule);

    @Prop({ type: Array, required: true }) public primeItems!: PrimeItem[];
    @Prop({ type: Object, required: true }) public saveData!: ISaveData;

    public updated() {
        // Force AOS to refresh so that display-none element are detected
        AOS.refresh();
    }

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

    private get isFiltered(): (primeItem: PrimeItem) => boolean {
        return item => {
            if (this.saveData.globalOptions.hideCompleted && this.saveData.primeItems[item.id].isChecked)
                return true;
            if (this.globalModule.searchQuery && !new RegExp(this.globalModule.searchQuery, 'i').test(item.name))
                return true;
            return false;
        }
    }

    private get primeItemSaveData(): (primeItem: PrimeItem) => IPrimeItemSaveData {
        return item => this.saveData.primeItems[item.id];
    }

    private saveAnonymous() {
        this.$emit('saveAnonymous');
    }
}
</script>

<style lang="scss" scoped>
.options {
    position: fixed;
    right: 0.5rem;
    z-index: 5;
}
.nothing {
    text-align: center;
    padding: 1rem;
}
.to-the-top {
    position: fixed;
    right: 10px;
    bottom: 10px;
}
</style>