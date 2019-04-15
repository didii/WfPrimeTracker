<template>
    <div data-component="primepartcontainerview" class="parts-container">
        Component PrimePartContainerView works!
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { PrimePart } from '../models/PrimePart';

@Component
export default class PrimePartContainerView extends Vue {
    @Prop({required: true}) private primeParts!: PrimePart[];
}
</script>

<style lang="scss" scoped>

</style>