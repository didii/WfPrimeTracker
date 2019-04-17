<template>
    <div data-component="reliccontainerview" class="relics-container">
        <RelicView
            v-for="relicDrop of relicDrops"
            :key="relicDrop.relic.id"
            :relicDrop="relicDrop"
        />
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { RelicDrop } from '../models/RelicDrop';
import RelicView from '@/components/RelicView.vue';

@Component({
    components: {
        RelicView
    }
})
export default class RelicContainerView extends Vue {
    @Prop({ type: Array, required: true }) relicDrops!: RelicDrop[];
}
</script>

<style lang="scss" scoped>
@import "../vars";

@media (min-width: 768px) {
    .relics-container {
        border-left: 1px solid $light-border-color;
    }
}
.relics-container {
    @extend %fade-in;
}
</style>