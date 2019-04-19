<template>
    <div data-component="primepartcontainerview" class="parts-container">
        <PrimePartView
            v-for="primePartIngredient in primePartIngredients"
            :key="primePartIngredient.id"
            :primePartIngredient="primePartIngredient"
            :isParentChecked="isParentChecked"
            class="prime-part"
        />
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { PrimePart } from '@/models/PrimePart';
import { PrimePartIngredient } from '@/models/PrimePartIngredient';
import PrimePartView from './PrimePartView.vue';

@Component({
    components: {
        PrimePartView,
    }
})
export default class PrimePartContainerView extends Vue {
    @Prop({ type: Array, required: true }) private primePartIngredients!: PrimePartIngredient[];
    @Prop({ type: Boolean, required: true }) public isParentChecked!: boolean;
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