import React, {useState} from 'react';
import {Hero} from "../../models/Hero";
import "./HeroList.scss"
import "../../styles/App.scss";
import {redirect, useNavigate} from "react-router-dom";
import {ToCamelCase} from "../../helpers/stringHelper";

const HeroInList = (props: {hero: Hero}) => {
    const [isHovering, setIsHovering] = useState(false);
    let navigate = useNavigate();
    
    function redirectToHero(){
        let urlHeroName = ToCamelCase(props.hero.name);
        navigate(`../hero/${urlHeroName}`);
    }
    
    return (
        <div className="hero">
            <div className={isHovering ? "hero-selected" : "hero-default"}
                 onClick={redirectToHero}
                 onMouseOver={() => setIsHovering(true)}
                 onMouseOut={() => setIsHovering(false)}
            >
                <div>{props.hero.name}</div>
                <div className="main-attribute">{props.hero.mainAttribute}</div>
                <div>
                    {props.hero.tags?.map(tag =>
                        <span className="badge bg-secondary mx-2">{tag.name}</span>
                    )}
                </div>
            </div>
        </div>
        
    );
}

export default HeroInList;