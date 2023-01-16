import React, {useEffect, useState} from 'react';
import {Hero} from "../../../../../models/Hero";
import {BlobWithPath, heroImages} from "../../../../../assets/HeroImages";
import "./HeroSmallImage.scss";
import {HeroImageSize} from "../../../../../globalConstants";

interface IHeroSmallImage {
    hero: Hero;
    isRadiant: boolean;
}

const HeroSmallImage: React.FC<IHeroSmallImage> = ({hero, isRadiant}) => {
    const [heroBlobWithPath, setHeroBlobWithPath] = useState<BlobWithPath>(null);

    useEffect(() => {
        setHeroBlobWithPath(heroImages.get(hero.id));
    }, [heroImages])

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
                    height={HeroImageSize.small.height}/>
                : <></>
            }
        </div>
    );
};

export default HeroSmallImage;