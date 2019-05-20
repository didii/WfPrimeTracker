interface IUtils {
    default<T>(...exprs: ((() => T) | T)[]): T;
}

let Utils: IUtils = {
    default<T>(...exprs: ((() => T) | T)[]): T {
        let result: T;
        for (const expr of exprs) {
            try {
                if (typeof expr === 'function') {
                    result = (<Function>expr)();
                } else {
                    result = expr;
                }
                if (result != null) {
                    return result;
                }
            } catch (error) {}
        }
        throw Error('All given values were null or undefined');
    }
}

export default Utils;