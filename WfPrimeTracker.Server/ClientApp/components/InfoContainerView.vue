<template>
    <div data-component="infocontainerview" class="info-container">
        <div class="image-container text-center">
            <img
                src="https://vignette.wikia.nocookie.net/warframe/images/8/8e/BansheePrimeIcon.png/revision/latest?cb=20170308023206"
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
        <IngredientsContainerView v-if="showIngredients" :ingredients="primeItem.ingredients" />
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import IngredientsContainerView from './IngredientsContainerView.vue';
import { PrimeItem } from '../models/PrimeItem';

@Component({
    components: {
        IngredientsContainerView,
    }
})
export default class InfoContainerView extends Vue {
    @Prop({ required: true }) public primeItem!: PrimeItem;

    private showIngredients: boolean = false;

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
            max-width: 300px;
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