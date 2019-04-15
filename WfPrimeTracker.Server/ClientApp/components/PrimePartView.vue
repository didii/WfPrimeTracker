<template>
    <div data-component="primepartview" class="prime-part m-0 w-100">
        <div class="row">
            <RelicCollapseButton
                :isOpen="showRelics"
                @click.native="onToggleCollapse"
                class="col-auto px-2 py-0"
            />
            <div class="col prime-sub-part-container">
                <PrimeSubPartView
                    v-for="index in primePart.count"
                    :key="index"
                    :primePart="primePart"
                    :index="index - 1"
                    :isParentChecked="false"
                    class="prime-sub-part w-100 m-0 px-2"
                />
            </div>
            <RelicContainerView
                v-if="showRelics"
                :relicDrops="primePart.relicDrops"
                class="relics-container col-12 col-md-5 mt-1"
            />
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { PrimePart } from '@/models/PrimePart';
import { DropChance } from '@/models/DropChance.enum';
import RelicCollapseButton from './utils/RelicCollapseButton.vue';
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
    @Prop({ required: true }) public primePart!: PrimePart;

    private get dropChance(): DropChance {
        return this.primePart.relicDrops[0].dropChance;
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

    private get isSelected(): boolean {
        return this.primePart.isChecked[0];
    }

    private get showRelics(): boolean {
        return this.primePart.showRelics;
    }

    private onToggleCollapse() {
        this.primePart.showRelics = !this.primePart.showRelics;
    }
}
</script>

<style lang="scss" scoped>
.prime-sub-part-container {
    display: flex;
    flex-direction: column;
    .prime-sub-part {
        flex: 1 0;
    }
}
</style>