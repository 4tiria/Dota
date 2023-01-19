import React, {useEffect, useRef, useState} from 'react';
import MatchInList from "../matchInList/MatchInList";
import {Match} from "../../../../models/Match";
import {getFilteredMatches} from "../../../../api/matchApi";
import {useSelector} from "react-redux";
import {IRootState} from "../../../../store/store";
import {MatchFilterModel} from "../../../../models/filterModels/matchFilterModel";
import {ICallBack} from "../../../interfaces/ICallBack";

interface IMatchListPerDay extends ICallBack<any> {
    day: number;
}

const MatchListPerDay: React.FC<IMatchListPerDay> = ({day, callBackFunction}) => {

    const filters = useSelector<IRootState, MatchFilterModel>(state => state.matchFilter);
    const [matches, setMatches] = useState<Match[]>([]);
    const [isLoaded, setIsLoaded] = useState<boolean>(false);

    useEffect(() => {
        filters.daysAgo = day;
        getFilteredMatches(filters).then(data => {
            setMatches(data);
        });
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
            <div>{day}, {matches.length}</div>
            <hr/>
            <div>{matches.map(m =>
            {
                return (<div key={m.id} className="m-2">
                    <MatchInList match={m}/>
                </div>);
            }
                
            )}</div>
        </div>
    );
};

export default MatchListPerDay;