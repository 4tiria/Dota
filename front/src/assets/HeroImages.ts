import {useEffect, useState} from "react";
import {HeroImage} from "../models/HeroImage";
import {getAllHeroImages} from "../api/imageApi";
import {getBlobFromBase64} from "../helpers/blobHelper";

export let heroImages: Map<number, Blob> = new Map<number, Blob>([]);

export async function downloadHeroImages(): Promise<any>{
    getAllHeroImages().then(x => {
            heroImages = new Map(x.map(i => [i.heroId, getBlobFromBase64(i.bytes)]))
        }
    );
}