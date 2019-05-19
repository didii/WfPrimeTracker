import { ISaveData, IPrimePartCheckedSaveData, IPrimeItemSaveData } from "@/models/SaveData";
import { PrimeItem } from '@/models/PrimeItem';

export class SaveDataOptimizerService {
    /**
     * Default value for a prime item being checked
     */
    private static defaultPrimeItemCheckedValue: boolean = false;

    /**
     * The default value for showing ingredients, is the same as showIngredients of the global options
     * @param saveData The current save data
     */
    private static defaultPrimeItemShowIngredientsValue(saveData?: ISaveData | null): boolean {
        return SaveDataOptimizerService.default(() => saveData!.globalOptions.showIngredients, false);
    };

    /**
     * The default value for a prime part being checked
     */
    private static defaultPrimePartCheckedValue: boolean = false;

    /**
     * The default value for a prime part being collapsed
     */
    private static defaultPrimePartCollapsedValue: boolean = true;

    /**
     * 'Optimizes' the data, i.e. removes all default values to make saveData as small as possible.
     * @param saveData The data to optimize
     * @remarks It leaves the given parameter unchanged
     */
    public optimize(saveData: ISaveData): ISaveData {
        // Clone save data so we don't modify it
        const result = JSON.parse(JSON.stringify(saveData)) as ISaveData;

        // Loop through every item
        for (const primeItemId in result.primeItems) {
            const primeItem = result.primeItems[primeItemId];
            // If checked value is the default -> delete the property
            if (primeItem.isChecked === SaveDataOptimizerService.defaultPrimeItemCheckedValue) {
                delete primeItem.isChecked;
            }
            // If show ingredients value is the default -> delete
            if (primeItem.showIngredients === SaveDataOptimizerService.defaultPrimeItemShowIngredientsValue(result)) {
                delete primeItem.showIngredients;
            }
            // Loop through every part
            for (const primePartId in primeItem.primePartIngredients) {
                const primePart = primeItem.primePartIngredients[primePartId];
                // If collapsed value is the default -> delete the property
                if (primePart.isCollapsed === SaveDataOptimizerService.defaultPrimePartCollapsedValue) {
                    delete primePart.isCollapsed;
                }
                // Loop through every index
                for (const index in primePart.isChecked) {
                    // If default -> delete
                    if (primePart.isChecked[index] === SaveDataOptimizerService.defaultPrimePartCheckedValue) {
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
                hideCompleted: SaveDataOptimizerService.default(() => saveData!.globalOptions.hideCompleted, false),
                showIngredients: SaveDataOptimizerService.default(() => saveData!.globalOptions.showIngredients, false),
            },
            primeItems: {},
        };
        for (const primeItem of primeItems) {
            let primePartSave: IPrimeItemSaveData = {
                isChecked: SaveDataOptimizerService.default(() => saveData!.primeItems[primeItem.id].isChecked, SaveDataOptimizerService.defaultPrimeItemCheckedValue),
                showIngredients: SaveDataOptimizerService.default(() => saveData!.primeItems[primeItem.id].showIngredients, SaveDataOptimizerService.defaultPrimeItemShowIngredientsValue(saveData!)),
                primePartIngredients: {},
            };
            for (const primePartIngredient of primeItem.primePartIngredients) {
                const isChecked: IPrimePartCheckedSaveData = {};
                for (let i = 0; i < primePartIngredient.count; i++) {
                    isChecked[i] = SaveDataOptimizerService.default(
                        () => saveData!.primeItems[primeItem.id].primePartIngredients[primePartIngredient.id].isChecked[i],
                        SaveDataOptimizerService.defaultPrimePartCheckedValue,
                    );
                }
                primePartSave.primePartIngredients[primePartIngredient.id] = {
                    isChecked: isChecked,
                    isCollapsed: SaveDataOptimizerService.default(
                        () => saveData!.primeItems[primeItem.id].primePartIngredients[primePartIngredient.id].isCollapsed,
                        SaveDataOptimizerService.defaultPrimePartCollapsedValue,
                    ),
                };
            }
            result.primeItems[primeItem.id] = primePartSave;
        }
        return result;
    }

    /**
     * The expressions/values are checked in order and the first non-null/undefined one is returned.
     * When a function is given, it is first evaluated before the check. If it throws any error, it is skipped and we'll continue to the next value.
     * When all values are null or undefined, an error is thrown.
     * @param exprs A list of expressions or values
     */
    private static default<T>(...exprs: ((() => T) | T)[]): T {
        let result: T;
        for (const expr of exprs) {
            try {
                if (typeof expr === 'function') {
                    result = (<Function>expr)();
                } else {
                    result = expr;
                }
                if (result != null) {
                    return result;
                }
            } catch (error) {}
        }
        throw Error('All given values were null or undefined');
    }

}
