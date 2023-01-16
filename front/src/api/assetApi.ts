import {Hero} from "../models/Hero";
import axios from "axios";
import {assetPath} from "./apiPaths";
import {Asset} from "../models/Asset";

export async function getAllAssets(): Promise<Asset[]> {
    const response = await axios.get<Asset[]>(assetPath);
    return response.data;
}