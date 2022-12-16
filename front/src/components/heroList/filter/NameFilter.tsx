import React, {useState} from 'react';
import {ICallBack} from "../../interfaces/ICallBack";
import {Input} from "@mui/material";

interface INameFilter extends ICallBack<string>{
    
}

const NameFilter: React.FC<INameFilter> = ({callBackFunction}) => {
    const [inputName, setInputName] = useState('');
    
    return (
        <div className="name-filter">
           <Input 
               defaultValue=''
               onChange={(event) => {
                   let newValue = event.target.value;
                   if(newValue?.length > 0){
                       setInputName(newValue);
                       callBackFunction(newValue);
                   }
               }}
           /> 
        </div>
    );
};

export default NameFilter;