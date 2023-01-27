import {Hero} from "../models/Hero";
import axios from "axios";
import {
    addHeroPath,
    deleteHeroPath,
    heroFilteredListPath,
    heroListPath,
    heroPath,
    tagPath
} from "./apiPaths";
import {Tag} from "../models/Tag";
import {HeroFilterModel} from "../models/filterModels/heroFilter";
import {HeroImage} from "../models/HeroImage";
import {configure} from "@testing-library/react";

import {api} from "./http";

export async function getAllHeroes(): Promise<Hero[]> {
    const response = await api.get<Hero[]>(heroListPath);
    return response.data;
}

export async function getFilteredList(filteredOptions: HeroFilterModel): Promise<Hero[]> {
    const response = await api.post<Hero[]>(heroFilteredListPath, filteredOptions);
    return response.data;
}

export async function getAllWithTag(tagName: string): Promise<Hero[]> {
    const response = await api.get<Hero[]>(`${heroListPath}/${tagName}`);
    return response.data;
}

export async function getById(id: number): Promise<Hero> {
    const response = await api.get<Hero>(heroPath + `/${id}`);
    return response.data;
}

export async function getByName(name: string): Promise<Hero> {
    const response = await api.post<Hero>(heroPath + `/byName`, {name: name});
    return response.data;
}

export async function updateHero(hero: Hero): Promise<Hero> {
    const response = await api.patch<Hero>(heroPath, hero);
    return response.data;
}

export async function addEmptyHero(): Promise<Hero> {
    const response = await api.post<Hero>(addHeroPath);
    return response.data;
}

export async function deleteHero(hero: Hero): Promise<Hero> {
    const response = await api.post<Hero>(deleteHeroPath, hero);
    return response.data;
}

export async function getTags(): Promise<Tag[]> {
    const response = await api.get<Tag[]>(tagPath);
    return response.data;
}

