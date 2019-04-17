import { RelicTier } from './RelicTier.enum';

export interface Relic {
    id: number;
    tier: RelicTier;
    name: string;
    isVaulted: boolean;
    wikiUrl: string;
}
