<template>
    <div data-component="ingredientview" class="text-center" :title="resourceIngredient.resource.name">
        <div>
            <img
                src="https://vignette.wikia.nocookie.net/warframe/images/b/b1/Credits64.png/revision/latest/scale-to-width-down/26?cb=20140515160015"
                height="25"
            />
        </div>
        <span class="ingredient-quantity">{{ resourceIngredient.count }}</span>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { ResourceIngredient } from '@/models/ResourceIngredient';

@Component
export default class IngredientView extends Vue {
    @Prop({ type: Object, required: true }) public resourceIngredient!: ResourceIngredient;
}
</script>

<style lang="scss" scoped>
.ingredient-quantity {
    font-size: 14px;
    color: dimgray;
}
</style>