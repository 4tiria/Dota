import React, {useEffect, useState} from 'react';
import {ICallBack} from "../../interfaces/ICallBack";
import Select from "react-select";
import {attributes} from "../../../styles/attributes";
import "./FilterStyles.scss";
import {HeroFilterModel} from "../../../models/filterModels/heroFilter";
import {updateHeroMainAttributeFilter, updateHeroNameFilter} from "../../../store/actionCreators/heroFilter";
import {useDispatch, useSelector} from "react-redux";
import {IRootState} from "../../../store/store";

const options = attributes.map(a => {
    return {value: a.name, label: <div style={{'color': a.color}}>{a.name}</div>}
}).concat({value: 'All', label: <div>Any</div>});

const AttributeFilter = () => {
    const heroAttributeFilter = useSelector<IRootState, string>(state => state.heroFilter.mainAttribute);
    const dispatch = useDispatch();

    return (
        <div>
            <Select
                className="attribute-combobox"
                placeholder="main attribute"
                options={options}
                value={options.find(x => x.value == heroAttributeFilter)}
                onChange={event => 
                    dispatch(updateHeroMainAttributeFilter(event.value))
                }
            />
        </div>
    );
};

export default AttributeFilter;