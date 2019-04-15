<template>
    <div data-component="ingredientview" class="text-center" :title="ingredient.name">
        <div>
            <img
                src="https://vignette.wikia.nocookie.net/warframe/images/b/b1/Credits64.png/revision/latest/scale-to-width-down/26?cb=20140515160015"
                height="25"
            />
        </div>
        <span class="ingredient-quantity">{{ ingredient.quantity }}</span>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Ingredient } from '../models/Ingredient';

@Component
export default class IngredientView extends Vue {
    @Prop({ required: true }) public ingredient!: Ingredient;
}
</script>

<style lang="scss" scoped>
.ingredient-quantity {
    font-size: 14px;
    color: dimgray;
}
</style>