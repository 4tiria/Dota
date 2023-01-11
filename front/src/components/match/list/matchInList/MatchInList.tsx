import React, {useEffect, useState} from 'react';
import {Match} from "../../../../models/Match";
import {Hero} from "../../../../models/Hero";
import "./MatchInList.scss";
import HeroSmallImage from "./heroSmallImage/HeroSmallImage";

interface IMatchInList {
    match: Match;
}

const MatchInList: React.FC<IMatchInList> = ({match}) => {
    const [radianKillCount, setRadiantKillCount] = useState<number>(0);
    const [direKillCount, setDireKillCount] = useState<number>(0);
    const [duration, setDuration] = useState<string>('');
    
    useEffect(() => {
        let split = match.score.split('-');
        setRadiantKillCount(parseInt(split[0]));
        setDireKillCount(parseInt(split[1]));
        debugger
        let difference = match.end.getTime() - match.start.getTime();
        setDuration(new Date(difference).toTimeString());
    }, []);
    
    function renderSide(side: string) {
        return (
            <div
                className="d-flex justify-content-end">
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
            <div key={match.id} className="match-in-list-container">
                <div className="side side-radiant">
                    {renderSide('radiant')}
                </div>
                <div className="d-flex justify-content-center">
                    <div className="score-font">{radianKillCount}</div>
                    <div className="duration-font">{duration}</div>
                    <div className="score-font">{direKillCount}</div>
                </div>
                <div className="side side-dire">
                    {renderSide('dire')}
                </div>
            </div>
            : <></>
    );
};

export default MatchInList;