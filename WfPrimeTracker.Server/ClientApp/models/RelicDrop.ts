import { DropChance } from "./DropChance.enum";
import { Relic } from "./Relic";

export class RelicDrop {
    public id!: number;
    public dropChance!: DropChance;
    public relic!: Relic;

    public constructor(init?: Partial<RelicDrop>) {
        Object.assign(this, init);
    }
}