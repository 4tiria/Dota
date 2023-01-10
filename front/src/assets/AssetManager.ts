import {downloadHeroImages} from "./HeroImages";

export async function downloadAllAssets(): Promise<any>{
    await downloadHeroImages();
}