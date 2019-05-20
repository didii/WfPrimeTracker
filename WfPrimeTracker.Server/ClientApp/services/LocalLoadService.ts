import Utils from '@/utils';
import { ISaveData } from '@/models/SaveData';
import Constants from '@/consts';

export class LocalLoadService {
    private saveKey: string = 'saveData';
    private userIdKey: string = 'anonId';

    public constructor() {}

    public load(): ISaveData {
        const saved = localStorage.getItem(this.saveKey);
        const result = Utils.default<ISaveData>(() => JSON.parse(saved!), Constants.defaultSaveData);
        return result;
    }

    public save(saveData: ISaveData) {
        let toSave = JSON.parse(JSON.stringify(saveData));
        toSave.saveDate = new Date();
        localStorage.setItem(this.saveKey, JSON.stringify(toSave));
    }

    public loadUserId(): string | null {
        return localStorage.getItem(this.userIdKey);
    }

    public saveUserId(userId: string) {
        localStorage.setItem(this.userIdKey, userId);
    }
}
