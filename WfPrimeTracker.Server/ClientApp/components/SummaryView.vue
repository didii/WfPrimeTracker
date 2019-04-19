<template>
    <div data-component="summaryview" class="summary-container border">
        <span
            v-for="primeItem in primeItems"
            :key="primeItem.id"
            :class="{
                badge: true,
                'badge-success': isCompleted(primeItem),
                'badge-secondary': !isCompleted(primeItem)
            }"
        >
            {{ shortName(primeItem) }}
        </span>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { PrimeItem } from '@/models/PrimeItem';
import { IPrimeItemsSaveData } from '@/services/LoadService';

@Component
export default class SummaryView extends Vue {
    @Prop({ type: Array, required: true }) public primeItems!: PrimeItem[];
    @Prop({ type: Object, required: true }) public saveData!: IPrimeItemsSaveData;

    public get shortName(): (primeItem: PrimeItem) => string {
        return item => item.name.replace(/ Prime/, '');
    }
    public get completedItems(): PrimeItem[] {
        return this.primeItems.filter(i => this.saveData[i.id].isChecked);
    }
    public get isCompleted(): (primeItem: PrimeItem) => boolean {
        return item => this.saveData[item.id].isChecked;
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