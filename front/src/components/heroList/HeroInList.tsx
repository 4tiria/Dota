import React, {useEffect, useState} from 'react';
import {Hero} from "../../models/Hero";
import "./HeroList.scss"
import "../../styles/App.scss";
import {useNavigate} from "react-router-dom";
import {ToCamelCase} from "../../helpers/stringHelper";
import {IoAdd, IoCloseOutline} from "react-icons/io5";
import {addEmptyHero} from "../../api/heroApi";
import {ICallBack} from "../interfaces/ICallBack";
import {GiBroadsword, GiPocketBow} from "react-icons/gi";
import {attributes} from "../../styles/attributes";
import {useSelector} from "react-redux";
import {IRootState} from "../../store/store";
import {User} from "../../models/dto/User";
import { AssetImage } from '../../assets/AssetImage';

interface IHeroInList extends ICallBack<Hero> {
    hero: Hero;
    isAddButton: boolean;
    isEmpty: boolean;
    hasNoFilters: boolean
}

const light = 'rgba(255,255,255,0.9)';
const dark = 'rgba(0,0,0,0.8)';

const HeroInList: React.FC<IHeroInList> = (
    {
        hero,
        callBackFunction,
        isAddButton = false,
        isEmpty = false,
        hasNoFilters = false
    }) => {

    const [heroPngPath, setHeroPngPath] = useState<string>(null);
    const [isHovering, setIsHovering] = useState(false);
    const [crossIsHovered, setCrossIsHovered] = useState(false);

    const user = useSelector<IRootState, User>(state => state.user);
    const themeMode = useSelector<IRootState, Palette>(state => state.palette);
    let navigate = useNavigate();

    useEffect(() => {
        if(hero) {
            const imageAsset = new AssetImage(hero.image);
            setHeroPngPath(imageAsset.path);
        } else {
            
        }
    }, []);

    function addOrRedirectToHero() {
        if (isEmpty) {
            return;
        }

        if (isAddButton) {
            addEmptyHero().then(response => {
                navigate(`../hero/${response.id}`);
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
        return (<div className="attack-type">{hero?.attackType === "Melee"
            ? <GiBroadsword/>
            : <GiPocketBow/>
        }</div>);
    }

    function renderMainAttribute() {
        return (
            <div
                className="main-attribute"
                style={{
                    'color': attributes.find(a => a.name === hero?.mainAttribute).color
                }}
            >{hero.mainAttribute}</div>
        );
    }

    function renderTags() {
        return (
            <div className="d-flex justify-content-between tag">
                <div>{hero.tags?.map(tag =>
                    <span className="badge bg-secondary mx-1" key={tag}>{tag}</span>
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
                        {user.accessLevel === "Admin" && (
                            <IoCloseOutline
                                className={
                                    crossIsHovered
                                        ? "cross-hovered"
                                        : "cross-default"
                                }
                                onMouseOver={() => setCrossIsHovered(true)}
                                onMouseOut={() => setCrossIsHovered(false)}
                            />
                        )}
                    </div>
                </div>

            </div>);
    }

    return (
        <div className="hero-container">
            <div className={isEmpty
                ?
                hasNoFilters
                    ? "hero hero-empty hero-invisible"
                    : "hero hero-invisible"
                : (isHovering !== (themeMode === 'dark')
                    ? "hero hero-selected"
                    : "hero hero-default")}
                 onClick={addOrRedirectToHero}
                 style={{
                     backgroundImage: `url(${heroPngPath}), ${(isHovering !== (themeMode === 'dark'))
                         ? `linear-gradient(90deg, ${dark}, rgba(0,0,0,0.2))`
                         : `linear-gradient(90deg, ${light}, 50%, rgba(0,0,0,0.2) 70%)`
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