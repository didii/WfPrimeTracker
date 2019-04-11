<template>
    <div data-component="relicview" class="relic neo vaulted">
        <span> Neo N7 <i class="fas fa-angle-up" title="Uncommon"></i> </span>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { RelicDrop } from '../models/RelicDrop';

@Component
export default class RelicView extends Vue {
    @Prop({ required: true }) public relicDrop!: RelicDrop;
}
</script>