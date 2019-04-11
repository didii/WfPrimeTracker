import { RelicTier } from "./RelicTier.enum";
import { RelicRarity } from "./RelicRarity.enum";

export class Relic {
    public id!: number;
    public tier!: RelicTier;
    public name!: string;
    public rarity!: RelicRarity;
    public isVaulted!: boolean;
    public wikiUrl!: string;
}
