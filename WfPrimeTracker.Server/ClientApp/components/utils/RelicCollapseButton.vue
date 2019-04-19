<template>
    <button :class="{ btn: true, 'caret-open': !isCollapsed }">
        <i class="fas fa-caret-right icon"></i>
    </button>
</template>

<script lang="ts">
import { Component, Vue, Prop, Emit } from 'vue-property-decorator';

@Component
export default class RelicCollapseButton extends Vue {
    @Prop({ type: Boolean, required: true }) public isCollapsed!: boolean;
}
</script>

<style lang="scss" scoped>
.fas {
    transition: transform 200ms ease;
}
.btn.caret-open {
    .icon {
        transform: rotate(45deg);
    }
}
</style>