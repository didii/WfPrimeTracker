import { Module, VuexModule } from 'vuex-module-decorators';
import { DataService } from '@/services/DataService';
import { LoadService } from '@/services/LoadService';

export interface IPartState {
    [partName: string]: boolean;
}
export interface IItemState {
    isChecked: boolean;
    partsState: IPartState;
}
export interface IState {
    [itemName: string]: IItemState;
}

@Module({ name: 'GlobalModule' })
export default class GlobalModule extends VuexModule {
    public get dataService(): DataService {
        return new DataService();
    }

    public get loadService(): LoadService {
        return new LoadService(this.dataService);
    }
}