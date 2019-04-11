import { ArgumentOutOfRangeError } from '@/Errors';

export enum RelicTier {
    Lith,
    Meso,
    Neo,
    Axi,
}

export namespace RelicTier {
    export function toString(relicTier: RelicTier) {
        switch (relicTier) {
            case RelicTier.Lith:
                return 'Lith';
            case RelicTier.Meso:
                return 'Meso';
            case RelicTier.Neo:
                return 'Neo';
            case RelicTier.Axi:
                return 'Axi';
            default:
                throw new ArgumentOutOfRangeError('relicTier');
        }
    }

    export function toClass(relicTier: RelicTier) {
        switch (relicTier) {
            case RelicTier.Lith:
                return 'lith';
            case RelicTier.Meso:
                return 'meso';
            case RelicTier.Neo:
                return 'neo';
            case RelicTier.Axi:
                return 'axi';
            default:
                throw new ArgumentOutOfRangeError('relicTier');
        }
    }
}