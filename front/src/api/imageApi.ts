import axios from "axios";
import {heroPath, imagePath} from "./apiPaths";
import {HeroImage} from "../models/HeroImage";

export async function postImage(blob: Blob, id: number): Promise<any> {
    const formData = new FormData();
    formData.append("image", blob);
    const response = await axios.patch<any>(`${heroPath}/${id}/image`, formData);
    return response.data;
}

export async function getImage(id: number): Promise<Blob> {
    const response = await axios.get<Blob>(`${heroPath}/${id}/image`, {responseType: 'blob'});
    return response.data;
}

export async function getAllHeroImages(): Promise<HeroImage[]> {
    const response = await axios.get<HeroImage[]>(`${imagePath}`);
    return response.data;
}

