import { AttackType, MainAttribute } from "models/Hero";

export class HeroFilterModel{
    name: string;
    mainAttribute: MainAttribute | 'All';
    attackType: AttackType | 'All';
    roles: string[];
    
    constructor() {
        this.name = '';
        this.mainAttribute = 'All';
        this.attackType = 'All';
        this.roles = [];
    }
}