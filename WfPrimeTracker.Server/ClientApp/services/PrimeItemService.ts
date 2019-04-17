import { PrimeItem } from '@/models/PrimeItem';

export class PrimeItemService {
    public async getAll(): Promise<PrimeItem[]> {
        var response = await fetch('/api/primeitems');
        if (response.ok) {
            return await response.json() as PrimeItem[];
        }
        throw response;
    }
}