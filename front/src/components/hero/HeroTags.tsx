import React, { useEffect, useState } from 'react';
import { Hero } from "../../models/Hero";
import { Role } from '../../models/Role';
import { ICallBack } from "../interfaces/ICallBack";
import { IEditable } from "../interfaces/IEditable";

interface IHeroTags extends IEditable, ICallBack<Role[]> {
    hero: Hero;
}

class TagSelection {
    tag: Role;
    isSelected: boolean;
}

const HeroTags: React.FC<IHeroTags> = ({hero, editMode, callBackFunction}) => {
    const [tags, setHeroTags] = useState<Role[]>(hero.roles);
    const [tagPull, setTagPull] = useState<Role[]>([]);
    const [allTags, setAllTags] = useState<TagSelection[]>();

    useEffect(() => {
        const tags = Object.values(Role).filter(value => typeof value === 'number') as Role[];
        setAllTags(tags.map(t => {
            return {tag: t, isSelected: hero.roles.includes(t)};
        }));

        setTagPull(tags.filter(t => !hero.roles.includes(t)));
    }, []);

    function changeTag(ts: TagSelection) {
        if (ts.isSelected)
            removeTag(ts.tag)
        else
            addTag(ts.tag);

        ts.isSelected = !ts.isSelected;
    }

    function addTag(tag: Role) {
        setTagPull(previousState => previousState.filter(t => t !== tag).sort((a, b) => Role[a].localeCompare(Role[b])))
        setHeroTags(previousState => {
                let newTagArray = [...previousState, tag].sort((a, b) => Role[a].localeCompare(Role[b]));
                callBackFunction(newTagArray);
                return newTagArray;
            }
        );
    }

    function removeTag(tag: Role) {
        setHeroTags(previousState => {
                let newTagArray = previousState.filter(t => t !== tag).sort((a, b) => Role[a].localeCompare(Role[b]));
                callBackFunction(newTagArray);
                return newTagArray;
            }
        );
        setTagPull(previousState => [...previousState, tag].sort((a, b) => Role[a].localeCompare(Role[b])));
    }

    function renderTwoPulls() {
        if (editMode) {
            return (
                <div>
                    <div className="hero-tags">
                        {tags.map(tag => {
                            return <div
                                onClick={() => removeTag(tag)}
                                className="btn btn-sm hero-tag hero-tag-selected" key={tag}>
                                    {tag}
                            </div>
                        })}</div>
                    <hr/>
                    <div className="hero-tags">
                        {tagPull.map(tag => {
                            return <div
                                onClick={() => addTag(tag)}
                                className="btn btn-sm hero-tag hero-tag-in-pull" key={tag}>
                                {tag}
                            </div>
                        })}
                    </div>
                </div>
            );
        }

        return (<div className="hero-tags">
            {hero?.roles.map(tag => {
                return <div
                    className="btn btn-sm hero-tag hero-tag-own" key={tag}>{tag}</div>
            })}</div>)
    }

    function renderInline() {
        if (editMode) {
            return (
                <div className="hero-tags">
                    {allTags?.map(ts => {
                        return <div
                            className={ts.isSelected
                                ? "btn btn-sm hero-tag hero-tag-selected"
                                : "btn btn-sm hero-tag hero-tag-deselected"}
                            key={ts.tag}
                            onClick={() => changeTag(ts)}
                        >
                            {ts.tag}
                        </div>
                    })}
                </div>
            )
        }

        return (
            <div className="hero-tags">
                {hero?.roles.map(tag => {
                    return <div
                        className="btn btn-sm hero-tag hero-tag-own" key={tag}>{tag}</div>
                })}</div>
        )
    }

    return (
        <div>
            {renderInline()}
        </div>
    );
};

export default HeroTags;