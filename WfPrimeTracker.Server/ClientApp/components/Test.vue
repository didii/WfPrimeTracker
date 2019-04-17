<template>
    <div data-component="test" class="container-fluid">
        <div v-if="primeItems == null">Loading...</div>
        <div v-else>
            <PrimeItemsContainerView :primeItems="primeItems" />
        </div>
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
import { PrimeItemService } from '@/services/PrimeItemService';
import { LoadService } from '@/services/LoadService';
import { PrimePartIngredient } from '@/models/PrimePartIngredient';
import { IngredientsGroup } from '@/models/IngredientsGroup';
import { ResourceIngredient } from '@/models/ResourceIngredient';
import { Resource } from '@/models/Resource';
import ChecklistContainerView from './ChecklistContainerView.vue';
import RelicContainerView from './RelicContainerView.vue';
import RelicView from './RelicView.vue';
import PrimePartView from './PrimePartView.vue';
import PrimePartContainerView from './PrimePartContainerView.vue';
import InfoContainerView from './InfoContainerView.vue';
import IngredientsContainerView from './IngredientsContainerView.vue';
import IngredientView from './IngredientView.vue';
import PrimeItemsContainerView from './PrimeItemsContainerView.vue';
import PrimeItemView from './PrimeItemView.vue';

@Component({
    components: {
        PrimeItemsContainerView,
        PrimeItemView,
        InfoContainerView,
        IngredientsContainerView,
        IngredientView,
        ChecklistContainerView,
        PrimePartContainerView,
        PrimePartView,
        RelicContainerView,
        RelicView,
    }
})
export default class Test extends Vue {
    private relicDrops: RelicDrop[] = [
        {
            id: 1,
            dropChance: DropChance.Common,
            relic: {
                id: 1,
                tier: RelicTier.Axi,
                name: 'A3',
                isVaulted: false,
                wikiUrl: 'https://warframe.fandom.com/wiki/Axi_A3',
            },
        },
        {
            id: 2,
            dropChance: DropChance.Uncommon,
            relic: {
                id: 4,
                tier: RelicTier.Axi,
                name: 'N2',
                isVaulted: false,
                wikiUrl: 'https://warframe.fandom.com/wiki/Axi_A3',
            },
        },
        {
            id: 3,
            dropChance: DropChance.Uncommon,
            relic: {
                id: 6,
                tier: RelicTier.Meso,
                name: 'T11',
                isVaulted: false,
                wikiUrl: 'https://warframe.fandom.com/wiki/Axi_A3',
            },
        },
        {
            id: 4,
            dropChance: DropChance.Rare,
            relic: {
                id: 8,
                tier: RelicTier.Neo,
                name: 'K1',
                isVaulted: false,
                wikiUrl: 'https://warframe.fandom.com/wiki/Axi_A3',
            },
        },
        {
            id: 5,
            dropChance: DropChance.Rare,
            relic: {
                id: 2,
                tier: RelicTier.Neo,
                name: 'K5',
                isVaulted: false,
                wikiUrl: 'https://warframe.fandom.com/wiki/Axi_A3',
            },
        },
    ];
    private primeParts: PrimePart[] = [
        {
            id: 1,
            name: 'Blueprint',
        },
        {
            id: 2,
            name: 'Chassis'
        },
        {
            id: 3,
            name: 'Neuroptics',
        },
        {
            id: 4,
            name: 'Systems',
        },
    ];
    private primePartIngredients: PrimePartIngredient[] = [
        {
            id: 1,
            primePart: this.primeParts[0],
            count: 1,
            relicDrops: this.relicDrops,
            isChecked: { 0: false },
            isCollapsed: true,
        },
        {
            id: 2,
            primePart: this.primeParts[1],
            count: 1,
            relicDrops: this.relicDrops,
            isChecked: { 0: true },
            isCollapsed: true,
        },
        {
            id: 3,
            primePart: this.primeParts[2],
            count: 2,
            relicDrops: this.relicDrops,
            isChecked: { 0: false, 1: true },
            isCollapsed: true,
        },
        {
            id: 4,
            primePart: this.primeParts[3],
            count: 1,
            relicDrops: this.relicDrops,
            isChecked: { 0: false },
            isCollapsed: false,
        },
    ];
    private resources: Resource[] = [
        {
            id: 1,
            name: 'Argon',
        },
        {
            id: 2,
            name: 'Orokin Cell',
        },
        {
            id: 3,
            name: 'Cryotic'
        },
        {
            id: 4,
            name: 'Credits',
        },
    ]
    private resourceIngredients: ResourceIngredient[] = [
        {
            resource: this.resources[3],
            count: 25000,
        },
        {
            resource: this.resources[0],
            count: 2,
        },
        {
            resource: this.resources[1],
            count: 15,
        },
    ];
    private ingredientsGroup: IngredientsGroup = {
        id: 1,
        name: null,
        resourceIngredients: this.resourceIngredients,
    };
    private primeItem: PrimeItem = {
        id: 1,
        name: 'Banshee Prime',
        wikiUrl: 'https://warframe.fandom.com/wiki/Banshee/Prime',
        primePartIngredients: this.primePartIngredients,
        ingredientsGroups: [this.ingredientsGroup],
        credits: 25000,
        isChecked: false,
        isCollapsed: false,
    };
    private primeItems: PrimeItem[] | null = null;

    private _loadService!: LoadService;

    public mounted() {
        this._loadService = new LoadService(new PrimeItemService());
        this.loadData();
    }

    private async loadData(): Promise<void> {
        try {
            this.primeItems = await this._loadService.load();
        }
        catch (err) {
            console.error(err);
            throw err;
        }
    }
}
</script>

<style lang="scss" scoped>
.container {
    margin-top: 30px;
}
.red-border {
    border: 1px solid red;
}
</style>