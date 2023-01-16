import {getAllHeroImages} from "../api/imageApi";
import {getBlobFromBase64} from "../helpers/blobHelper";
import {BlobWithPath, heroImages} from "./HeroImages";
import {getAllAssets} from "../api/assetApi";

export let commonIcons: Map<string, BlobWithPath> = new Map<string, BlobWithPath>([]);

export async function downloadCommonIcons(): Promise<any> {
    getAllAssets().then(x => {
        commonIcons = new Map(x.map(i => [i.name,
                new BlobWithPath(getBlobFromBase64(i.bytes))
            ]))
        }
    );
}
