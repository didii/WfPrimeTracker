<template>
    <div data-component="test" class="container border">
        <PrimePartView :primePart="primePart" />
    </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { RelicDrop } from '@/models/RelicDrop';
import { DropChance } from '@/models/DropChance.enum';
import { Relic } from '@/models/Relic';
import { RelicTier } from '@/models/RelicTier.enum';
import { PrimePart } from '@/models/PrimePart';
import RelicContainerView from './RelicContainerView.vue';
import RelicView from './RelicView.vue';
import PrimePartView from './PrimePartView.vue';

@Component({
    components: {
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
}
</script>