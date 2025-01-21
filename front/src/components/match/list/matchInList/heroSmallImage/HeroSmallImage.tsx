import React from 'react';
import { HeroImageSize } from "../../../../../globalConstants";
import { Hero } from "../../../../../models/Hero";
import "./HeroSmallImage.scss";

interface IHeroSmallImage {
    hero: Hero;
    isRadiant: boolean;
}

const HeroSmallImage: React.FC<IHeroSmallImage> = ({hero, isRadiant}) => {

    return (
        <div
            className={isRadiant
                ? "small-image-container skew-left border-radiant"
                : "small-image-container skew-right border-dire"}>
            <img
                className={isRadiant ? "skew-right" : "skew-left"}
                src={hero.imageLink}
                width={HeroImageSize.small.width}
                height={HeroImageSize.small.height}
                alt='unlucky bro =('/>
        </div>
    );
};

export default HeroSmallImage;