<template>
    <div data-component="test" class="container">
        <ChecklistContainerView :primeItem="primeItem" class="red-border" />
    </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { RelicDrop } from '@/models/RelicDrop';
import { DropChance } from '@/models/DropChance.enum';
import { Relic } from '@/models/Relic';
import { RelicTier } from '@/models/RelicTier.enum';
import { PrimePart } from '@/models/PrimePart';
import { PrimeItem } from '@/models/PrimeItem';
import ChecklistContainerView from './ChecklistContainerView.vue';
import RelicContainerView from './RelicContainerView.vue';
import RelicView from './RelicView.vue';
import PrimePartView from './PrimePartView.vue';
import PrimePartContainerView from './PrimePartContainerView.vue';

@Component({
    components: {
        ChecklistContainerView,
        PrimePartContainerView,
        PrimePartView,
        RelicContainerView,
        RelicView,
    }
})
export default class Test extends Vue {
    private relicDrop: RelicDrop = new RelicDrop({
        id: 1,
        dropChance: DropChance.Common,
        relic: new Relic({
            id: 2,
            tier: RelicTier.Axi,
            name: 'A3',
            isVaulted: false,
            wikiUrl: 'https://warframe.fandom.com/wiki/Axi_A3',
        }),
    });
    private relicDrops: RelicDrop[] = [
        new RelicDrop({
            id: 1,
            dropChance: DropChance.Common,
            relic: new Relic({
                id: 2,
                tier: RelicTier.Lith,
                name: 'T4',
                isVaulted: false,
                wikiUrl: 'https://warframe.fandom.com/wiki/Axi_A3',
            }),
        }),
        new RelicDrop({
            id: 2,
            dropChance: DropChance.Uncommon,
            relic: new Relic({
                id: 4,
                tier: RelicTier.Axi,
                name: 'N2',
                isVaulted: false,
                wikiUrl: 'https://warframe.fandom.com/wiki/Axi_A3',
            }),
        }),
        new RelicDrop({
            id: 3,
            dropChance: DropChance.Uncommon,
            relic: new Relic({
                id: 6,
                tier: RelicTier.Meso,
                name: 'T11',
                isVaulted: false,
                wikiUrl: 'https://warframe.fandom.com/wiki/Axi_A3',
            }),
        }),
        new RelicDrop({
            id: 4,
            dropChance: DropChance.Rare,
            relic: new Relic({
                id: 8,
                tier: RelicTier.Neo,
                name: 'K1',
                isVaulted: false,
                wikiUrl: 'https://warframe.fandom.com/wiki/Axi_A3',
            }),
        }),
        new RelicDrop({
            id: 5,
            dropChance: DropChance.Rare,
            relic: new Relic({
                id: 2,
                tier: RelicTier.Neo,
                name: 'K5',
                isVaulted: false,
                wikiUrl: 'https://warframe.fandom.com/wiki/Axi_A3',
            }),
        }),
    ];
    private primePart: PrimePart = new PrimePart({
        id: 1,
        name: 'Blueprint',
        count: 2,
        relicDrops: this.relicDrops,
        isChecked: {
            0: false,
            1: false,
        },
        showRelics: false,
    });
    private primeParts: PrimePart[] = [
        new PrimePart({
            id: 2,
            name: 'Chassis',
            count: 1,
            relicDrops: this.relicDrops,
            isChecked: {
                0: true,
            },
            showRelics: true,
        }),
        this.primePart,
        new PrimePart({
            id: 3,
            name: 'Neuroptics',
            count: 1,
            relicDrops: this.relicDrops,
            isChecked: {
                0: false,
            },
            showRelics: false,
        }),
        new PrimePart({
            id: 4,
            name: 'Systems',
            count: 2,
            relicDrops: this.relicDrops,
            isChecked: {
                0: false,
                1: true,
            },
            showRelics: true,
        }),
    ];
    private primeItem: PrimeItem = new PrimeItem({
        id: 1,
        name: 'Banshee Prime',
        wikiUrl: 'https://warframe.fandom.com/wiki/Banshee/Prime',
        primeParts: this.primeParts,
        ingredients: [],
        credits: 25000,
        isChecked: false,
        isCollapsed: false,
    });
}
</script>

<style lang="scss" scoped>
.red-border {
    border: 1px solid red;
}
</style>