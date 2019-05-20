import { Module, VuexModule } from 'vuex-module-decorators';
import { SaveDataOptimizerService } from '@/services/SaveDataOptimizerService';
import { PrimeItemService } from '@/services/PrimeItemService';
import { LocalLoadService } from '@/services/LocalLoadService';
import { ServerLoadService } from '@/services/ServerLoadService';

@Module({name: 'DiModule'})
export default class DiModule extends VuexModule {

    public get primeItemService(): PrimeItemService {
        return new PrimeItemService();
    }

    public get saveDataOptimizer(): SaveDataOptimizerService {
        return new SaveDataOptimizerService();
    }

    public get localLoadService(): LocalLoadService {
        return new LocalLoadService();
    }

    public get serverLoadService(): ServerLoadService {
        return new ServerLoadService();
    }
}