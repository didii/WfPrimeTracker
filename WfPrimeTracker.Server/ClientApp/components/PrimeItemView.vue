<template>
    <div data-component="primeitemview" :class="{'prime-item-view': true, 'highlight': highlight}">
        <div class="row">
            <!-- <div class="collapse-container">
                <i class="fas fa-angle-down"></i>
            </div> -->
            <InfoContainerView
                :primeItem="primeItem"
                :saveData="saveData"
                class="info-container col-md-3 col-12"
            />
            <ChecklistContainerView :primeItem="primeItem" :saveData="saveData" class="col" />
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import GlobalModule from '@/stores/GlobalModule';
import { IPrimeItemSaveData } from '@/services/LoadService';
import { PrimeItem } from '@/models/PrimeItem';
import InfoContainerView from './info/InfoContainerView.vue';
import ChecklistContainerView from './checklist/ChecklistContainerView.vue';
import { setTimeout } from 'timers';

@Component({
    components: {
        InfoContainerView,
        ChecklistContainerView,
    }
})
export default class PrimeItemView extends Vue {
    private globalModule = getModule(GlobalModule);
    private highlight: boolean = false;

    @Prop({ type: Object, required: true }) public primeItem!: PrimeItem;
    @Prop({ type: Object, required: true }) public saveData!: IPrimeItemSaveData;


    @Watch('globalModule.highlightId')
    private onHighlightIdChanged(highlightId: number) {
        if (this.primeItem.id === highlightId) {
            this.highlight = true;
            setTimeout(() => this.highlight = false, 100);
        }
    }
}
</script>

<style lang="scss" scoped>
.prime-item-view {
    height: 100%;
    border: 2px solid gray;
    border-radius: 0.5rem;
    transition: background-color 1s linear;
    &.highlight {
        background-color: burlywood;
        transition: none;
    }
    > .row {
        height: 100%;
    }
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