import { PrimeItem } from "@/models/PrimeItem";

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
    primePartIngredients: IPrimePartsSaveData;
}
export interface IPrimeItemsSaveData {
    [id: number]: IPrimeItemSaveData;
}
export interface ISaveData {
    primeItems: IPrimeItemsSaveData;
}

export class LoadService {
    private saveKey: string = "saveData";
    private defaultPrimeItemCheckedValue: boolean = false;
    private defaultPrimePartCheckedValue: boolean = false;
    private defaultPrimePartCollapsedValue: boolean = true;

    public constructor() {}

    public load(primeItems: PrimeItem[]): ISaveData {
        const saved = localStorage.getItem(this.saveKey);
        const saveData = this.default<ISaveData | null>(() => JSON.parse(saved!), null);
        const result = this.ensureCorrectSaveData(primeItems, saveData);
        return result;
    }

    public save(saveData: ISaveData) {
        // Clone save data so we don't modify it
        const toSave = JSON.parse(JSON.stringify(saveData)) as ISaveData;
        
        // Loop through every item
        for (const primeItemId in toSave.primeItems) {
            const primeItem = toSave.primeItems[primeItemId];
            // If checked value is the default -> delete the property
            if (primeItem.isChecked === this.defaultPrimeItemCheckedValue) {
                delete primeItem.isChecked;
            }
            // Loop through every part
            for (const primePartId in primeItem.primePartIngredients) {
                const primePart = primeItem.primePartIngredients[primePartId];
                // If collapsed value is the default -> delete the property
                if (primePart.isCollapsed === this.defaultPrimePartCollapsedValue) {
                    delete primePart.isCollapsed;
                }
                // Loop through every index
                for (const index in primePart.isChecked) {
                    // If default -> delete
                    if (primePart.isChecked[index] === this.defaultPrimePartCheckedValue) {
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
                delete toSave.primeItems[primeItemId];
            }
        }

        // Save the data
        localStorage.setItem(this.saveKey, JSON.stringify(toSave));
    }

    private ensureCorrectSaveData(primeItems: PrimeItem[], saveData?: ISaveData | null): ISaveData {
        const result: ISaveData = {
            primeItems: {},
        };
        for (const primeItem of primeItems) {
            let primePartSave: IPrimeItemSaveData = {
                isChecked: this.default(() => saveData!.primeItems[primeItem.id].isChecked, false),
                primePartIngredients: {},
            };
            for (const primePartIngredient of primeItem.primePartIngredients) {
                const isChecked: IPrimePartCheckedSaveData = {};
                for (let i = 0; i < primePartIngredient.count; i++) {
                    isChecked[i] = this.default(
                        () =>
                            saveData!.primeItems[primeItem.id].primePartIngredients[primePartIngredient.id].isChecked[
                                i
                            ],
                        false
                    );
                }
                primePartSave.primePartIngredients[primePartIngredient.id] = {
                    isChecked: isChecked,
                    isCollapsed: this.default(
                        () =>
                            saveData!.primeItems[primeItem.id].primePartIngredients[primePartIngredient.id].isCollapsed,
                        true
                    ),
                };
            }
            result.primeItems[primeItem.id] = primePartSave;
        }
        return result;
    }

    private default<T>(...exprs: ((() => T) | T)[]): T {
        let result;
        for (const expr of exprs) {
            try {
                if (typeof expr === "function") {
                    result = (<Function>expr)();
                } else {
                    result = expr;
                }
                if (result != null) {
                    return result;
                }
            } catch (error) {}
        }
        return result;
    }
}
