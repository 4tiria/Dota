export class Hero {
    id: string
    name: string
    attackType: AttackType
    mainAttribute: MainAttribute
    roles: string[]
    imageLink: string
}

export declare type AttackType = 'Melee' | 'Ranged'

export declare type MainAttribute =
    | 'Strength'
    | 'Agility'
    | 'Intelligence'
    | 'Universal'
