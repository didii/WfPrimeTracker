import { RelicTier } from "./RelicTier.enum";

export class Relic {
    public id!: number;
    public tier!: RelicTier;
    public name!: string;
    public isVaulted!: boolean;
    public wikiUrl!: string;

    public constructor(init?: Partial<Relic>) {
        Object.assign(this, init);
    }
}
