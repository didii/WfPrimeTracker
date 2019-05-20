import { Module, VuexModule, Mutation } from 'vuex-module-decorators';

@Module({ name: 'GlobalModule' })
export default class GlobalModule extends VuexModule {
    private _searchQuery: string = '';
    private _highlightId: number = 0;
    private _userId: string | null = null;
    private _isUserDataLoaded: boolean = false;

    public get searchQuery(): string {
        return this._searchQuery;
    }

    public get highlightId(): number {
        return this._highlightId;
    }

    public get userId(): string | null {
        return this._userId;
    }

    public get isUserDataLoaded(): boolean {
        return this._isUserDataLoaded;
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

    @Mutation public setIsUserDataLoaded() {
        this._isUserDataLoaded = true;
    }
}
