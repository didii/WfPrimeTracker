import { RelicDrop } from "./RelicDrop";

export class PrimePart {
    public id!: number;
    public name!: string;
    public count!: number;
    public relicDrops!: RelicDrop[];
    public isChecked!: { [index: number]: boolean };
}
