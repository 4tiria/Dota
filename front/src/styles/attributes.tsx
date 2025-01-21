import { MainAttribute } from "models/Hero";

export const attributes: {name: MainAttribute | "All", color: string}[] = [
    {name: MainAttribute.Strength, color: '#bd0000'},
    {name: MainAttribute.Agility, color: '#34c001'},
    {name: MainAttribute.Intelligence, color: '#55a1ef'},
    {name: MainAttribute.Universal, color: '#55a1ef'},
]