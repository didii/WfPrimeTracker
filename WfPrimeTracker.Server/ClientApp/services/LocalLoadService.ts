import { PrimeItem } from '@/models/PrimeItem';
import { ISaveData } from '@/models/SaveData';
import { SaveDataOptimizerService } from './SaveDataOptimizerService';

export class LocalLoadService {
    private saveKey: string = 'saveData';
    private userIdKey: string = 'anonId';

    public constructor(private optimizer: SaveDataOptimizerService) {}

    public load(primeItems: PrimeItem[]): ISaveData {
        const saved = localStorage.getItem(this.saveKey);
        const saveData = this.default<ISaveData | undefined>(() => JSON.parse(saved!), undefined);
        const result = this.optimizer.deOptimize(primeItems, saveData);
        return result;
    }

    public save(saveData: ISaveData) {
        const toSave = this.optimizer.optimize(saveData);
        localStorage.setItem(this.saveKey, JSON.stringify(toSave));
    }

    public loadUserId(): string | null {
        return localStorage.getItem(this.userIdKey);
    }

    public saveUserId(userId: string) {
        localStorage.setItem(this.userIdKey, userId);
    }


    private default<T>(...exprs: ((() => T) | T)[]): T {
        let result;
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
        return result;
    }
}
