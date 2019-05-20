<template>
    <div data-component="primeitemsoptions">
        <button v-if="!isOpen" class="btn btn-secondary" @click="isOpen = true" data-aos="fade">
            <i class="fas fa-cog"></i>
        </button>
        <div v-else class="options-container" data-aos="fade">
            <h6 class="font-weight-bold">Options</h6>
            <div>
                <label> <input type="checkbox" v-model="globalOptions.hideCompleted" /> Hide completed </label>
            </div>
            <div>
                <label> <input type="checkbox" v-model="globalOptions.showIngredients" /> Show ingredients </label>
            </div>
            <button class="btn btn-primary" @click="saveAnonymous">
                Save anonymous
            </button>
            <br/>
            <button class="btn btn-secondary btn-close" @click="isOpen = false">
                <small>Close</small>
            </button>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { IGlobalOptions } from '@/models/SaveData';

@Component
export default class PrimeItemsOptions extends Vue {
    private isOpen: boolean = false;
    @Prop({ type: Object, required: true }) public globalOptions!: IGlobalOptions;

    private saveAnonymous() {
        this.$emit('saveAnonymous');
    }
}
</script>

<style lang="scss" scoped>
.options-container {
    border: 1px solid gray;
    background-color: white;
    padding: 0.5rem;
    border-radius: 0.25rem;
    .btn {
        margin-bottom: 2px;
        padding: 0.1rem 0.5rem;
    }
}
</style>