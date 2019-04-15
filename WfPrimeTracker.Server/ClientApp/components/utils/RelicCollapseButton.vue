<template>
    <button class="btn">
        <i :class="`fas fa-caret-right ${isOpen ? 'caret-open' : ''}`"></i>
    </button>
</template>

<script lang="ts">
import { Component, Vue, Prop, Emit } from 'vue-property-decorator';

@Component
export default class RelicCollapseButton extends Vue {
    @Prop({ required: true }) public isOpen!: boolean;
}
</script>

<style lang="scss" scoped>
.fas {
    transition: transform 200ms ease;
}
.caret-open {
    transform: rotate(45deg);
}
</style>