import React, {useEffect, useState} from 'react';
import {Hero} from "../../../../../models/Hero";
import "./HeroSmallImage.scss";
import {HeroImageSize} from "../../../../../globalConstants";
import { AssetImage } from '../../../../../assets/AssetImage';

interface IHeroSmallImage {
    hero: Hero;
    isRadiant: boolean;
}

const HeroSmallImage: React.FC<IHeroSmallImage> = ({hero, isRadiant}) => {
    const [heroBlobWithPath, setHeroBlobWithPath] = useState<AssetImage>(null);

    useEffect(() => {
        setHeroBlobWithPath(new AssetImage(hero.image));
    }, [])

    return (
        <div
            className={isRadiant
                ? "small-image-container skew-left border-radiant"
                : "small-image-container skew-right border-dire"}>
            {heroBlobWithPath ?
                <img
                    className={isRadiant ? "skew-right" : "skew-left"}
                    src={heroBlobWithPath.path}
                    width={HeroImageSize.small.width}
                    height={HeroImageSize.small.height}
                    alt='unlucky bro =('/>
                : <></>
            }
        </div>
    );
};

export default HeroSmallImage;