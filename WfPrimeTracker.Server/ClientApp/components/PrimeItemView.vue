<template>
    <div data-component="primeitemview" class="prime-item-view">
        <div class="row">
            <!-- <div class="collapse-container">
                <i class="fas fa-angle-down"></i>
            </div> -->
            <InfoContainerView
                :primeItem="primeItem"
                class="info-container col-md-2 col-12"
            />
            <ChecklistContainerView :primeItem="primeItem" class="col" />
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { PrimeItem } from '@/models/PrimeItem';
import InfoContainerView from './InfoContainerView.vue';
import ChecklistContainerView from './ChecklistContainerView.vue';

@Component({
    components: {
        InfoContainerView,
        ChecklistContainerView,
    }
})
export default class PrimeItemView extends Vue {
    @Prop({ type: Object, required: true }) public primeItem!: PrimeItem;
}
</script>

<style lang="scss" scoped>
.prime-item-view {
    border: 2px solid gray;
    border-radius: 0.5rem;
    .collapse-container {
        position: absolute;
        margin: 3px;
        padding: 2px 8px;
        border: 2px solid #c3c3c3;
        border-radius: 0.5rem;
        background-color: white;
        z-index: 5;
    }
    .info-container {
        border-bottom: 2px solid gray;
    }
    @media (min-width: 768px) {
        .info-container {
            border-right: 1px solid gray;
            border-bottom: none;
        }
    }
}
</style>