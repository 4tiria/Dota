import React, {useState} from 'react';
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

interface IHeroInList {
    hero: Hero;
    isAddButton: boolean;
    isEmpty: boolean;
}

const HeroInList: React.FC<IHeroInList> = ({hero, isAddButton = false, isEmpty = false}) => {
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
            deleteHero(hero).then();
            return;
        }

        let urlHeroName = ToCamelCase(hero?.name);
        navigate(`../hero/${urlHeroName}`);
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

        return (<>
            <div className="d-flex justify-content-between w-100">
                <div>
                    <div className="hero-name">{hero.name}</div>
                    <div className="fix-padding d-flex justify-content-start">
                        <div className="attack-type">{hero?.attackType == "Melee"
                            ? <GiBroadsword/>
                            : <GiPocketBow/>
                        }</div>
                        <div
                            className="main-attribute"
                            style={{
                                'color': attributes.find(a => a.name == hero?.mainAttribute).color
                            }}
                        >{hero.mainAttribute}</div>
                    </div>
                    <div className="d-flex justify-content-between tag">
                        <div>{hero.tags?.map(tag =>
                            <span className="badge bg-secondary mx-1" key={tag?.name}>{tag.name}</span>
                        )}</div>

                    </div>
                </div>
                <div>
                    <IoCloseOutline className={crossIsHovered ? "cross-hovered" : "cross-default"}
                                    onMouseOver={() => setCrossIsHovered(true)}
                                    onMouseOut={() => setCrossIsHovered(false)}/>
                </div>
            </div>

        </>);
    }

    return (
        <div className={isEmpty
            ? "hero hero-empty"
            : (isHovering
                ? "hero hero-selected"
                : "hero hero-default")}
             onClick={addOrRedirectToHero}
             onMouseOver={() => setIsHovering(true)}
             onMouseOut={() => setIsHovering(false)}
        >
            {renderHeroContainer()}
        </div>
    );
}

export default HeroInList;