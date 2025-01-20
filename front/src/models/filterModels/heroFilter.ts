import { Role } from "../Role";

export class HeroFilterModel{
    name: string;
    mainAttribute: string;
    attackType: string;
    tags: Role[];
    
    constructor() {
        this.name = '';
        this.mainAttribute = 'All';
        this.attackType = 'All';
        this.tags = [];
    }
}