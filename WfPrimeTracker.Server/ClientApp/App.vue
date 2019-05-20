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
import DiModule from './stores/DiModule';

@Component({
    components: {
        NavMenu,
    }
})
export default class App extends Vue {
    private globalModule = getModule(GlobalModule, this.$store);
    private diModule = getModule(DiModule, this.$store);

    public created() {
        if (this.setAnonId(this.$route.query)) {
            this.$router.replace(this.$route.path + '?anonId=' + this.globalModule.userId);
        }
    }

    @Watch('$route.query') private onQueryParamsChange(query: Dictionary<string | (string | null)[]>) {
        this.setAnonId(query);
    }

    private setAnonId(queryParams: Dictionary<string | (string | null)[]>): boolean {
        // First use URL
        if (!this.setAnonIdFromQueryParams(queryParams)) {
            // If URL not available, check local storage
            if (!this.setAnonIdFromLocalStorage()) {
                console.log('No user ID found to set...');
            }
        }
        // Make sure if there is a value, to store it in the local storage
        if (this.globalModule.userId != null) {
            this.diModule.localLoadService.saveUserId(this.globalModule.userId);
            return true;
        }
        return false;
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
        console.log('User ID set to ' + anonId + ' from query param');
        this.globalModule.setUserId(anonId);
        return true;
    }

    private setAnonIdFromLocalStorage(): boolean {
        let anonId = this.diModule.localLoadService.loadUserId();
        if (anonId == null) {
            return false;
        }
        console.log('User ID set to ' + anonId + ' from local storage');
        this.globalModule.setUserId(anonId);
        return true;
    }
}
</script>