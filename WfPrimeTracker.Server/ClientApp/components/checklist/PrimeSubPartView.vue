<template>
    <label
        data-component="primesubpartview"
        :class="`prime-sub-part ${isChecked ? 'checked' : ''}`"
    >
        <input type="checkbox" v-model="isChecked" :disabled="isParentChecked" />
        {{ primePartIngredient.primePart.name }} {{ index > 0 ? index + 1 : "" }}
        <i
            :class="`fas fa-${iconName}`"
            :title="dropChanceLabel"
        ></i>
    </label>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import GlobalModule from '@/stores/GlobalModule';
import { IPrimePartSaveData } from '@/services/LoadService';
import { PrimePart } from '@/models/PrimePart';
import { DropChance } from '@/models/DropChance.enum';
import { PrimePartIngredient } from '@/models/PrimePartIngredient';

@Component
export default class PrimeSubPartView extends Vue {
    private globalModule = getModule(GlobalModule);

    @Prop({ type: Object, required: true })
    private primePartIngredient!: PrimePartIngredient;
    @Prop({type: Object, required: true})
    public saveData!: IPrimePartSaveData;
    @Prop({ type: Number, required: true })
    private index!: number;
    @Prop({ type: Boolean, required: true })
    public isParentChecked!: boolean;

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

    private get isChecked(): boolean {
        return this.isParentChecked || this.saveData.isChecked[this.index];
    }
    private set isChecked(value: boolean) {
        this.saveData.isChecked[this.index] = value;
    }
}
</script>

<style lang="scss" scoped>
@import "../../vars";

.prime-sub-part {
    margin-bottom: 0;
    @extend %gradient-base;
    &.checked {
        @extend %gradient-checked;
    }
    .fas {
        &[title="Common"] {
            color: $common-color;
        }
        &[title="Uncommon"] {
            color: $uncommon-color;
        }
        &[title="Rare"] {
            color: $rare-color;
        }
    }
}
</style>