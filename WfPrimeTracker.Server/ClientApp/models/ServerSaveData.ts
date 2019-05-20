export interface IServerPrimePartSaveData {
    checkedCount: number;
}
export interface IServerPrimePartsSaveData {
    [id: number]: IServerPrimePartSaveData;
}
export interface IServerPrimeItemSaveData {
    isChecked: boolean;
    primePartIngredients: IServerPrimePartsSaveData;
}
export interface IServerPrimeItemsSaveData {
    [id: number]: IServerPrimeItemSaveData;
}
export interface IServerSaveData {
    primeItems: IServerPrimeItemsSaveData;
    modifiedOn: Date;
}
