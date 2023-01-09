import {Tag} from "../Tag";

export class HeroFilterModel{
    name: string;
    mainAttribute: string;
    attackType: string;
    tags: Tag[];
    
    constructor() {
        this.name = '';
        this.mainAttribute = 'All';
        this.attackType = 'All';
        this.tags = [];
    }
}