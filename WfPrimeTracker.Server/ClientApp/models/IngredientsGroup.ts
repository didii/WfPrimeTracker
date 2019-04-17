import { ResourceIngredient } from './ResourceIngredient'

export interface IngredientsGroup {
    id: number;
    name: string | null;
    resourceIngredients: ResourceIngredient[];
}
