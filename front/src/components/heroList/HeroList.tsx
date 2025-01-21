import { Paper } from "@mui/material";
import React, { useState } from 'react';
import { generateUniqueID } from "web-vitals/dist/modules/lib/generateUniqueID";
import { getFilteredList } from "../../api/heroApi";
import { HeroFilterModel } from "../../models/filterModels/heroFilter";
import { Hero } from "../../models/Hero";
import FilterPanel from "./filter/FilterPanel";
import HeroInList from "./HeroInList";

const HeroList = () => {
    const [list, setList] = useState<Hero[]>([]);

    function getListOfElements(heroes: Hero[]): JSX.Element[] {
        let result = heroes.map(h =>
            (<HeroInList
                hero={h}
                key={h.id} />)
        );
    
        return result;
    }

    function splitListFor(columns: number, elements: JSX.Element[]) {
        let result: JSX.Element[][] = [];
        let rowNumber = -1;
        for (let i = 0; i < elements.length; i++) {
            if (i % columns === 0) {
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

    function renderRow(columns: number, array: JSX.Element[]) {
        let difference = columns - array.length;
        if (difference > 0) {
            for (let i = 0; i < difference; i++) {
                array.push(
                    <HeroInList
                        hero={null}
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
    }

    return (
        <Paper sx={{paddingBottom: 20}} variant="elevation" square={true}>
            <FilterPanel callBackFunction={(heroFilterModel) => {
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
        </Paper>

    );
}

export default HeroList;