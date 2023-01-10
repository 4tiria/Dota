import React, {useEffect, useState} from 'react';
import {Match} from "../../../models/Match";
import {getAllMatches} from "../../../api/matchApi";
import {Hero} from "../../../models/Hero";
import MatchInList from "./MatchInList/MatchInList";

const MatchList = () => {
    const [matchList, setMatchList] = useState<Match[]>([]);
    useEffect(() => {
        getAllMatches().then(x => setMatchList(x));
    }, []);

    return (
        matchList.length == 0
            ? <></>
            :
            <div>
                {matchList.map(m => <MatchInList match={m}/>)}
            </div>
    );
};

export default MatchList;