import { PrimePart } from "./PrimePart";
import { Ingredient } from './Ingredient';

export class PrimeItem {
    public id!: number;
    public name!: string;
    public wikiUrl!: string;
    public primeParts!: PrimePart[];
    public ingredients!: Ingredient[];
    public credits!: number;
    public isChecked!: boolean;
    public isCollapsed!: boolean;

    public constructor(init?: Partial<PrimeItem>) {
        Object.assign(this, init);
    }
}
