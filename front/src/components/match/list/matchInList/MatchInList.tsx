import React from 'react';
import {Match} from "../../../../models/Match";
import {Hero} from "../../../../models/Hero";
import "./MatchInList.scss";
import HeroSmallImage from "./heroSmallImage/HeroSmallImage";

interface IMatchInList {
    match: Match;
}

const MatchInList: React.FC<IMatchInList> = ({match}) => {

    function renderSide(side: string){
        return (
            <div
                className="d-flex justify-content-end">
                {match.heroes.filter(hm => hm.side.toLowerCase() == side.toLowerCase()).map(hm => {
                    return <HeroSmallImage hero={hm.hero}/>
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
                <div>
                    <div>{match.score}</div>
                </div>
                <div className="side side-dire">
                    {renderSide('dire')}
                </div>
            </div>
            : <></>
    );
};

export default MatchInList;