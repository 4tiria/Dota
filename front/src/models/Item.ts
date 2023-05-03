import {ItemGroup} from "./ItemGroup";

export class Item{
    id: number;
    type: 'Main' | 'Neutral'
    group: ItemGroup;
}