import React from 'react';
import { HeroImageSize } from "../../globalConstants";
import { Hero } from "../../models/Hero";
import { IEditable } from "../interfaces/IEditable";
import "./HeroInfo.scss";

interface IHeroImage extends IEditable {
    hero: Hero;
}

const HeroImage: React.FC<IHeroImage> = ({hero, editMode}) => {
  
    return (
        <div className="image-container">
            <img
                src={hero.imageLink}
                style={{
                    boxShadow: editMode ? '0px 0px 20px rgba(7, 230, 55, 0.5)' : '',
                    opacity: editMode ? '20%' : '100%',
                    width: HeroImageSize.full.width,
                    height: HeroImageSize.full.height,
                    display: 'flex',
                    border: '2px solid tomato',
                    borderRadius: 3
                }}
                alt='down to earth'/>
            {editMode ? <div>Перетащите .png</div> : <></>}
        </div>
    );
};

export default HeroImage;


