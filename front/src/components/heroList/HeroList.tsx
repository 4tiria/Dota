import React, {useEffect, useMemo, useState} from 'react';
import {Hero} from "../../models/Hero";
import axios from "axios";
import HeroInList from "./HeroInList";
import {heroListPath} from "../../api/apiPaths";
import {generateUniqueID} from "web-vitals/dist/modules/lib/generateUniqueID";
import NameFilter from "./filter/NameFilter";
import {HeroFilterModel} from "../../models/filterModels/heroFilter";
import nameFilter from "./filter/NameFilter";
import {deleteHero, getFilteredList} from "../../api/heroApi";
import AttributeFilter from "./filter/AttributeFilter";
import AttackTypeFilter from "./filter/AttackTypeFilter";
import TagFilter from "./filter/TagFilter";
import FilterPanel, {noFilterApplied} from "./filter/FilterPanel";


const HeroList = (props) => {
    const [list, updateList] = useState<Hero[]>([]);
    const [hasFilters, setHasFilters] = useState<boolean>(false);
    const [memorizedFilterModel, setMemorizedFilterModel] = useState<HeroFilterModel>(new HeroFilterModel());

    useEffect(() => {
        axios.get<Hero[]>(heroListPath)
            .then(response => updateList(response.data))
    }, []);

    function getListOfElements(heroes: Hero[]): JSX.Element[] {
        let result = heroes.map(h =>
            (<HeroInList hero={h} callBackFunction={deleteHeroFromList} 
                         isAddButton={false} 
                         isEmpty={false} 
                         hasFilters={hasFilters} key={h.id}/>)
        );
        if (hasFilters)
            result.push(<HeroInList hero={null} callBackFunction={deleteHeroFromList} 
                                    isAddButton={true} 
                                    hasFilters={hasFilters} 
                                    isEmpty={false}/>);
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
                    <HeroInList hero={null} callBackFunction={deleteHeroFromList} 
                                isAddButton={false} isEmpty={true} hasFilters={hasFilters}
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
            updateList(response);
        });

        setHasFilters(noFilterApplied(filterOptions));
    }

    return (
        <div>
            <FilterPanel callBackFunction={(heroFilterModel) => {
                setMemorizedFilterModel(heroFilterModel);
                applyFilters(heroFilterModel);
            }}/>
            <div className="d-flex justify-content-center">
                <hr/>    
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