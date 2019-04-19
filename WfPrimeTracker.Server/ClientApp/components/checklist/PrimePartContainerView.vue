<template>
    <div data-component="primepartcontainerview" class="parts-container">
        <PrimePartView
            v-for="primePartIngredient in primePartIngredients"
            :key="primePartIngredient.id"
            :primePartIngredient="primePartIngredient"
            :saveData="primePartIngredientSaveData(primePartIngredient)"
            :isParentChecked="isParentChecked"
            class="prime-part"
        />
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { IPrimePartSaveData, IPrimePartsSaveData } from '@/services/LoadService';
import { PrimePart } from '@/models/PrimePart';
import { PrimePartIngredient } from '@/models/PrimePartIngredient';
import PrimePartView from './PrimePartView.vue';

@Component({
    components: {
        PrimePartView,
    }
})
export default class PrimePartContainerView extends Vue {
    @Prop({ type: Array, required: true }) public primePartIngredients!: PrimePartIngredient[];
    @Prop({ type: Object, required: true }) public saveData!: IPrimePartsSaveData; 
    @Prop({ type: Boolean, required: true }) public isParentChecked!: boolean;

    private get primePartIngredientSaveData(): (primePartIngredient: PrimePartIngredient) => IPrimePartSaveData {
        return ingredient => this.saveData[ingredient.id];
    }
}
</script>

<style lang="scss" scoped>
@import "../../vars";

.parts-container > * {
    border-bottom: 1px solid $light-border-color;
}
.parts-container > *:last-child {
    border-bottom: none;
}
</style>