import { PrimePartIngredient } from './PrimePartIngredient'
import { IngredientsGroup } from './IngredientsGroup'

export interface PrimeItem {
    id: number;
    name: string;
    wikiUrl: string;
    primePartIngredients: PrimePartIngredient[];
    credits: number;
    ingredientsGroups: IngredientsGroup[];

    isChecked: boolean;
    isCollapsed: boolean;
}
