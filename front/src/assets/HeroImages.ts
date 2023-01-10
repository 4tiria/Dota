import {useEffect, useState} from "react";
import {HeroImage} from "../models/HeroImage";
import {getAllHeroImages} from "../api/imageApi";
import {getBlobFromBase64} from "../helpers/blobHelper";

export class BlobWithPath {
    blob: Blob;
    path: string;

    constructor(blob: Blob) {
        this.blob = blob;
        this.path = window.URL.createObjectURL(blob);
    }
}

export let heroImages: Map<number, BlobWithPath> = new Map<number, BlobWithPath>([]);

export async function downloadHeroImages(): Promise<any> {
    getAllHeroImages().then(x => {
            heroImages = new Map(x.map(i => [i.heroId,
                new BlobWithPath(getBlobFromBase64(i.bytes))
            ]))
        }
    );
}
