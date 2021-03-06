<template>
    <div data-component="primepartview" class="prime-part m-0 w-100">
        <div class="row">
            <RelicCollapseButton
                :isCollapsed="isCollapsed"
                @click.native="onToggleCollapse"
                class="col-auto px-2 py-0"
            />
            <div class="col prime-sub-part-checklist">
                <div class="row m-0">
                    <div class="col-auto image-container">
                        <img
                            v-lazy="imgUrl"
                            width="30"
                            :style="
                                isBlueprint ? { transform: 'scale(0.7)' } : null
                            "
                        />
                    </div>
                    <div class="col prime-sub-part-container">
                        <PrimeSubPartView
                            v-for="index in primePartIngredient.count"
                            :key="index"
                            :primePartIngredient="primePartIngredient"
                            :saveData="saveData"
                            :index="index - 1"
                            :isParentChecked="isParentChecked"
                            class="prime-sub-part w-100"
                        />
                    </div>
                </div>
            </div>
            <RelicContainerView
                v-if="!isCollapsed"
                :relicDrops="primePartIngredient.relicDrops"
                class="relics-container col-12 col-md-5 mt-1"
                data-aos="fade"
            />
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { IPrimePartSaveData, IPrimePartCheckedSaveData } from '@/services/LoadService';
import { PrimePart } from '@/models/PrimePart';
import { DropChance } from '@/models/DropChance.enum';
import { PrimePartIngredient } from '@/models/PrimePartIngredient';
import RelicCollapseButton from '@/components/utils/RelicCollapseButton.vue';
import PrimeSubPartView from './PrimeSubPartView.vue';
import RelicContainerView from './RelicContainerView.vue';

@Component({
    components: {
        RelicCollapseButton,
        PrimeSubPartView,
        RelicContainerView,
    },
})
export default class PrimePartView extends Vue {
    @Prop({ type: Object, required: true }) public primePartIngredient!: PrimePartIngredient;
    @Prop({ type: Object, required: true }) public saveData!: IPrimePartSaveData;
    @Prop({ type: Boolean, required: true }) public isParentChecked!: boolean;

    private get dropChance(): DropChance {
        return this.primePartIngredient.relicDrops[0].dropChance;
    }

    private get dropChanceLabel(): string {
        return DropChance.toString(this.dropChance);
    }

    private get iconName(): string {
        switch (this.dropChance) {
            case DropChance.Common:
                return 'angle-down';
            case DropChance.Uncommon:
                return 'angle-up';
            case DropChance.Rare:
                return 'angle-double-up';
            default:
                return 'times';
        }
    }

    private get imgUrl(): string {
        return `/api/primeparts/${this.primePartIngredient.primePart.id}/image`;
    }

    private get isBlueprint(): boolean {
        return this.primePartIngredient.primePart.name == 'Blueprint' || this.primePartIngredient.primePart.name == 'Collar Blueprint';
    }

    private get isCollapsed(): boolean {
        return this.saveData.isCollapsed;
    }

    private onToggleCollapse() {
        this.saveData.isCollapsed = !this.saveData.isCollapsed;
    }
}
</script>

<style lang="scss" scoped>
.prime-sub-part-checklist > .row {
    height: 100%;
    .image-container {
        display: flex;
        align-items: center;
    }
    .prime-sub-part-container {
        display: flex;
        flex-direction: column;
        .prime-sub-part {
            padding-left: 4px;
            flex: 1 0;
        }
    }
}
</style>