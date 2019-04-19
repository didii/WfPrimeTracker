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

@Component({
    components: {
        NavMenu,
    }
})
export default class App extends Vue {
}
</script>