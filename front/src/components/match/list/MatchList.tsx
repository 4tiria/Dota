import React, {useEffect, useState} from 'react';
import {Match} from "../../../models/Match";
import {getAllMatches} from "../../../api/matchApi";
import {Hero} from "../../../models/Hero";
import MatchInList from "./matchInList/MatchInList";

const MatchList = () => {
    const [matchList, setMatchList] = useState<Match[]>([]);
    useEffect(() => {
        getAllMatches().then(x => {
            setMatchList(x)
        });
    }, []);

    return (
        matchList.length == 0
            ? <></>
            :
            <div className="d-flex justify-content-center my-2">
                {matchList.map(m =>
                    <div key={m.id}>
                        <MatchInList match={m}/>
                    </div>
                )}
            </div>
    );
};

export default MatchList;