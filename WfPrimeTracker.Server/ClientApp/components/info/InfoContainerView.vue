<template>
    <div data-component="infocontainerview" class="info-container">
        <div class="image-container text-center border-bottom">
            <img
                v-lazy="imageUrl"
                alt=""
                class="image"
            />
            <button
                class="btn btn-link hide-button"
                @click="onToggleIngredients"
            >
                {{ showIngredients ? "Hide" : "Show" }} ingredients
            </button>
        </div>
        <IngredientsContainerView v-if="showIngredients" :ingredientsGroups="primeItem.ingredientsGroups" />
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { PrimeItem } from '@/models/PrimeItem';
import IngredientsContainerView from './IngredientsContainerView.vue';

@Component({
    components: {
        IngredientsContainerView,
    }
})
export default class InfoContainerView extends Vue {
    @Prop({ type: Object, required: true }) public primeItem!: PrimeItem;

    private showIngredients: boolean = false;

    private get imageUrl(): string {
        return `/api/primeitems/${this.primeItem.id}/image`;
    }

    private onToggleIngredients() {
        this.showIngredients = !this.showIngredients;
    }
}
</script>

<style lang="scss" scoped>
.info-container {
    .image-container {
        position: relative;
        img {
            max-width: 145px;
        }
        .hide-button {
            position: absolute;
            bottom: 0;
            right: 0;
            background-color: rgba(255, 255, 255, 0.85);
            font-size: 12px;
            padding: 0;
        }
    }
}
</style>