import { ISaveData, IPrimePartCheckedSaveData, IPrimeItemSaveData, IGlobalOptions } from "@/models/SaveData";
import { PrimeItem } from '@/models/PrimeItem';
import { IServerSaveData } from '@/models/ServerSaveData';
import Constants from '@/consts';
import Utils from '@/utils';

export class SaveDataOptimizerService {
    /**
     * 'Optimizes' the data, i.e. removes all default values to make saveData as small as possible.
     * @param saveData The data to optimize
     * @remarks It leaves the given parameter unchanged
     */
    public optimize(saveData: ISaveData): ISaveData {
        // Clone save data so we don't modify it
        const result = JSON.parse(JSON.stringify(saveData)) as ISaveData;
        console.log({msg: 'Optimizing', saveData});

        // Loop through every item
        for (const primeItemId in result.primeItems) {
            const primeItem = result.primeItems[primeItemId];
            // If checked value is the default -> delete the property
            if (primeItem.isChecked === Constants.defaultPrimeItemCheckedValue) {
                delete primeItem.isChecked;
            }
            // If show ingredients value is the default -> delete
            if (primeItem.showIngredients === Constants.defaultPrimeItemShowIngredientsValue(result.globalOptions)) {
                delete primeItem.showIngredients;
            }
            // Loop through every part
            for (const primePartId in primeItem.primePartIngredients) {
                const primePart = primeItem.primePartIngredients[primePartId];
                // If collapsed value is the default -> delete the property
                if (primePart.isCollapsed === Constants.defaultPrimePartCollapsedValue) {
                    delete primePart.isCollapsed;
                }
                // Loop through every index
                for (const index in primePart.isChecked) {
                    // If default -> delete
                    if (primePart.isChecked[index] === Constants.defaultPrimePartCheckedValue) {
                        delete primePart.isChecked[index];
                    }
                }
                // Check if any index is left, delete if none
                if (Object.keys(primePart.isChecked).length === 0) {
                    delete primePart.isChecked;
                }
                // Check if any info is left, delete if none
                if (Object.keys(primePart).length === 0) {
                    delete primeItem.primePartIngredients[primePartId];
                }
            }
            // Check if part still has data, delete is none
            if (Object.keys(primeItem.primePartIngredients).length === 0) {
                delete primeItem.primePartIngredients;
            }
            // Check if item has any part left, delete item if none
            if (Object.keys(primeItem).length === 0) {
                delete result.primeItems[primeItemId];
            }
        }
        return result;
    }

    /**
     * 'De-optimizes' the save data by matching it with the prime items data. Every value not present is set to its default.
     * @param primeItems The prime items list to fill the data up with
     * @param saveData The save data to fill up
     */
    public deOptimize(primeItems: PrimeItem[], saveData?: ISaveData) {
        const result: ISaveData = {
            globalOptions: {
                hideCompleted: Utils.default(() => saveData!.globalOptions.hideCompleted, false),
                showIngredients: Utils.default(() => saveData!.globalOptions.showIngredients, false),
            },
            primeItems: {},
            saveDate: Utils.default(() => saveData!.saveDate, new Date()),
        };
        for (const primeItem of primeItems) {
            let primePartSave: IPrimeItemSaveData = {
                isChecked: Utils.default(() => saveData!.primeItems[primeItem.id].isChecked, Constants.defaultPrimeItemCheckedValue),
                showIngredients: Utils.default(() => saveData!.primeItems[primeItem.id].showIngredients, Constants.defaultPrimeItemShowIngredientsValue(saveData!.globalOptions)),
                primePartIngredients: {},
            };
            for (const primePartIngredient of primeItem.primePartIngredients) {
                const isChecked: IPrimePartCheckedSaveData = {};
                for (let i = 0; i < primePartIngredient.count; i++) {
                    isChecked[i] = Utils.default(
                        () => saveData!.primeItems[primeItem.id].primePartIngredients[primePartIngredient.id].isChecked[i],
                        Constants.defaultPrimePartCheckedValue,
                    );
                }
                primePartSave.primePartIngredients[primePartIngredient.id] = {
                    isChecked: isChecked,
                    isCollapsed: Utils.default(
                        () => saveData!.primeItems[primeItem.id].primePartIngredients[primePartIngredient.id].isCollapsed,
                        Constants.defaultPrimePartCollapsedValue,
                    ),
                };
            }
            result.primeItems[primeItem.id] = primePartSave;
        }
        return result;
    }

    /**
     * Converts saveData to its server equivalent, best to use the optimized version of the save data for this
     * @param saveData The data to convert
     */
    public toServer(saveData: ISaveData): IServerSaveData {
        console.log({msg: 'toServer', saveData});
        let result: IServerSaveData = {
            modifiedOn: saveData.saveDate,
            primeItems: {},
        };
        for (let primeItemId in saveData.primeItems) {
            let savedPrimeItem = saveData.primeItems[primeItemId];
            result.primeItems[primeItemId] = {
                isChecked: savedPrimeItem.isChecked,
                primePartIngredients: {},
            };

            for (let primePartId in savedPrimeItem.primePartIngredients) {
                let savedPrimePart = savedPrimeItem.primePartIngredients[primePartId];
                result.primeItems[primeItemId].primePartIngredients[primePartId] = {
                    checkedCount: Utils.default(() => Object.keys(savedPrimePart.isChecked).filter((k: any) => savedPrimePart.isChecked[k]).length, 0),
                };
            }
        }
        return result;
    }

    /**
     * Merges saveData and serverSaveData to a ISaveData type, best to de-optimize the save data after this
     * @param serverSaveData The data to convert
     * @param saveData The data to use as base 
     */
    public fromServer(serverSaveData: IServerSaveData, saveData?: ISaveData): ISaveData {
        let result: ISaveData = {
            globalOptions: Utils.default<IGlobalOptions>(() => JSON.parse(JSON.stringify(saveData!.globalOptions)), Constants.defaultGlobalOptions),
            primeItems: {},
            saveDate: Utils.default(() => saveData!.saveDate, new Date()),
        };
        for (let primeItemId in serverSaveData.primeItems) {
            let serverPrimeItem = serverSaveData.primeItems[primeItemId];
            result.primeItems[primeItemId] = {
                isChecked: serverPrimeItem.isChecked,
                showIngredients: Utils.default(() => saveData!.primeItems[primeItemId].showIngredients,
                Constants.defaultPrimeItemShowIngredientsValue(saveData!.globalOptions), false),
                primePartIngredients: {},
            }
            for (let primePartId in serverPrimeItem.primePartIngredients) {
                let serverPrimePart = serverPrimeItem.primePartIngredients[primePartId];
                result.primeItems[primeItemId].primePartIngredients[primePartId] = {
                    isCollapsed: Utils.default(() => saveData!.primeItems[primeItemId].primePartIngredients[primePartId].isCollapsed,
                    Constants.defaultPrimePartCollapsedValue),
                    isChecked: [...Array(serverPrimePart.checkedCount)].map(() => true),
                };
            }
        }
        return result;
    }
}
