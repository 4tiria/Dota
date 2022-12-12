import React, {createContext, useEffect, useState} from 'react';
import {Hero} from "../../models/Hero";
import {useLocation, useParams} from "react-router-dom";
import {getByName, getTags, postOrUpdate} from "../../api/heroApi";
import "./heroInfo.scss";
import "react-bootstrap";
import {IoBanOutline, IoCheckmarkOutline, IoCloseOutline, IoCreateOutline, IoRemoveOutline} from "react-icons/io5";
import "../../styles/App.scss"
import {Tag} from "../../models/Tag";

class EditHeroForm {
    name: '';
    mainAttribute: '';
    tags: [];
}

export const HeroInfo = () => {

    const params = useParams();
    const [hero, setHero] = useState<Hero>();
    const [editMode, setEdit] = useState(false);

    const [heroName, setHeroName] = useState('');
    const [heroAttribute, setHeroAttribute] = useState('');
    const [tags, setHeroTags] = useState<Tag[]>([]);
    const [tagPull, setTagPull] = useState<Tag[]>([]);

    const [editHeroForm, setForm] = useState({
        name: '',
        mainAttribute: '',
        tags: [],
    });

    useEffect(() => {
        getByName(params.name).then(response => {
            setHero(response);
            getTags().then(tagResponse => {
                let tagNames = response?.tags.map(t => t.name);
                tagResponse = tagResponse.filter(t => !tagNames.includes(t.name));
                setTagPull(tagResponse);
            });
        });

    }, []);

    const colors = {
        "Strength": "#bd0000",
        "Agility": "#34c001",
        "Intelligence": "#55a1ef",
    }


    function addTag(tag: Tag) {
        setTagPull(previousState => previousState.filter(t => t.name !== tag.name).sort((a, b) => a.name.localeCompare(b.name)))
        setHeroTags(previousState => [...previousState, tag].sort((a, b) => a.name.localeCompare(b.name)));
    }

    function removeTag(tag: Tag) {
        setHeroTags(previousState => previousState.filter(t => t.name !== tag.name).sort((a, b) => a.name.localeCompare(b.name)))
        setTagPull(previousState => [...previousState, tag].sort((a, b) => a.name.localeCompare(b.name)));
    }

    function getMainAttributeStyle(attributeName: string) {
        return {color: colors[attributeName]};
    }

    function startEdit() {
        setEdit(true);

        setHeroName(hero?.name);
        setHeroAttribute(hero?.mainAttribute);
        setHeroTags(hero?.tags);

        setForm({
            name: hero?.name,
            tags: hero?.tags.map(t => t?.name),
            mainAttribute: hero.mainAttribute
        });
    }

    function confirm() {
        let flag = true;
        setHero((prevState) => {
            const newState = {...prevState, tags: tags, name: heroName, mainAttribute: heroAttribute};
            if (flag){
                flag = false;
                postOrUpdate(newState).then(() => {
                    setEdit(false);
                });
            }
               
            return newState;
        });
    }

    function cancel() {
        setEdit(false);
    }

    function deleteHero() {

    }

    return (
        <div className="hero-container">
            <form>
                <div className="hero-edit-panel">
                    {editMode
                        ?
                        <div className="d-flex justify-content-center">
                            <div className="btn custom-icon" onClick={confirm}>
                                <IoCheckmarkOutline className="tick"/>
                            </div>
                            <div className="btn custom-icon" onClick={cancel}>
                                <IoCloseOutline className="cross"/>
                            </div>
                        </div>
                        :
                        <div className="d-flex justify-content-center">
                            <div className="btn custom-icon" onClick={startEdit}><IoCreateOutline/></div>
                            <div className="btn custom-icon"><IoBanOutline/></div>
                        </div>
                    }
                </div>
                <div className="hero-info">
                    <div className="d-flex justify-content-between">
                        <div className="hero-image"></div>
                        <div className="hero-header">
                            <div>
                                {editMode
                                    ?
                                    <input
                                        id="heroName"
                                        name="heroName"
                                        className="hero-name"
                                        onChange={event => setHeroName(event.target.value)}
                                        value={heroName}
                                    />
                                    : <div className="hero-name">{hero?.name}</div>
                                }
                            </div>

                            <div className="hero-attribute">
                                {editMode
                                    ?
                                    <select className="main-attribute"
                                            onChange={event => setHeroAttribute(event.target.value)}
                                            style={getMainAttributeStyle(heroAttribute)}
                                            value={heroAttribute}>
                                        <option value="Strength" style={getMainAttributeStyle("Strength")}>Strength
                                        </option>
                                        <option value="Agility" style={getMainAttributeStyle("Agility")}>Agility
                                        </option>
                                        <option value="Intelligence"
                                                style={getMainAttributeStyle("Intelligence")}>Intelligence
                                        </option>
                                    </select>
                                    :
                                    <div style={getMainAttributeStyle(hero?.mainAttribute)}>{hero?.mainAttribute}</div>
                                }
                            </div>

                            <div>
                                {editMode
                                    ?
                                    <div>
                                        <div className="hero-tags">
                                            {tags.map(tag => {
                                                return <div
                                                    onClick={() => removeTag(tag)}
                                                    className="btn btn-sm hero-tag hero-tag-selected">{tag?.name}</div>
                                            })}</div>
                                        <hr/>
                                        <div className="hero-tags">
                                            {tagPull.map(tag => {
                                                return <div
                                                    onClick={() => addTag(tag)}
                                                    className="btn btn-sm hero-tag hero-tag-in-pull">{tag?.name}</div>
                                            })}
                                        </div>
                                    </div>
                                    :
                                    <div className="hero-tags">
                                        {hero?.tags.map(tag => {
                                            return <div
                                                className="btn btn-sm hero-tag hero-tag-selected">{tag?.name}</div>
                                        })}</div>
                                }
                            </div>
                            <hr/>
                        </div>
                    </div>
                    <div className="hero-description">

                    </div>
                </div>
            </form>
        </div>
    )
        ;
};

export default HeroInfo;