import React, {useState} from 'react';
import {BsPlusLg} from "react-icons/all";

const HeroAdd = () => {
    const [isHovering, setIsHovering] = useState(false);
    
    return (
        <div className="hero hero-default text-center d-flex justify-content-center align-items-center">
           <div><BsPlusLg className="plus"/></div>
        </div>
    );
};

export default HeroAdd;