import React, {useEffect, useState} from 'react';
import {Hero} from "../../../../../models/Hero";
import {BlobWithPath, heroImages} from "../../../../../assets/HeroImages";
import "./HeroSmallImage.scss";

interface IHeroSmallImage {
    hero: Hero;
    isRadiant: boolean;
}

const HeroSmallImage: React.FC<IHeroSmallImage> = ({hero, isRadiant}) => {
    const [heroBlobWithPath, setHeroBlobWithPath] = useState<BlobWithPath>(null);

    useEffect(() => {
        let forDebug = heroImages.get(hero.id);
        setHeroBlobWithPath(forDebug);
    }, [heroImages])

    return (
       
            <div className={isRadiant ? "small-image-container skew-left" : "small-image-container skew-right"}>
                {heroBlobWithPath ?
                    <img
                        className={isRadiant ? "skew-right" : "skew-left"}
                        src={heroBlobWithPath.path}
                        width={128}
                        height={72}/>
                    : <></>
                }
            </div>
    );
};

export default HeroSmallImage;