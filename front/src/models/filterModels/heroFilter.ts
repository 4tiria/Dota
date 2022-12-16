export class HeroFilterModel{
    name: string = '';
    mainAttribute?: 'Strength' | 'Agility' | 'Intelligence';
    attackType?: 'Melee' | 'Range';
    tags?: [];
    
    constructor() {
    }
}