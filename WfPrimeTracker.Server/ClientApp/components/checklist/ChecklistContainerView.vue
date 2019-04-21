<template>
    <div
        data-component="checklistcontainerview"
        :class="{ 'check-container': true, checked: isChecked }"
    >
        <div class="item-container">
            <div class="row">
                <RelicCollapseButton
                    :isCollapsed="isAllCollapsed"
                    @click.native="isAllCollapsed = !isAllCollapsed"
                    class="col-auto px-2 py-0"
                />
                <div class="col">
                    <label class="row">
                        <div class="font-weight-bold col">
                            <input
                                type="checkbox"
                                v-model="isChecked"
                                class="checkbox"
                            />
                            {{ primeItem.name }}
                            <a :href="wikiUrl" target="_blank">
                                <i class="fas fa-external-link-alt"></i>
                            </a>
                        </div>
                        <div v-if="isVaulted" title="Vaulted" class="col-auto vaulted text-muted">
                            V
                        </div>
                    </label>
                </div>
            </div>
        </div>
        <hr class="separator" />
        <PrimePartContainerView
            :primePartIngredients="primeItem.primePartIngredients"
            :saveData="saveData.primePartIngredients"
            :isParentChecked="isChecked"
        />
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import GlobalModule from '@/stores/GlobalModule';
import { IPrimeItemSaveData } from '@/services/LoadService';
import { PrimeItem } from '@/models/PrimeItem';
import RelicCollapseButton from '@/components/utils/RelicCollapseButton.vue';
import PrimePartContainerView from './PrimePartContainerView.vue';

@Component({
    components: {
        RelicCollapseButton,
        PrimePartContainerView
    }
})
export default class ChecklistContainerView extends Vue {
    private globalModule = getModule(GlobalModule, this.$store);
    @Prop({ type: Object, required: true }) private primeItem!: PrimeItem;
    @Prop({ type: Object, required: true }) private saveData!: IPrimeItemSaveData;

    private get isAllCollapsed(): boolean {
        for (const key in this.saveData.primePartIngredients) {
            const partSaveData = this.saveData.primePartIngredients[key];
            if (!partSaveData.isCollapsed) return false;
        }
        return true;
    }
    private set isAllCollapsed(value: boolean) {
        for (const key in this.saveData.primePartIngredients) {
            this.saveData.primePartIngredients[key].isCollapsed = value;
        }
    }

    private get isChecked(): boolean {
        return this.saveData.isChecked;
    }
    private set isChecked(value: boolean) {
        this.saveData.isChecked = value;
    }

    private get wikiUrl(): string {
        return this.globalModule.wikiBaseUrl + this.primeItem.wikiUrl;
    }

    private get isVaulted(): boolean {
        return this.primeItem.primePartIngredients.every(i => i.relicDrops.every(d => d.relic.isVaulted));
    }
}
</script>

<style lang="scss" scoped>
@import "../../vars";

.separator {
    margin: 0;
    border-top-color: gray;
}
.item-container {
    label {
        margin: 0;
        > * {
            padding: 0.5rem;
        }
        .vaulted {
            font-size: 85%;
            background-color: aliceblue;
            border-top-right-radius: .35rem;
        }
    }
    .fas.fa-external-link-alt {
        margin-left: 4px;
        font-size: 14px;
    }
}
.check-container {
    @extend %gradient-base;
    &.checked {
        @extend %gradient-checked;
    }
}
</style>