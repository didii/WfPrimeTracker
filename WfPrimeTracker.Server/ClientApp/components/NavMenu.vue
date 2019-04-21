<template>
    <nav
        data-component="navmenu"
        class="navbar sticky-top navbar-expand-md navbar-dark bg-dark"
    >
        <router-link class="navbar-brand" to="/home">
            <img
                src="https://i.imgur.com/PQPrX2s.png"
                width="30"
                height="30"
                class="d-inline-block align-top"
                alt=""
            />
            Prime Tracker
        </router-link>
        <button
            class="navbar-toggler"
            type="button"
            data-toggle="collapse"
            data-target="#navbarSupportedContent"
            aria-controls="navbarSupportedContent"
            aria-expanded="false"
            aria-label="Toggle navigation"
        >
            <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="navbarSupportedContent">
            <ul class="navbar-nav mr-auto">
                <li class="nav-item">
                    <router-link class="nav-link" to="/home">Tracker</router-link>
                </li>
                <li class="nav-item">
                    <router-link class="nav-link" to="/about">About</router-link>
                </li>
            </ul>
            <div class="form-inline my-2 my-lg-0">
                <div class="input-group">
                    <div class="input-group-prepend">
                        <span class="input-group-text" id="basic-addon1">
                            <i class="fas fa-search"></i>
                        </span>
                    </div>
                    <input
                        type="text"
                        class="form-control"
                        placeholder="Search..."
                        @input="onSearchQueryChange"
                        aria-label="Search"
                    />
                </div>
            </div>
        </div>
    </nav>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import GlobalModule from '@/stores/GlobalModule';

@Component
export default class NavMenu extends Vue {
    private globalModule = getModule(GlobalModule, this.$store);

    private onSearchQueryChange(event: any) {
        this.globalModule.search(event.target.value);
    }
}
</script>

<style lang="scss" scoped>
.router-link-active {
    color: white !important;
}
</style>