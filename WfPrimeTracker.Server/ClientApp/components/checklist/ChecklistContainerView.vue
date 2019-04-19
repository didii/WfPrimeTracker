<template>
    <div data-component="checklistcontainerview" :class="{'check-container': true, checked: isChecked}">
        <div class="item-container">
            <div class="row">
                <RelicCollapseButton
                    :isCollapsed="isAllCollapsed"
                    @click.native="isAllCollapsed = !isAllCollapsed"
                    class="col-auto px-2 py-0"
                />
                <div class="col">
                    <label class="font-weight-bold m-0 p-2 w-100">
                        <input
                            type="checkbox"
                            v-model="isChecked"
                            class="checkbox"
                        />
                        {{ primeItem.name }}
                        <a :href="wikiUrl" target="_blank">
                            <i class="fas fa-external-link-alt"></i>
                        </a>
                    </label>
                </div>
            </div>
        </div>
        <hr class="separator" />
        <PrimePartContainerView
            :primePartIngredients="primeItem.primePartIngredients"
            :isParentChecked="isChecked"
        />
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import GlobalModule from '@/stores/GlobalModule';
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

    private get isAllCollapsed(): boolean {
        return this.primeItem.primePartIngredients.every(p => p.isCollapsed);
    }
    private set isAllCollapsed(value: boolean) {
        this.primeItem.primePartIngredients.forEach(p => {
            p.isCollapsed = value;
        });
    }

    private get isChecked(): boolean {
        return this.primeItem.isChecked;
    }
    private set isChecked(value: boolean) {
        this.primeItem.isChecked = value;
    }

    private get wikiUrl(): string {
        return this.globalModule.wikiBaseUrl + this.primeItem.wikiUrl;
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