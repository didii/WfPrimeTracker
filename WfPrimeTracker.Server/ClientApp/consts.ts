import { IGlobalOptions, ISaveData } from '@/models/SaveData';

interface IConstants {
    readonly wikiUrl: string;
    readonly defaultGlobalOptions: IGlobalOptions;
    readonly defaultPrimeItemCheckedValue: boolean;
    defaultPrimeItemShowIngredientsValue(globalOptions: IGlobalOptions): boolean;
    readonly defaultPrimePartCheckedValue: boolean;
    readonly defaultPrimePartCollapsedValue: boolean;
    readonly defaultSaveData: ISaveData;
}

let defaultGlobalOptions: IGlobalOptions = {
    hideCompleted: false,
    showIngredients: false,
}

let Constants: IConstants = {
    wikiUrl: 'https://warframe.fandom.com',
    defaultGlobalOptions: defaultGlobalOptions,
    defaultPrimeItemCheckedValue: false,
    defaultPrimeItemShowIngredientsValue(globalOptions: IGlobalOptions) {
        if (globalOptions != null && globalOptions.showIngredients != null) {
            return globalOptions.showIngredients;
        }
        return false;
    },
    defaultPrimePartCheckedValue: false,
    defaultPrimePartCollapsedValue: true,
    defaultSaveData: {
        globalOptions: defaultGlobalOptions,
        primeItems: {},
        saveDate: new Date(-8640000000000000),
    }
}
export default Constants;