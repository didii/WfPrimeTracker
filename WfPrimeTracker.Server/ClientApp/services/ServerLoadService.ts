import { IServerSaveData } from '@/models/ServerSaveData';

export class ServerLoadService {
    public constructor() {}

    public async loadAnonymous(anonId?: string | null): Promise<IServerSaveData | null> {
        var requestInit = this.getDefaultRequestInit(anonId);
        var response = await fetch('/api/users/', requestInit);
        if (response.ok) {
            if (response.status == 204) {
                return null;
            }
            return await response.json();
        }
        throw response;
    }

    public async saveAnonymous(anonId: string, saveData: IServerSaveData): Promise<void> {
        let requestInit = this.getDefaultRequestInit(anonId);
        requestInit.method = 'POST';
        requestInit.body = JSON.stringify(saveData);
        let response = await fetch('/api/users/', requestInit);
        if (!response.ok) {
            throw response;
        }
    }

    private getDefaultRequestInit(anonId?: string | null): RequestInit {
        let result: RequestInit = {};
        if (anonId != null) {
            result.headers = {
                'AnonymousId': anonId,
                'Content-Type': 'application/json',
            };
        }
        return result;
    }
}