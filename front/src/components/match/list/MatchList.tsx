import React, {useContext, useEffect, useRef, useState} from 'react';
import "./MatchList.scss";
import {useDispatch, useSelector} from "react-redux";
import {IRootState} from "../../../store/store";
import {MatchListFilterModel} from "../../../models/filterModels/matchListFilterModel";
import FilterPanel from "./filters/FilterPanel";
import MatchListPerDay from "./matchListPerDay/MatchListPerDay";
import Button from "@mui/material/Button";

const MatchList = () => {

    const [lastDayAgo, setLastDayAgo] = useState<number>(null);
    const [dayArray, setDayArray] = useState<number[]>([]);
    const lastElement = useRef();
    const observer = useRef<IntersectionObserver>();

    const filters = useSelector<IRootState, MatchListFilterModel>(state => state.matchFilter);

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
        let minLastDayAgo = filters.maxStartedMillisecondsBefore
            ? Math.floor(filters.maxStartedMillisecondsBefore / (1000 * 60 * 60 * 24))
            : 0;
        setLastDayAgo(prevState => prevState === null ? minLastDayAgo : prevState + 1);
    }

    useEffect(() => {
        if (lastDayAgo != null) {
            setDayArray(prevState => [...prevState, lastDayAgo]);
        }
    }, [lastDayAgo]);

    useEffect(() => {
        if (lastDayAgo != null) {
            setDayArray([]);
            setLastDayAgo(null);
        }

    }, [
        filters.minStartedMillisecondsBefore,
        filters.maxStartedMillisecondsBefore, 
        filters.minDurationInMinutes,
        filters.maxDurationInMinutes,
        filters.skip,
        filters.take,
        filters.selfTeam,
        filters.otherTeam,
    ]);

    return (
        <div
            className="d-flex justify-content-center my-2"
        >
            <FilterPanel/>
            <div className="list">
                <div>
                    {dayArray.map(x => {
                        return (
                            <div key={x}>
                                <MatchListPerDay
                                    daysAgo={x}
                                    callBackFunction={updateObserver}
                                />
                            </div>
                        );
                    })}
                </div>
                <div
                    ref={lastElement}
                    className="last-element d-flex justify-content-center">
                    <Button
                        sx={{
                            width: 500,
                            height: 60,
                            color: 'grey'
                        }}
                        variant="text"
                        onClick={_ => downloadMatchesInDay()}
                    >
                        Load more
                    </Button>
                </div>
            </div>
        </div>
    );
};

export default MatchList;