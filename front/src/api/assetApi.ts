import {Hero} from "../models/Hero";
import axios from "axios";
import {assetPath} from "./apiPaths";
import {Asset} from "../models/Asset";
import {api} from "./http";

export async function getAllAssets(): Promise<Asset[]> {
    const response = await api.get<Asset[]>(assetPath);
    return response.data;
}