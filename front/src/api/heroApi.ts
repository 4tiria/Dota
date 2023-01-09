﻿import {Hero} from "../models/Hero";
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

export async function getAll(): Promise<Hero[]> {
    const response = await axios.get<Hero[]>(heroListPath);
    return response.data;
}

export async function getFilteredList(filteredOptions: HeroFilterModel): Promise<Hero[]> {
    const response = await axios.post<Hero[]>(heroFilteredListPath, filteredOptions);
    return response.data;
}

export async function getAllWithTag(tagName: string): Promise<Hero[]> {
    const response = await axios.get<Hero[]>(`${heroListPath}/${tagName}`);
    return response.data;
}

export async function getById(id: number): Promise<Hero> {
    const response = await axios.get<Hero>(heroPath + `/${id}`);
    return response.data;
}

export async function getByName(name: string): Promise<Hero> {
    const response = await axios.post<Hero>(heroPath + `/byName`, {name: name});
    return response.data;
}

export async function updateHero(hero: Hero): Promise<Hero> {
    const response = await axios.patch<Hero>(heroPath, hero);
    return response.data;
}

export async function addEmptyHero(): Promise<Hero> {
    const response = await axios.post<Hero>(addHeroPath);
    return response.data;
}

export async function deleteHero(hero: Hero): Promise<Hero> {
    const response = await axios.post<Hero>(deleteHeroPath, hero);
    return response.data;
}

export async function getTags(): Promise<Tag[]> {
    const response = await axios.get<Tag[]>(tagPath);
    return response.data;
}

export async function postImage(blob: Blob, id: number): Promise<any> {
    const formData = new FormData();
    formData.append("image", blob);
    const response = await axios.patch<any>(`${heroPath}/${id}/image`, formData);
    return response.data;
}

export async function getTestImage(id: number): Promise<Blob> {
    const response = await axios.get<Blob>(`${heroPath}/${id}/image`, {responseType: 'blob'});
    return response.data;
}

