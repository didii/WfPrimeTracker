export class ArgumentOutOfRangeError extends Error {
    constructor(argument: string, message?: string) {
        super(message ? message : 'Argument is out of range. Name: ' + argument);

        // Set the prototype explicitly.
        Object.setPrototypeOf(this, ArgumentOutOfRangeError.prototype);
    }
}
