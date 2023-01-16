import React, {useState} from 'react';
import {IEditable} from "../interfaces/IEditable";
import {ICallBack} from "../interfaces/ICallBack";
import {Hero} from "../../models/Hero";
import Select from "react-select";
import {attributes} from "../../styles/attributes";

interface IHeroAttribute extends IEditable, ICallBack<string> {
    hero: Hero;
}

const colors = attributes.map(a => {
    return {value: a.name, label: <div style={{'color': a.color}}>{a.name}</div>}
});

function getMainAttributeStyle(attributeName: string) {
    return {color: colors[attributeName]};
}

const HeroAttribute: React.FC<IHeroAttribute> = ({editMode, callBackFunction, hero}) => {
    const [heroAttribute, setHeroAttribute] = useState(hero?.mainAttribute);
    
    function renderCombobox(){
        return (
            <Select className="select-container"
                options={colors}
                defaultValue={colors.find(c => c.value == heroAttribute)}
                onChange={event => {
                    setHeroAttribute(event.value);
                    callBackFunction(event.value);
                }}
            />
        )
    }
    
    function renderAttribute(){
        return (
            colors.find(c => c.value == hero?.mainAttribute)?.label
        )
    }
    
    return (
        <div className="hero-attribute">
            {editMode
                ?
                renderCombobox()
                :
                renderAttribute()
            }
        </div>
    );
};

export default HeroAttribute;