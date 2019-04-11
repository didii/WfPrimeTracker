import { PrimePart } from "./PrimePart";

export class PrimeItem {
    public id!: number;
    public name!: string;
    public wikiUrl!: string;
    public primeParts!: PrimePart[];
    public ingredients!: PrimeItem[];
    public credits!: number;
    public isChecked!: boolean;
    public isCollapsed!: boolean;
}
