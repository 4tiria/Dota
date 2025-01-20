import { Role } from "../models/Role";

function createEnumFromStringFunction<T>(enumObj: T) {
    return (tagName: string): T[keyof T] | undefined => {
        return (enumObj as any)[tagName as keyof T];
    };
}

export const getTagFromString = createEnumFromStringFunction(Role);

export const getAllTags = (): Role[] => {
    return Object.values(Role).filter(value => typeof value === 'number') as Role[]
}