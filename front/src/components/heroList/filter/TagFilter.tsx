import React, {useEffect, useState} from 'react';
import Select from "react-select";
import {ICallBack} from "../../interfaces/ICallBack";
import {Tag} from "../../../models/Tag";
import {getTags} from "../../../api/heroApi";
import "./FilterStyles.scss";
import {useDispatch, useSelector} from "react-redux";
import {updateHeroTagsFilter} from "../../../store/actionCreators/heroFilter";
import {IRootState} from "../../../store/store";

class TagElements {
    tag: Tag;
    element: JSX.Element;
}

let allTags: { value: string, label: JSX.Element }[] = [];

const TagFilter = () => {
    const heroTagFilter = useSelector<IRootState, string[]>(state => state.heroFilter.tags.map(x => x.name));
    const dispatch = useDispatch();

    useEffect(() => {
        getTags().then(data => allTags = data.map(t => {
            return {value: t.name, label: <div>{t.name}</div>}
        }));
    }, []);

    return (
        allTags.length > 0
            ?
            <div>
                <Select className="tags"
                        placeholder="tags..."
                        options={allTags}
                        isMulti={true}
                        value={allTags.filter(x =>heroTagFilter.includes(x.value))}
                        onChange={event => {
                            dispatch(updateHeroTagsFilter(event.map(x => {
                                return {name: x.value}
                            })));
                        }}
                />
            </div>
            :
            <div></div>
    );
};

export default TagFilter;