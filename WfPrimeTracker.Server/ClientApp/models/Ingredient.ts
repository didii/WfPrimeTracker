export class Ingredient {
    public id!: number;
    public name!: string;
    public quantity!: number;

    public constructor(init?: Partial<Ingredient>) {
        Object.assign(this, init);
    }
}