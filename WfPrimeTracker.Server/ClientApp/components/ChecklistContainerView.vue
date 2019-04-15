<template>
    <div data-component="checklistcontainerview">
        <div class="item-container">
            <div class="row">
                <RelicCollapseButton
                    :isOpen="isAllOpen"
                    @click.native="isAllOpen = !isAllOpen"
                    class="col-auto px-2 py-0"
                />
                <div :class="`col check-container ${isChecked ? 'checked' : ''}`">
                    <label class="font-weight-bold m-0 p-2 w-100">
                        <input
                            type="checkbox"
                            v-model="isChecked"
                            class="checkbox"
                        />
                        {{ primeItem.name }}
                        <a
                            :href="primeItem.wikiUrl"
                            target="_blank"
                            title="Go to wiki"
                        >
                            <i class="fas fa-external-link-alt"></i>
                        </a>
                    </label>
                </div>
            </div>
        </div>
        <hr class="separator" />
        <PrimePartContainerView
            :primeParts="primeItem.primeParts"
            :isParentChecked="isChecked"
        />
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { PrimeItem } from '@/models/PrimeItem';
import PrimePartContainerView from './PrimePartContainerView.vue';
import RelicCollapseButton from './utils/RelicCollapseButton.vue';

@Component({
    components: {
        RelicCollapseButton,
        PrimePartContainerView
    }
})
export default class ChecklistContainerView extends Vue {
    @Prop({ required: true }) private primeItem!: PrimeItem;

    private get isAllOpen(): boolean {
        return this.primeItem.primeParts.some(p => p.showRelics);
    }
    private set isAllOpen(value: boolean) {
        this.primeItem.primeParts.forEach(p => {
            p.showRelics = value;
        });
    }

    private get isChecked(): boolean {
        return this.primeItem.isChecked;
    }
}
</script>

<style lang="scss" scoped>
@import "../vars";

.separator {
    margin: 0;
    border-top-color: gray;
}
.item-container {
    .check-container {
        @extend %gradient-base;
        &.checked {
            @extend %gradient-checked;
        }
    }
    .fas.fa-external-link-alt {
        margin-left: 4px;
        font-size: 14px;
    }
}
</style>