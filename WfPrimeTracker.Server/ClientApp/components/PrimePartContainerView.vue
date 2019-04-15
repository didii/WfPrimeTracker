<template>
    <div data-component="primepartcontainerview" class="parts-container">
        <PrimePartView
            v-for="primePart in primeParts"
            :key="primePart.id"
            :primePart="primePart"
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
}
</script>

<style lang="scss" scoped>
.parts-container > * {
    border-bottom: 1px solid #dee2e6;
}
.parts-container > *:last-child {
    border-bottom: none;
}
</style>