export interface EnumOption<T extends number> {
    value: T;
    label: string;
}

export function enumToOptions<T extends Record<string, string | number>>(
    e: T
): EnumOption<number>[] {
    return Object.keys(e)
        .filter(k => !isNaN(Number(e[k]))) // keep only numbers
        .map(k => ({
            value: e[k] as number,
            label: k
        }));
}
