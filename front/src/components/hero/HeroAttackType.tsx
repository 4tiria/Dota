import React, {useState} from 'react';
import "./HeroInfo.scss";
import {ICallBack} from "../interfaces/ICallBack";
import {IEditable} from "../interfaces/IEditable";
import {Hero} from "../../models/Hero";
import Select from 'react-select';
import {attackTypes} from "../../styles/attackTypes";

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
                    options={attackTypesOptions}
                    defaultValue={attackTypesOptions.find(at => at.value == heroAttackType)}
                    onChange={e => {
                        setHeroAttackType(e.value);
                        callBackFunction(e.value);
                    }}
            />
        );
    }
    
    function renderAttackType(){
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