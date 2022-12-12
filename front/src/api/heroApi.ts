import {Hero} from "../models/Hero";
import axios from "axios";
import {heroListPath, heroPath, tagPath} from "./apiPaths";
import {Tag} from "../models/Tag";

export async function getAll() : Promise<Hero[]>{
    const response = await axios.get<Hero[]>(heroListPath);
    return response.data;
}

export async function getAllWithTag(tagName: string) : Promise<Hero[]>{
    const response = await axios.get<Hero[]>(`${heroListPath}/${tagName}`);
    return response.data;
}

export async function get(id: number) : Promise<Hero>{
    const response = await axios.get<Hero>(heroPath + `/${id}`);
    return response.data;
}

export async function getByName(name: string) : Promise<Hero>{
    const response = await axios.post<Hero>(heroPath + `/byName`, {name: name});
    return response.data;
}

export async function postOrUpdate(hero: Hero): Promise<Hero>{
    const response = await axios.post<Hero>(heroPath, hero);
    return response.data;
}

export async function getTags(): Promise<Tag[]>{
    const response = await axios.get<Tag[]>(tagPath);
    return response.data;
}
