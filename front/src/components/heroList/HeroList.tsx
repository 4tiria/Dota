import React, {useEffect, useState} from 'react';
import {Hero} from "../../models/Hero";
import axios from "axios";
import {forEach} from "react-bootstrap/ElementChildren";
import HeroInList from "./HeroInList";
import {heroListPath} from "../../api/apiPaths";

const HeroList = (props) => {
    const [list, updateList] = useState([]);
    useEffect(() => {
        axios.get<Hero[]>(heroListPath)
            .then(response => updateList(response.data))
    }, []);
    
    return (
        <div className="hero-list">
            <div>
                {list.map(hero =>
                    <HeroInList hero={hero} key={hero.id}/>)}
            </div>
        </div>
    );
}

export default HeroList;