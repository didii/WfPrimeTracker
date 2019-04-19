<template>
    <div data-component="primeitemsoptions">
        <button v-if="!isOpen" class="btn btn-secondary" @click="isOpen = true">
            <i class="fas fa-cog"></i>
        </button>
        <div v-else class="options-container">
            <h6>Options</h6>
            <div>
                <label> <input type="checkbox" v-model="globalOptions.hideCompleted" /> Hide completed </label>
            </div>
            <div>
                <label> <input type="checkbox" v-model="globalOptions.showIngredients" /> Show ingredients </label>
            </div>
            <button class="btn btn-secondary btn-close" @click="isOpen = false">
                <small>Close</small>
            </button>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { IGlobalOptions } from '@/services/LoadService';

@Component
export default class PrimeItemsOptions extends Vue {
    private isOpen: boolean = false;
    @Prop({ type: Object, required: true }) public globalOptions!: IGlobalOptions;
}
</script>

<style lang="scss" scoped>
.options-container {
    border: 1px solid gray;
    background-color: white;
    padding: 0.5rem;
    border-radius: 0.25rem;
    .btn-close {
        padding: 0.1rem 0.5rem;
    }
}
</style>