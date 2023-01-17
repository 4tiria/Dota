import React, {useEffect, useRef, useState} from 'react';
import {Match} from "../../../models/Match";
import {getAllMatches, getFilteredMatches} from "../../../api/matchApi";
import {Hero} from "../../../models/Hero";
import MatchInList from "./matchInList/MatchInList";
import "./MatchList.scss";
import {useSelector} from "react-redux";
import {IRootState} from "../../../store/store";
import {MatchFilterModel} from "../../../models/filterModels/matchFilterModel";
import FilterPanel from "./filters/FilterPanel";


const MatchList = () => {
    const [isLastData, setIsLastData] = useState<boolean>(false);
    const [matchList, setMatchList] = useState<Match[]>([]);
    const lastElement = useRef();
    const observer = useRef<IntersectionObserver>()
    const filters = useSelector<IRootState, MatchFilterModel>(state => state.matchFilter);
    //todo: add reset button
    //todo: so I will need useDispatch

    useEffect(() => {
        setIsLastData(false);
        setMatchList([]);
    }, [filters])

    useEffect(() => {
        if (!lastElement.current) return;
        if (observer.current) observer.current.disconnect();
        observer.current = new IntersectionObserver(
            (entries, observer) => {
                if (entries[0].isIntersecting)
                    downloadNewBatchOfMatches();
            }, {});
        observer.current.observe(lastElement.current);
    }, [matchList]);


    function downloadNewBatchOfMatches() {
        filters.skip = matchList.length;
        filters.take = 20; //not necessary to give user control of this value
        if (isLastData) return;

        getFilteredMatches(filters).then(data => {
            if (data.length == 0) {
                setIsLastData(true);
            } else {
                setMatchList(prevState => {
                    return [...prevState, ...data];
                });
            }
        });
    }

    return (
        <div className="d-flex justify-content-center my-2">
            <FilterPanel/>
            <div>
                {matchList.map(m =>
                    <div key={m.id} className="m-2">
                        <MatchInList match={m}/>
                    </div>
                )}
                <div ref={lastElement} className="last-element"></div>
            </div>
        </div>
    );
};

export default MatchList;