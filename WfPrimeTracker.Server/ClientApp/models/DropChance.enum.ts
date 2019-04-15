import { ArgumentOutOfRangeError } from '@/Errors';

export enum DropChance {
    Common,
    Uncommon,
    Rare,
}

export namespace DropChance {
    export function toString(rarity: DropChance): string {
        switch (rarity) {
            case DropChance.Common:
                return 'Common';
            case DropChance.Uncommon:
                return 'Uncommon';
            case DropChance.Rare:
                return 'Rare';
            default:
                throw new ArgumentOutOfRangeError('rarity');
        }
    }

    export function toChance(rarity: DropChance): number[] {
        switch (rarity) {
            case DropChance.Common:
                return [25, 23, 20, 17];
            case DropChance.Uncommon:
                return [11, 13, 17, 20];
            case DropChance.Rare:
                return [2, 4, 6, 10];
            default:
                throw new ArgumentOutOfRangeError('rarity');
        }
    }
}