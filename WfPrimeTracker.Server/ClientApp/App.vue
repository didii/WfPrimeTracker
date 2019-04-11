<template>
    <div id="app-root">
        <NavMenu class="nav-menu" />
        <div class="page-container">
            <router-view></router-view>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import GlobalModule from '@/stores/GlobalModule';
import { PrimeItem } from './models/PrimeItem';
import NavMenu from '@/components/NavMenu.vue';

@Component({
    components: {
        NavMenu,
    }
})
export default class App extends Vue {
    private module = getModule(GlobalModule, this.$store);
    private items: PrimeItem[] | null = null;
    
    public mounted() {
        this.loadData();
    }

    private async loadData() {
        this.items = await this.module.loadService.load();
    }

    @Watch('items', {deep: true})
    private saveData() {
        this.module.loadService.save(this.items!);
    }
}
</script>