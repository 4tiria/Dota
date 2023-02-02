import React, {useEffect, useState} from 'react';
import {Hero} from "../../models/Hero";
import {IEditable} from "../interfaces/IEditable";
import {ICallBack} from "../interfaces/ICallBack";
import {Input} from "@mui/material";

interface IHeroName extends IEditable, ICallBack<string> {
    hero: Hero;
}

const HeroName: React.FC<IHeroName> = ({editMode, hero, callBackFunction}) => {
    const [heroName, setHeroName] = useState(hero?.name);

    return (
        <div>
            {editMode
                ?
                <Input
                    sx={{
                      textAlign:"center",  
                    }}
                    id="heroName"
                    name="heroName"
                    className="hero-name"
                    inputProps={{min: 0, style: { textAlign: 'center' }}}
                    onChange={
                        event => {
                            let newValue = event.target.value;
                            callBackFunction(newValue);
                            setHeroName(newValue);
                        }
                    }
                    value={heroName}
                />
                : <div className="hero-name">{hero?.name}</div>
            }
        </div>
    );
};

export default HeroName;