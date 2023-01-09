import React from 'react';
import {ICallBack} from "../../interfaces/ICallBack";
import Select from "react-select";
import {attackTypes} from "../../../styles/attackTypes";
import "./FilterStyles.scss";
import {useDispatch, useSelector} from "react-redux";
import {updateHeroAttackTypeFilter} from "../../../store/actionCreators/heroFilter";
import {IRootState} from "../../../store/store";


const options = attackTypes.map(at => {
    return {value: at.name, label: <div>{at.name} {at.icon}</div>}
}).concat({value: "All", label: <div>Any</div>});

const AttackTypeFilter = () => {
    const heroAttackTypeFilter = useSelector<IRootState, string>(state => state.heroFilter.attackType);
    const dispatch = useDispatch();

    return (
        <div>
            <Select
                className="attack-type-combobox"
                placeholder="attack type"
                options={options}
                value={options.find(x => x.value == heroAttackTypeFilter)}
                onChange={event => {
                    dispatch(updateHeroAttackTypeFilter(event.value))
                }}
            />
        </div>
    );
};

export default AttackTypeFilter;