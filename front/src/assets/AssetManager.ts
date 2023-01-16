import {downloadHeroImages} from "./HeroImages";
import {downloadCommonIcons} from "./CommonIcons";

export async function downloadAllAssets(): Promise<any>{
    await downloadHeroImages();
    await downloadCommonIcons();
}

export const DireLogo = "Dire_logo.png";
export const RadiantLogo = "Radiant_logo.png";
