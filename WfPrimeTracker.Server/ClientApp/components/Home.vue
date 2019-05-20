<template>
    <div data-component="home" class="container-fluid">
        <div v-if="primeItems == null" class="loading-container">
            <i class="fas fa-spinner loading-icon"></i>
            Loading...
        </div>
        <PrimeItemsContainerView
            v-else
            :primeItems="primeItems"
            :saveData="saveData"
            @saveAnonymous="saveAnonymous"
        />
    </div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import * as uuid from 'uuid';
import Constants from '@/consts';
import DiModule from '@/stores/DiModule';
import GlobalModule from '@/stores/GlobalModule';
import { ISaveData } from '@/models/SaveData';
import { PrimeItem } from '@/models/PrimeItem';
import PrimeItemsContainerView from './PrimeItemsContainerView.vue';

@Component({
    components: {
        PrimeItemsContainerView,
    },
})
export default class Home extends Vue {
    private globalModule = getModule(GlobalModule, this.$store);
    private diModule = getModule(DiModule, this.$store);

    private primeItems: PrimeItem[] | null = null;
    private saveData: ISaveData | null = null;
    private previousShowIngredients: boolean | null = null;

    public created() {
        this.loadData().then(({ primeItems, saveData }) => {
            this.primeItems = primeItems;
            this.saveData = this.diModule.saveDataOptimizer.deOptimize(primeItems, saveData);
        }, err => {
            throw err;
        });
    }

    private async loadData(): Promise<{ primeItems: PrimeItem[], saveData: ISaveData }> {
        // Load all data
        console.log('loading local data');
        let saveDataFromLocal = this.diModule.localLoadService.load();
        console.log(saveDataFromLocal);
        console.log('loading prime items')
        let primeItemsPromise = this.diModule.primeItemService.getAll();

        // Shortcut if no anonymous ID is available
        if (this.globalModule.userId == null) {
            console.log('no user id available');
            return {
                primeItems: await primeItemsPromise,
                saveData: saveDataFromLocal == null ? Constants.defaultSaveData : saveDataFromLocal,
            };
        }
        let serverSaveDataPromise = this.diModule.serverLoadService.loadAnonymous(this.globalModule.userId);

        // Wait for server data and pick which data set to use
        let tempData: ISaveData | null = null;
        let serverSaveData = await serverSaveDataPromise;

        // If both save data is unavailable
        if (saveDataFromLocal == null && serverSaveData == null) {
            // Use default
            console.log('Save data taken from default due to no data');
            return {
                primeItems: await primeItemsPromise,
                saveData: Constants.defaultSaveData,
            };
        }

        // If local data is only one available or local is more recent
        if (saveDataFromLocal != null && (serverSaveData == null || saveDataFromLocal.saveDate >= serverSaveData.modifiedOn)) {
            // Use local result
            return {
                primeItems: await primeItemsPromise,
                saveData: saveDataFromLocal,
            };
        }

        // If local is unavailble, convert server data and return
        if (saveDataFromLocal == null) {
            let saveDataFromServer = this.diModule.saveDataOptimizer.fromServer(serverSaveData!);
            return {
                primeItems: await primeItemsPromise,
                saveData: saveDataFromServer,
            }
        }

        // Both are available so server must be more recent, convert and return
        let saveDataFromServer = this.diModule.saveDataOptimizer.fromServer(serverSaveData!, saveDataFromLocal);
        return {
            primeItems: await primeItemsPromise,
            saveData: saveDataFromServer,
        };
    }

    private saveAnonymous() {
        console.log({msg: 'Saving...', saveData: this.saveData});
        let optimized = this.diModule.saveDataOptimizer.optimize(this.saveData!);
        let toSave = this.diModule.saveDataOptimizer.toServer(optimized);
        if (this.globalModule.userId == null) {
            let newId = uuid.v4();
            this.globalModule.setUserId(newId);
        }
        this.diModule.serverLoadService.saveAnonymous(this.globalModule.userId!, toSave);
    }

    @Watch('saveData.globalOptions.showIngredients')
    private onShowIngredientsChanged(show: boolean) {
        if (this.previousShowIngredients == null || this.previousShowIngredients == show) {
            this.previousShowIngredients = show;
            return;
        }

        for (const primeItemId in this.saveData!.primeItems) {
            this.saveData!.primeItems[primeItemId].showIngredients = show;
        }
        this.previousShowIngredients = show;
    }

    @Watch('saveData', { deep: true })
    private onSaveDataChanged(saveData: ISaveData) {
        this.diModule.localLoadService.save(saveData);
    }
}
</script>

<style lang="scss" scoped>
@keyframes spin {
    from {
        transform: rotate(0deg);
    }
    to {
        transform: rotate(360deg);
    }
}

.loading-container {
    text-align: center;
    padding: 1rem;
    .loading-icon {
        animation: spin 2s infinite linear;
    }
}
</style>