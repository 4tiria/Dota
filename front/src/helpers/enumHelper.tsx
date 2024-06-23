import { Tag } from "../models/Tag";

function createEnumFromStringFunction<T>(enumObj: T) {
    return (tagName: string): T[keyof T] | undefined => {
        return (enumObj as any)[tagName as keyof T];
    };
}

export const getTagFromString = createEnumFromStringFunction(Tag);

export const getAllTags = (): Tag[] => {
    return Object.values(Tag).filter(value => typeof value === 'number') as Tag[]
}