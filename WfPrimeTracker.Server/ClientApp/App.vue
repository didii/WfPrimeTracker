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
import { PrimeItem } from '@/models/PrimeItem';
import NavMenu from '@/components/NavMenu.vue';
import { Dictionary } from 'vuex';

@Component({
    components: {
        NavMenu,
    }
})
export default class App extends Vue {
    private globalModule = getModule(GlobalModule, this.$store);

    public mounted() {
        this.setAnonId(this.$route.query);
        
    }

    @Watch('$route.query') private onQueryParamsChange(query: Dictionary<string | (string | null)[]>) {
        this.setAnonId(query);
    }

    private setAnonId(queryParams: Dictionary<string | (string | null)[]>) {
        // First use URL
        if (!this.setAnonIdFromQueryParams(queryParams)) {
            // If URL not available, check local storage
            this.setAnonIdFromLocalStorage();
        }
        // Make sure if there is a value, to store it in the local storage
        if (this.globalModule.userId != null) {
            localStorage.setItem('anonId', this.globalModule.userId);
        }
    }

    private setAnonIdFromQueryParams(queryParams: Dictionary<string | (string | null)[]>): boolean {
        let anonIdValues = queryParams["anonId"];
        if (anonIdValues == null) {
            return false;
        }
        let anonId: string | null;
        if (typeof anonIdValues === 'string') {
            anonId = anonIdValues;
        } else {
            anonId = anonIdValues[0];
        }
        if (anonId == null) {
            return false;
        }
        this.globalModule.setUserId(anonId);
        return true;
    }

    private setAnonIdFromLocalStorage(): boolean {
        var anonId = localStorage.getItem('anonId');
        if (anonId == null) {
            return false;
        }
        this.globalModule.setUserId(anonId);
        return true;
    }
}
</script>