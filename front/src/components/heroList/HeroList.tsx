import React, {useContext, useEffect, useMemo, useState} from 'react';
import {Hero} from "../../models/Hero";
import axios from "axios";
import HeroInList from "./HeroInList";
import {heroListPath} from "../../api/apiPaths";
import {generateUniqueID} from "web-vitals/dist/modules/lib/generateUniqueID";
import NameFilter from "./filter/NameFilter";
import {HeroFilterModel} from "../../models/filterModels/heroFilter";
import nameFilter from "./filter/NameFilter";
import {deleteHero, getAllHeroes, getFilteredList} from "../../api/heroApi";
import AttributeFilter from "./filter/AttributeFilter";
import AttackTypeFilter from "./filter/AttackTypeFilter";
import TagFilter from "./filter/TagFilter";
import FilterPanel, {noFilterApplied} from "./filter/FilterPanel";
import {useSelector} from "react-redux";
import {IRootState} from "../../store/store";
import {User} from "../../models/dto/User";
import {ThemeContext} from "../../context/ThemeContext";


const HeroList = (props) => {
    const {theme, setTheme} = useContext(ThemeContext);
    
    const [list, setList] = useState<Hero[]>([]);
    const [hasNoFilters, setHasNoFilters] = useState<boolean>(false);
    const [memorizedFilterModel, setMemorizedFilterModel] = useState<HeroFilterModel>(new HeroFilterModel());

    const user = useSelector<IRootState, User>(state => state.user);

    function getListOfElements(heroes: Hero[]): JSX.Element[] {
        let result = heroes.map(h =>
            (<HeroInList
                hero={h}
                callBackFunction={deleteHeroFromList}
                isAddButton={false}
                isEmpty={false}
                hasNoFilters={hasNoFilters}
                key={h.id}/>)
        );
        if (hasNoFilters && user.accessLevel == "Admin") {
            result.push(<HeroInList
                hero={null}
                callBackFunction={deleteHeroFromList}
                isAddButton={true}
                hasNoFilters={hasNoFilters}
                isEmpty={false}/>);
        }

        return result;
    }

    function splitListFor(columns: number, elements: JSX.Element[]) {
        let result: JSX.Element[][] = [];
        let rowNumber = -1;
        for (let i = 0; i < elements.length; i++) {
            if (i % columns == 0) {
                result.push([]);
                rowNumber++;
            }
            result[rowNumber].push(elements[i]);
        }

        return result;
    }

    function splitAndRenderAllRows(columns: number) {
        if (list?.length > 0) {
            let elements = getListOfElements(list);
            let splitList = splitListFor(columns, elements);

            return splitList.map((row, index) =>
                (
                    <div key={index}>
                        {renderRow(columns, row)}
                    </div>
                ));
        }
    }

    function deleteHeroFromList(hero: Hero) {
        deleteHero(hero).then(() => {
            applyFilters(memorizedFilterModel);
        });
    }

    function renderRow(columns: number, array: JSX.Element[]) {
        let difference = columns - array.length;
        if (difference > 0) {
            for (let i = 0; i < difference; i++) {
                array.push(
                    <HeroInList
                        hero={null}
                        callBackFunction={deleteHeroFromList}
                        isAddButton={false}
                        isEmpty={true}
                        hasNoFilters={hasNoFilters}
                        key={generateUniqueID()}/>
                )
            }
        }

        return (
            <div className="d-flex justify-content-center mx-5">
                {array.map(el =>
                    (<div key={el.key}>
                        {el}
                    </div>))}
            </div>
        )
    }

    function applyFilters(filterOptions: HeroFilterModel) {
        getFilteredList(filterOptions).then(response => {
            setList(response);
        });

        setHasNoFilters(noFilterApplied(filterOptions));
    }

    return (
        <div data-theme={theme}>
            <FilterPanel callBackFunction={(heroFilterModel) => {
                setMemorizedFilterModel(heroFilterModel);
                applyFilters(heroFilterModel);
            }}/>
            <div className="d-flex justify-content-center">
                <hr className="hr-hero"/>
            </div>
            <div className="hero-list">
                <div>
                    {splitAndRenderAllRows(4)}
                </div>
            </div>
        </div>

    );
}

export default HeroList;