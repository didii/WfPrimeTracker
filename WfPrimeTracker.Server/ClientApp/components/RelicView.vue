<template>
    <div
        data-component="relicview"
        :class="`relic ${tierClass} ${isVaulted ? 'vaulted' : ''}`"
    >
        <span>
            {{ label }}
            <i :class="`fas fa-${iconName}`" :title="rarityLabel"></i>
        </span>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { RelicDrop } from '../models/RelicDrop';
import { RelicTier } from '../models/RelicTier.enum';
import { DropChance } from '../models/DropChance.enum';

@Component
export default class RelicView extends Vue {
    @Prop({ required: true }) public relicDrop!: RelicDrop;

    private get label(): string {
        return `${RelicTier.toString(this.relicDrop.relic.tier)} ${this.relicDrop.relic.name}`;
    }

    private get rarityLabel(): string {
        return DropChance.toString(this.relicDrop.dropChance);
    }

    private get tierClass(): string {
        return RelicTier.toClass(this.relicDrop.relic.tier);
    }

    private get isVaulted(): boolean {
        return this.relicDrop.relic.isVaulted;
    }

    private get iconName(): string {
        switch (this.relicDrop.dropChance) {
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
}
</script>

<style lang="scss" scoped>
@import "../vars";

.relic {
    position: relative;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: left;
    &.lith {
        background-image: url($lith-img-url);
    }
    &.meso {
        background-image: url($meso-img-url);
    }
    &.neo {
        background-image: url($neo-img-url);
    }
    &.axi {
        background-image: url($axi-img-url);
    }
    &.vaulted::after {
        content: "";
        position: absolute;
        top: 0;
        left: 0;
        height: 100%;
        width: 100%;
        background-color: rgba(255, 255, 255, 0.5);
        pointer-events: none;
    }
    span {
        margin-left: 30px;
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