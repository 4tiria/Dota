import React, {useEffect, useRef, useState} from 'react';
import "./MatchList.scss";
import {useDispatch, useSelector} from "react-redux";
import {IRootState} from "../../../store/store";
import {MatchFilterModel} from "../../../models/filterModels/matchFilterModel";
import FilterPanel from "./filters/FilterPanel";
import MatchListPerDay from "./matchListPerDay/MatchListPerDay";

const MatchList = () => {
    const [lastDayAgo, setLastDayAgo] = useState<number>(0);
    const [dayArray, setDayArray] = useState<number[]>([]);
    const [message, setMessage] = useState<string>("");
    const lastElement = useRef();
    const observer = useRef<IntersectionObserver>();

    const filters = useSelector<IRootState, MatchFilterModel>(state => state.matchFilter);

    useEffect(() => {
        setDayArray([]);
        setMessage(prevState => prevState + `ъ`);
    }, [filters]);

    function updateObserver() {
        if (!lastElement.current) return;
        if (observer.current) observer.current.disconnect();
        observer.current = new IntersectionObserver(
            (entries, observer) => {
                if (entries[0].isIntersecting) {
                    downloadMatchesInDay();
                }
            }, {});
        observer.current.observe(lastElement.current);
    }

    function downloadMatchesInDay(): void {
        setLastDayAgo(lastDayAgo + 1);
    }

    useEffect(() => {
        if (dayArray?.length == 0){
            setMessage(prevState => prevState + `/`);
            setLastDayAgo(0);
        }
           
    }, [dayArray, filters]);

    useEffect(() => {
        if (lastDayAgo == 0) {
            setLastDayAgo(1);
            return;
        }

        setDayArray(prevState => [...prevState, lastDayAgo]);
    }, [lastDayAgo]);


    return (
        <div className="d-flex justify-content-center my-2">
            <FilterPanel/>
            <div className="list">
                <div>{JSON.stringify(dayArray)}</div>
                <div>{message}</div>
                <div>{JSON.stringify(lastDayAgo)}</div>
                <div>
                    {dayArray.map(x => {
                        return (
                            <div key={x}>
                                <MatchListPerDay day={x} callBackFunction={updateObserver}/>
                            </div>
                        );
                    })}
                </div>
                <div ref={lastElement} className="last-element"></div>
            </div>
        </div>
    );
};

export default MatchList;