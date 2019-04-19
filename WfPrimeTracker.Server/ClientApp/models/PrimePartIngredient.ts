import { PrimePart } from './PrimePart'
import { RelicDrop } from './RelicDrop';

export interface PrimePartIngredient {
    id: number;
    primePart: PrimePart;
    relicDrops: RelicDrop[];
    count: number;
}
