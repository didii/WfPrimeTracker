<template>
    <div
        data-component="ingredientscontainerview"
        class="ingredients-container"
    >
        <div class="ingredients row border-top">
            <IngredientView
                v-for="ingredient in ingredients"
                :key="ingredient.id"
                :ingredient="ingredient"
                class="ingredient col"
            />
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Ingredient } from '../models/Ingredient';
import IngredientView from './IngredientView.vue';

@Component({
    components: {
        IngredientView,
    }
})
export default class IngredientsContainerView extends Vue {
    @Prop({ required: true }) public ingredients!: Ingredient[];
}
</script>

<style lang="scss" scoped>
@import "../vars";

.ingredients-container {
    position: relative;
    .hide-button {
        position: absolute;
        background-color: white;
        top: -20px;
        right: 0;
        font-size: 12px;
        padding: 0;
    }
    .ingredients {
        border-bottom: 1px solid $light-border-color;
        .ingredient {
            border-right: 1px solid $light-border-color;
            &:last-child {
                border-right: none;
            }
        }
    }
}
</style>