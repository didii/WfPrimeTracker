<template>
    <button data-component="tothetop" @click="toTheTop" class="btn badge badge-primary">
        Top <i class="fas fa-arrow-up"></i>
    </button>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class ToTheTop extends Vue {
    private toTheTop() {
        window.scrollTo(0, 0);
    }
}
</script>