import React, { useEffect, useState } from 'react'
import { IoCloseOutline } from 'react-icons/io5'
import { useDispatch, useSelector } from 'react-redux'
import { heroFilterSlice } from 'store/reducers/heroFilterOptionsReducer'
import { HeroFilterModel } from '../../../models/filterModels/heroFilter'
import { IRootState } from '../../../store/store'
import { ICallBack } from '../../interfaces/ICallBack'
import AttackTypeFilter from './AttackTypeFilter'
import AttributeFilter from './AttributeFilter'
import NameFilter from './NameFilter'
import TagFilter from './TagFilter'

interface IFilterPanel extends ICallBack<HeroFilterModel> {}

export function noFilterApplied(heroFilterModel: HeroFilterModel): boolean {
    return (
        heroFilterModel.roles.length === 0 &&
        heroFilterModel.name.length === 0 &&
        heroFilterModel.attackType === 'All' &&
        heroFilterModel.mainAttribute === 'All'
    )
}

const FilterPanel: React.FC<IFilterPanel> = ({ callBackFunction }) => {
    const filters = useSelector<IRootState, HeroFilterModel>(
        (state) => state.heroFilter
    )
    const dispatch = useDispatch()
    const [resetIsHovered, setResetIsHovered] = useState<boolean>(false)

    useEffect(() => {
        callBackFunction(filters)
    }, [callBackFunction, filters])

    return (
        <div className="d-flex justify-content-center filter-panel">
            <NameFilter />
            <AttributeFilter />
            <AttackTypeFilter />
            <TagFilter />
            <div className="d-flex align-items-center mx-1">
                <div
                    onMouseOver={() => setResetIsHovered(true)}
                    onMouseLeave={() => setResetIsHovered(false)}
                    className={
                        resetIsHovered
                            ? 'reset-cross-hovered'
                            : 'reset-cross-default'
                    }
                    onClick={() => dispatch(heroFilterSlice.actions.reset())}
                >
                    <IoCloseOutline />
                </div>
            </div>
        </div>
    )
}

export default FilterPanel
