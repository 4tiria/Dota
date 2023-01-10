import React, {useEffect, useState} from 'react';
import {Hero} from "../../../../../models/Hero";
import {BlobWithPath, heroImages} from "../../../../../assets/HeroImages";
import "./HeroSmallImage.scss";

interface IHeroSmallImage {
    hero: Hero;
}

const HeroSmallImage: React.FC<IHeroSmallImage> = ({hero}) => {
    const [heroBlobWithPath, setHeroBlobWithPath] = useState<BlobWithPath>(null);

    useEffect(() => {
        let forDebug = heroImages.get(hero.id);
        setHeroBlobWithPath(forDebug);
    }, [])

    return (

        <div className="small-image-container">
            {heroBlobWithPath ?
                <img src={heroBlobWithPath.path}
                     width={64}
                     height={36}/>
                : <></>
            }

        </div>
    );
};

export default HeroSmallImage;