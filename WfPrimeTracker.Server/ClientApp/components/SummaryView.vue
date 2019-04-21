<template>
    <div data-component="summaryview" class="summary-container border">
        <button
            v-for="primeItem in primeItems"
            :key="primeItem.id"
            @click="() => onClick(primeItem)"
            :class="{
                btn: true,
                badge: true,
                'badge-success': isCompleted(primeItem),
                'badge-secondary': !isCompleted(primeItem)
            }"
        >
            {{ shortName(primeItem) }}
        </button>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import GlobalModule from '@/stores/GlobalModule';
import { PrimeItem } from '@/models/PrimeItem';
import { IPrimeItemsSaveData } from '@/services/LoadService';

@Component
export default class SummaryView extends Vue {
    private globalModule = getModule(GlobalModule);

    @Prop({ type: Array, required: true }) public primeItems!: PrimeItem[];
    @Prop({ type: Object, required: true }) public saveData!: IPrimeItemsSaveData;

    private get shortName(): (primeItem: PrimeItem) => string {
        return item => item.name.replace(/ Prime/, '');
    }
    private get completedItems(): PrimeItem[] {
        return this.primeItems.filter(i => this.saveData[i.id].isChecked);
    }
    private get isCompleted(): (primeItem: PrimeItem) => boolean {
        return item => this.saveData[item.id].isChecked;
    }
    private onClick(primeItem: PrimeItem) {
        const element = document.getElementById('' + primeItem.id);
        if (element != null) {
            element.scrollIntoView({block: 'center'});
        }
        this.globalModule.highlight(primeItem.id);
    }
}
</script>

<style lang="scss" scoped>
.summary-container {
    padding: 0.5rem;
    .badge {
        font-size: 12px;
        margin-right: 2px;
        transition: all 250ms linear;
        transition-property: background-color, opacity;
        &.badge-secondary {
            opacity: 0.7;
        }
    }
}
</style>