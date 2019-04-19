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
                @click="showIngredients = !showIngredients"
            >
                {{ showIngredients ? "Hide" : "Show" }} ingredients
            </button>
        </div>
        <IngredientsContainerView v-if="showIngredients" :ingredientsGroups="primeItem.ingredientsGroups" />
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { IPrimeItemSaveData } from '@/services/LoadService';
import { PrimeItem } from '@/models/PrimeItem';
import IngredientsContainerView from './IngredientsContainerView.vue';

@Component({
    components: {
        IngredientsContainerView,
    }
})
export default class InfoContainerView extends Vue {
    @Prop({ type: Object, required: true }) public primeItem!: PrimeItem;
    @Prop({ type: Object, required: true }) public saveData!: IPrimeItemSaveData;

    private get imageUrl(): string {
        return `/api/primeitems/${this.primeItem.id}/image`;
    }

    private get showIngredients(): boolean {
        return this.saveData.showIngredients;
    }
    private set showIngredients(value: boolean) {
        this.saveData.showIngredients = value;
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
            bottom: 1px;
            right: 1px;
            background-color: rgba(255, 255, 255, 0.85);
            font-size: 12px;
            padding: 0;
        }
    }
}
</style>