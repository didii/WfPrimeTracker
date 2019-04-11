import { ArgumentOutOfRangeError } from '@/Errors';

export enum RelicRarity {
    Common,
    Uncommon,
    Rare,
}

export namespace RelicRarity {
    export function toString(rarity: RelicRarity): string {
        switch (rarity) {
            case RelicRarity.Common:
                return 'Common';
            case RelicRarity.Uncommon:
                return 'Uncommon';
            case RelicRarity.Rare:
                return 'Rare';
            default:
                throw new ArgumentOutOfRangeError('rarity');
        }
    }

    export function toChance(rarity: RelicRarity): number[] {
        switch (rarity) {
            case RelicRarity.Common:
                return [25, 23, 20, 17];
            case RelicRarity.Uncommon:
                return [11, 13, 17, 20];
            case RelicRarity.Rare:
                return [2, 4, 6, 10];
            default:
                throw new ArgumentOutOfRangeError('rarity');
        }
    }
}