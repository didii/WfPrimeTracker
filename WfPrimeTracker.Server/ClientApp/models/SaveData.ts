export interface IPrimePartCheckedSaveData {
    [index: number]: boolean;
}
export interface IPrimePartSaveData {
    isChecked: IPrimePartCheckedSaveData;
    isCollapsed: boolean;
}
export interface IPrimePartsSaveData {
    [id: number]: IPrimePartSaveData;
}
export interface IPrimeItemSaveData {
    isChecked: boolean;
    showIngredients: boolean;
    primePartIngredients: IPrimePartsSaveData;
}
export interface IPrimeItemsSaveData {
    [id: number]: IPrimeItemSaveData;
}
export interface IGlobalOptions {
    hideCompleted: boolean;
    showIngredients: boolean;
}
export interface ISaveData {
    globalOptions: IGlobalOptions;
    primeItems: IPrimeItemsSaveData;
}
