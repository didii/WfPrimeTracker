<template>
    <div data-component="primepartcontainerview" class="parts-container">
        <PrimePartView
            v-for="primePart in primeParts"
            :key="primePart.id"
            :primePart="primePart"
            :isParentChecked="isParentChecked"
            class="prime-part"
        />
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { PrimePart } from '../models/PrimePart';
import PrimePartView from './PrimePartView.vue';

@Component({
    components: {
        PrimePartView,
    }
})
export default class PrimePartContainerView extends Vue {
    @Prop({ required: true }) private primeParts!: PrimePart[];
    @Prop({ required: true }) public isParentChecked!: boolean;
}
</script>

<style lang="scss" scoped>
@import "../vars";

.parts-container > * {
    border-bottom: 1px solid $light-border-color;
}
.parts-container > *:last-child {
    border-bottom: none;
}
</style>