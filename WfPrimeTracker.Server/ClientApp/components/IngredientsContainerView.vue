<template>
    <div
        data-component="ingredientscontainerview"
        class="ingredients-container"
    >
        <div
            v-for="ingredientsGroup in ingredientsGroups"
            :key="ingredientsGroup.id"
            class="ingredients row"
        >
            <div v-if="ingredientsGroup.name" class="col-12 border-bottom text-center text-muted">{{ingredientsGroup.name}}</div>
            <IngredientView
                v-for="resourceIngredient in ingredientsGroup.resourceIngredients"
                :key="resourceIngredient.resource.id"
                :resourceIngredient="resourceIngredient"
                class="ingredient col"
            />
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { ResourceIngredient } from '@/models/ResourceIngredient';
import { IngredientsGroup } from '@/models/IngredientsGroup';
import IngredientView from './IngredientView.vue';

@Component({
    components: {
        IngredientView,
    }
})
export default class IngredientsContainerView extends Vue {
    @Prop({ type: Array, required: true }) public ingredientsGroups!: IngredientsGroup[];
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