import { Module, VuexModule, Mutation } from 'vuex-module-decorators';
import { PrimeItemService } from '@/services/PrimeItemService';
import { LoadService } from '@/services/LoadService';

@Module({ name: 'GlobalModule' })
export default class GlobalModule extends VuexModule {
    private _searchQuery: string = '';
    private _highlightId: number = 0;

    public get primeItemService(): PrimeItemService {
        return new PrimeItemService();
    }

    public get loadService(): LoadService {
        return new LoadService();
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

    @Mutation public search(query: string) {
        this._searchQuery = query;
    }

    @Mutation public highlight(primeItemId: number) {
        this._highlightId = primeItemId;
    }
}
