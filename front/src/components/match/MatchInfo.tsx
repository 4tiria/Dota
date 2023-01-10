import React, {useEffect, useState} from 'react';
import {Match} from "../../models/Match";
import {getMatch} from "../../api/matchApi";

const MatchInfo = () => {

    const [match, setMatch] = useState<Match>();
    useEffect(() => {
        getMatch('0x802596B8708FC849817D36E8349866C8').then(x => setMatch(x));
    }, [])


    return (
        <div>
            Тут будет матч
        </div>
    );
};

export default MatchInfo;