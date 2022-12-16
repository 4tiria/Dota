import React, {useState} from 'react';
import "./heroInfo.scss";
import {ICallBack} from "../interfaces/ICallBack";
import {IEditable} from "../interfaces/IEditable";
import {GiBroadsword, GiPocketBow} from "react-icons/gi";
import {Hero} from "../../models/Hero";
import Select from 'react-select';

interface IHeroAttackType extends IEditable, ICallBack<string> {
    hero: Hero;
}

const HeroAttackType: React.FC<IHeroAttackType> = ({editMode, callBackFunction, hero}) => {
    const [heroAttackType, setHeroAttackType] = useState(hero?.attackType);
    
    const attackTypes = [
        {value: "Melee", label: <div>Melee <GiBroadsword/></div>},
        {value: "Range", label: <div>Range <GiPocketBow/></div>}];

    function renderCombobox() {
        return (
            <Select className="select-container"
                    options={attackTypes}
                    defaultValue={attackTypes.find(at => at.value == heroAttackType)}
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
                {attackTypes.find(at => at.value == heroAttackType).label}
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