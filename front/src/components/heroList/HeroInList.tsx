import React, {useEffect, useState} from 'react';
import {Hero} from "../../models/Hero";
import "./HeroList.scss"
import "../../styles/App.scss";
import {redirect, useNavigate} from "react-router-dom";
import {ToCamelCase} from "../../helpers/stringHelper";
import {Button} from "@mui/material";
import {IoAdd, IoCloseOutline} from "react-icons/io5";
import {addEmptyHero, deleteHero, updateHero} from "../../api/heroApi";
import {ICallBack} from "../interfaces/ICallBack";
import {GiBroadsword, GiPocketBow, GiPointySword} from "react-icons/gi";
import {attributes} from "../../styles/attributes";
import {getBlobFromBase64} from "../../helpers/blobHelper";

interface IHeroInList extends ICallBack<Hero> {
    hero: Hero;
    isAddButton: boolean;
    isEmpty: boolean;
    hasFilters: boolean
}

const HeroInList: React.FC<IHeroInList> = (
    {
        hero,
        callBackFunction,
        isAddButton = false,
        isEmpty = false,
        hasFilters = false
    }) => {

    const [heroPngPath, setHeroPngPath] = useState<string>(null);
    const [isHovering, setIsHovering] = useState(false);
    const [crossIsHovered, setCrossIsHovered] = useState(false);

    let navigate = useNavigate();
    

    function addOrRedirectToHero() {
        if (isEmpty) {
            return;
        }

        if (isAddButton) {
            addEmptyHero().then(response => {
                navigate(`../hero/${response.id}`)
            });

            return;
        }

        if (crossIsHovered) {
            callBackFunction(hero);
            return;
        }

        let urlHeroName = ToCamelCase(hero?.name);
        navigate(`../hero/${urlHeroName}`);
    }


    function renderAttackType() {
        return (<div className="attack-type">{hero?.attackType == "Melee"
            ? <GiBroadsword/>
            : <GiPocketBow/>
        }</div>);
    }

    function renderMainAttribute() {
        return (
            <div
                className="main-attribute"
                style={{
                    'color': attributes.find(a => a.name == hero?.mainAttribute).color
                }}
            >{hero.mainAttribute}</div>
        );
    }

    function renderTags() {
        return (
            <div className="d-flex justify-content-between tag">
                <div>{hero.tags?.map(tag =>
                    <span className="badge bg-secondary mx-1" key={tag?.name}>{tag.name}</span>
                )}</div>
            </div>
        );
    }

    function renderHeroContainer() {
        if (isEmpty) {
            return (
                <div className="empty">

                </div>
            )
        }

        if (isAddButton) {
            return (
                <div className="d-flex justify-content-center h-100 align-items-center text-center">
                    <div className="plus"><IoAdd/></div>
                </div>);
        }

        return (
            <div>
                <div className="d-flex justify-content-between w-100">
                    <div>
                        <div className="hero-name">{hero.name}</div>
                        <div className="fix-padding d-flex justify-content-start">
                            {renderAttackType()}
                            {renderMainAttribute()}
                        </div>
                        {renderTags()}
                    </div>
                    <div>
                        <IoCloseOutline className={crossIsHovered ? "cross-hovered" : "cross-default"}
                                        onMouseOver={() => setCrossIsHovered(true)}
                                        onMouseOut={() => setCrossIsHovered(false)}/>
                    </div>
                </div>

            </div>);
    }

    return (
        <div className="hero-container">
            <div className={isEmpty
                ?
                hasFilters
                    ? "hero hero-empty"
                    : "hero hero-invisible"
                : (isHovering
                    ? "hero hero-selected"
                    : "hero hero-default")}
                 onClick={addOrRedirectToHero}
                 style={{
                     backgroundImage: `url(${heroPngPath}), ${isHovering
                         ? 'linear-gradient(90deg, rgba(0,0,0,0.8), rgba(0,0,0,0.8))'
                         : 'linear-gradient(90deg, rgba(255,255,255,1) 33%, rgba(0,0,0,0.2) 70%)'
                     }`,
                 }}
                 onMouseOver={() => setIsHovering(true)}
                 onMouseOut={() => setIsHovering(false)}
            >
                <div className="container-padding">
                    {renderHeroContainer()}
                </div>
            </div>
        </div>
    );
}

export default HeroInList;