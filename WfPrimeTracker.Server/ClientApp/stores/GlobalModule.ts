import { Module, VuexModule, Mutation } from 'vuex-module-decorators';
import { PrimeItemService } from '@/services/PrimeItemService';
import { LocalLoadService } from '@/services/LocalLoadService';
import { SaveDataOptimizerService } from '@/services/SaveDataOptimizerService';

@Module({ name: 'GlobalModule' })
export default class GlobalModule extends VuexModule {
    private _searchQuery: string = '';
    private _highlightId: number = 0;
    private _userId: string | null = null;

    public get primeItemService(): PrimeItemService {
        return new PrimeItemService();
    }

    public get saveDataOptimizer(): SaveDataOptimizerService {
        return new SaveDataOptimizerService();
    }

    public get loadService(): LocalLoadService {
        return new LocalLoadService(this.saveDataOptimizer);
    }

    public get wikiBaseUrl(): string {
        return 'https://warframe.fandom.com';
    }

    public get searchQuery(): string {
        return this._searchQuery;
    }

    public get highlightId(): number {
        return this._highlightId;
    }

    public get userId(): string | null {
        return this._userId;
    }

    @Mutation public search(query: string) {
        this._searchQuery = query;
    }

    @Mutation public highlight(primeItemId: number) {
        this._highlightId = primeItemId;
    }

    @Mutation public setUserId(userId: string | null) {
        this._userId = userId;
    }
}
