import React, {useEffect, useState} from 'react';
import MatchInList from "../matchInList/MatchInList";
import {Match} from "../../../../models/Match";
import {getFilteredMatches} from "../../../../api/matchApi";
import {useSelector} from "react-redux";
import {IRootState} from "../../../../store/store";
import {MatchListFilterModel} from "../../../../models/filterModels/matchListFilterModel";
import {ICallBack} from "../../../interfaces/ICallBack";
import "./MatchListPerDay.scss";

interface IMatchListPerDay extends ICallBack<any> {
    daysAgo: number;
}

function getDaysAgoString(day: number): string {
    if (day === 0) return "today";
    if (day === 1) return "yesterday";
    return `${day} days ago`;
}


function getDateString(daysAgo: number) : string {
    let date = new Date();
    date.setUTCDate(date.getUTCDate() - daysAgo);
    let month = new Intl.DateTimeFormat('en', {month: 'long'}).format(date);
    let day = new Intl.DateTimeFormat('en', {day: '2-digit'}).format(date);
    return `${month}, ${day}`;
}


const MatchListPerDay: React.FC<IMatchListPerDay> = ({daysAgo, callBackFunction}) => {
    const filters = useSelector<IRootState, MatchListFilterModel>(state => state.matchFilter);
    const [matches, setMatches] = useState<Match[]>([]);
    const [isLoaded, setIsLoaded] = useState<boolean>(false);
    const [date, setDate] = useState<string>('');
    const [daysAgoString, setDaysAgoString] = useState<string>('');

    useEffect(() => {
        getFilteredMatches(
            {
                daysAgo: daysAgo,
                minDurationInMinutes: filters.minDurationInMinutes,
                maxDurationInMinutes: filters.maxDurationInMinutes,
                minStartedMillisecondsBefore: filters.minStartedMillisecondsBefore,
                maxStartedMillisecondsBefore: filters.maxStartedMillisecondsBefore,
                take: filters.take,
                skip: filters.skip,
                selfTeam: filters.selfTeam,
                otherTeam: filters.otherTeam
            }).then(data => {
            setMatches(data);
        });

        setDate(getDateString(daysAgo));
        setDaysAgoString(getDaysAgoString(daysAgo));
    }, []);

    useEffect(() => {
        if (isLoaded)
            callBackFunction(null);
    }, [isLoaded])

    useEffect(() => {
        if (matches.length > 0 && !isLoaded)
            setIsLoaded(true);
    }, [matches]);

    return (
        <div>
            <div className="d-flex justify-content-between align-items-end">
                <div className="date">{date} ({daysAgoString})</div>
                <div className="count">{matches.length} matches found</div>
            </div>

            <hr/>
            <div>{matches.map(m => {
                    return (
                        <div key={m.id} className="m-2">
                            <MatchInList match={m}/>
                        </div>);
                }
            )}</div>
        </div>
    );
};

export default MatchListPerDay;