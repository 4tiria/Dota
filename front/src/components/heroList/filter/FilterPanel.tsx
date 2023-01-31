import React, {useEffect, useState} from 'react';
import NameFilter from "./NameFilter";
import AttributeFilter from "./AttributeFilter";
import AttackTypeFilter from "./AttackTypeFilter";
import TagFilter from "./TagFilter";
import {HeroFilterModel} from "../../../models/filterModels/heroFilter";
import {ICallBack} from "../../interfaces/ICallBack";
import {IoCloseOutline} from "react-icons/io5";
import {resetHeroFilters} from "../../../store/actionCreators/heroFilter";
import {useDispatch, useSelector} from "react-redux";
import {IRootState} from "../../../store/store";

interface IFilterPanel extends ICallBack<HeroFilterModel> {
}

export function noFilterApplied(heroFilterModel: HeroFilterModel): boolean {
    return heroFilterModel.tags.length == 0
        && heroFilterModel.name.length == 0
        && heroFilterModel.attackType == "All"
        && heroFilterModel.mainAttribute == "All";
}

const FilterPanel: React.FC<IFilterPanel> = ({callBackFunction}) => {
    const filters = useSelector<IRootState, HeroFilterModel>(state => state.heroFilter);
    const dispatch = useDispatch();
    const [resetIsHovered, setResetIsHovered] = useState<boolean>(false);

    useEffect(() => {
        callBackFunction(filters);
    }, [filters]);
    
    return (
        <div className="d-flex justify-content-center filter-panel">
            <NameFilter/>
            <AttributeFilter/>
            <AttackTypeFilter/>
            <TagFilter/>
            <div className="d-flex align-items-center mx-1">
                <div
                    onMouseOver={() => setResetIsHovered(true)}
                    onMouseLeave={() => setResetIsHovered(false)}
                    className={resetIsHovered ? "reset-cross-hovered" : "reset-cross-default"}
                    onClick={() => dispatch(resetHeroFilters())}>
                    <IoCloseOutline/>
                </div>
            </div>
         
        </div>
    );
};

export default FilterPanel;