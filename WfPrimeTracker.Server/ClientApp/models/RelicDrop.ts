import { DropChance } from "./DropChance.enum";
import { Relic } from "./Relic";

export class RelicDrop {
    public id!: number;
    public dropChance!: DropChance;
    public relic!: Relic;
}