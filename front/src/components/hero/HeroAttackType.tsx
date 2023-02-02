import React, {useState} from 'react';
import "./HeroInfo.scss";
import {ICallBack} from "../interfaces/ICallBack";
import {IEditable} from "../interfaces/IEditable";
import {Hero} from "../../models/Hero";
import {attackTypes} from "../../styles/attackTypes";
import {MenuItem, Select} from "@mui/material";

interface IHeroAttackType extends IEditable, ICallBack<string> {
    hero: Hero;
}

const attackTypesOptions = attackTypes.map(at => {
    return {value: at.name, label: <div>{at.name} {at.icon}</div>}
});

const HeroAttackType: React.FC<IHeroAttackType> = ({editMode, callBackFunction, hero}) => {
    const [heroAttackType, setHeroAttackType] = useState(hero?.attackType);

    function renderCombobox() {
        return (
            <Select className="select-container"
                    value={heroAttackType}
                    onChange={e => {
                        let value = e.target.value as string;
                        setHeroAttackType(value);
                        callBackFunction(value);
                    }}
            >
                {attackTypesOptions.map(x =>
                    <MenuItem
                        value={x.value}
                        key={x.value}
                    >
                        {x.label}
                    </MenuItem>)}
            </Select>
        );
    }

    function renderAttackType() {
        return (
            <div>
                {attackTypesOptions.find(at => at.value == heroAttackType)?.label}
            </div>
        )
    }

    return (
        <div className="attack-type">
            {editMode
                ?
                renderCombobox()
                :
                renderAttackType()
            }
        </div>
    );
};

export default HeroAttackType;