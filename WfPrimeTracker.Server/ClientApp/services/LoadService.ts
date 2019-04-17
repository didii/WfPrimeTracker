import { PrimeItemService } from './PrimeItemService';
import { PrimeItem } from '@/models/PrimeItem';

interface IPartSaveData {
    [partName: string]: number;
}
interface IItemSaveData {
    isChecked: boolean;
    isCollapsed: boolean;
    parts: IPartSaveData;
}
interface ISaveData {
    [itemName: string]: IItemSaveData;
}

export class LoadService {
    private saveKey: string = 'saveData';
    public constructor(private primeItemService: PrimeItemService) { }

    public async load(): Promise<PrimeItem[]> {
        console.log('Start load');
        // Load items from server
        const items = await this.primeItemService.getAll();

        // Get saved data from local storage
        const saved = localStorage.getItem(this.saveKey);
        const saveData = this.default<ISaveData>(() => JSON.parse(saved!), {});

        for (const item of items) {
            const foundItem = saveData[item.name];
            item.isChecked = this.default(() => foundItem.isChecked, false);
            item.isCollapsed = this.default(() => foundItem.isCollapsed, false);
            for (const partIngredient of item.primePartIngredients) {
                const part = partIngredient.primePart;
                const countChecked = this.default(() => foundItem.parts[part.name], 0);
                partIngredient.isChecked = {};
                for (let i = 0; i < partIngredient.count; i++) {
                    partIngredient.isChecked[i] = countChecked > i;
                }
                partIngredient.isCollapsed = true;
            }
        }
        console.log('End load');
        return items;
    }

    public save(items: PrimeItem[]) {
        const result: ISaveData = {};
        for (const item of items) {
            if (item.isChecked || item.isCollapsed) {
                result[item.name] = {
                    isChecked: item.isChecked,
                    isCollapsed: item.isCollapsed,
                    parts: {}
                };
            }
            for (const partIngredient of item.primePartIngredients) {
                const part = partIngredient.primePart;
                if (!partIngredient.isChecked || Object.getOwnPropertyNames(partIngredient.isChecked).every(x => !partIngredient.isChecked[Number(x)]))
                    continue;
                if (!result[item.name]) {
                    result[item.name] = {
                        isChecked: item.isChecked,
                        isCollapsed: item.isCollapsed,
                        parts: {}
                    };
                }
                let count = 0;
                for (const propName of Object.getOwnPropertyNames(partIngredient.isChecked)) {
                    if (partIngredient.isChecked[<any>propName] === true) {
                        count++;
                    }
                }
                result[item.name].parts[part.name] = count;
            }
        }
        localStorage.setItem(this.saveKey, JSON.stringify(result));
    }

    private default<T>(expr: () => T, defaultValue: T): T {
        try {
            const result = expr();
            if (result != null) {
                return result;
            }
        }
        catch (error) { }
        return defaultValue;
    }
}