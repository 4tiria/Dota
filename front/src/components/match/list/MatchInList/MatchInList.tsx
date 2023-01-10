import React from 'react';
import {Match} from "../../../../models/Match";
import {Hero} from "../../../../models/Hero";

interface IMatchInList {
    match: Match;
}

const MatchInList: React.FC<IMatchInList> = ({match}) => {

    function renderHero(hero: Hero) {
        return (<div>
            {hero.name}
        </div>);
    }

    return (
        match ?
            <div key={match.id}>
                <div>{match.id}</div>
                <div>{match.start.toString()}</div>
                <div>{match.end.toString()}</div>
                <div>{match.score}</div>
                <div>{match.heroes.map(hm => renderHero(hm.hero))}</div>
            </div>
            : <></>
    );
};

export default MatchInList;