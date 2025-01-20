export class Hero {
    id: string;
    name: string;
    attackType: AttackType;
    mainAttribute: MainAttribute;
    roles: string[];
    imageLink: string;
}

export enum AttackType {
    Melee = 0,
    Ranged = 1
}

export enum MainAttribute
{
    Strength = 0,
    Agility = 1,
    Intelligence = 2,
    Universal = 3
}
