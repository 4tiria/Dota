import React, {useEffect, useState} from 'react';
import {Match} from "../../../../models/Match";
import {Hero} from "../../../../models/Hero";
import "./MatchInList.scss";
import HeroSmallImage from "./heroSmallImage/HeroSmallImage";
import {commonIcons} from "../../../../assets/CommonIcons";
import {BlobWithPath, heroImages} from "../../../../assets/HeroImages";
import {getFullDateString, getTimeString} from "../../../../helpers/dateHelpers";

interface IMatchInList {
    match: Match;
}

const MatchInList: React.FC<IMatchInList> = ({match}) => {
    const [radianKillCount, setRadiantKillCount] = useState<number>(0);
    const [direKillCount, setDireKillCount] = useState<number>(0);
    const [duration, setDuration] = useState<string>('');
    const [start, setStart] = useState<string>('');

    useEffect(() => {
        if (!match) return;
        
        let split = match.score.split('-');
        setRadiantKillCount(parseInt(split[0]));
        setDireKillCount(parseInt(split[1]));
        setStart(getTimeString(new Date(match.start)));
        let difference = match.end - match.start;
        let duration = new Date(difference);
        duration.setUTCHours(-4);
        let seconds = duration.getSeconds().toString();
        setDuration(`${duration.getHours() * 60 + duration.getMinutes()}:${seconds.length == 1 ? '0' : ''}${seconds}`);
    }, [commonIcons]);

    function renderSide(side: string) {
        return (
            <div
                className="d-flex">
                {match.heroes.filter(hm => hm.side.toLowerCase() == side.toLowerCase()).map(hm => {
                    return (
                        <div key={hm.heroId}>
                            <HeroSmallImage
                                hero={hm.hero}
                                isRadiant={hm.side.toLowerCase() == 'radiant'}/>
                        </div>)
                })}
            </div>
        )
    }

    return (
        match ?
            <div>
                <div className="match-info d-flex justify-content-between">
                    <div className="win win-radiant">{match.winnerSide == 'Radiant'? 'Radiant Win' : ''}</div>
                    <div className="start-time">{start}</div>
                    <div className="win win-dire">{match.winnerSide == 'Dire'? 'Dire win' : ''}</div>
                </div>
                <div key={match.id} className="match-in-list-container">
                    <div className="side side-radiant">
                        {renderSide('radiant')}
                    </div>
                    <div className="d-flex justify-content-center mx-2 align-items-center score-and-time">
                        <div className="score">{radianKillCount}</div>
                        <div className="duration">{duration}</div>
                        <div className="score">{direKillCount}</div>
                    </div>
                    <div className="side side-dire">
                        {renderSide('dire')}
                    </div>
                </div>
            </div>
            : <></>
    );
};

export default MatchInList;