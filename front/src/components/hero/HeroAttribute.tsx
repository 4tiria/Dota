import { MenuItem, Select } from "@mui/material";
import React, { useState } from 'react';
import { Hero, MainAttribute } from "../../models/Hero";
import { attributes } from "../../styles/attributes";
import { ICallBack } from "../interfaces/ICallBack";
import { IEditable } from "../interfaces/IEditable";

interface IHeroAttribute extends IEditable, ICallBack<MainAttribute> {
    hero: Hero;
}

const colors = attributes.map(attribute => {
    return {value: attribute.name, label: <div style={{'color': attribute.color}}>{attribute.name}</div>}
});

const HeroAttribute: React.FC<IHeroAttribute> = ({editMode, callBackFunction, hero}) => {
    const [heroAttribute, setHeroAttribute] = useState(hero?.mainAttribute);

    function renderCombobox() {
        return (
            <Select className="select-container"
                    value={heroAttribute}
                    onChange={event => {
                        let value = event.target.value as MainAttribute;
                        setHeroAttribute(value);
                        callBackFunction(value);
                    }}
            >
                {colors.map(x =>
                    <MenuItem
                        value={x.value}
                        key={x.value}>
                        {x.label}
                    </MenuItem>)}
            </Select>
        )
    }

    function renderAttribute() {
        return (
            colors.find(c => c.value === hero?.mainAttribute)?.label
        )
    }

    return (
        <div className="hero-attribute">
            {editMode
                ?
                <>
                    {renderCombobox()}
                </>
                :
                renderAttribute()
            }
        </div>
    );
};

export default HeroAttribute;