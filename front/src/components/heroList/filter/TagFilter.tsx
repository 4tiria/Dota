import React, {useEffect, useState} from 'react';
import "./FilterStyles.scss";
import {useDispatch, useSelector} from "react-redux";
import {updateHeroTagsFilter} from "../../../store/actionCreators/heroFilter";
import {IRootState} from "../../../store/store";
import {Box, FormControl, InputLabel, MenuItem, Select} from "@mui/material";
import {generateUniqueID} from "web-vitals/dist/modules/lib/generateUniqueID";
import { Tag } from '../../../models/Tag';
import { getTagFromString } from '../../../helpers/enumHelper';


let allTags: { value: string, label: JSX.Element }[] = [];

const TagFilter = () => {
    const heroTagFilter = useSelector<IRootState, Tag[]>(state => state.heroFilter.tags);
    const dispatch = useDispatch();

    const [tags, setTags] = useState<Tag[]>([]);

    useEffect(() => {
        setTags(Object.values(Tag).filter(value => typeof value === 'number') as Tag[]);
    }, []);

    useEffect(() => {
        dispatch(updateHeroTagsFilter(tags));
    }, [tags]);

    return (
        allTags.length > 0
            ?
            <Box className="box-margin">
                <FormControl fullWidth color="secondary">
                    <InputLabel id="label-tags">Tags</InputLabel>
                    <Select
                        labelId="label-tags"
                        id="select-tags"
                        className="tags"
                        multiple={true}
                        renderValue={(selected) => selected.join(", ")}
                        value={allTags.filter(x => heroTagFilter.includes(getTagFromString(x.value))).map(x => x.value)}
                        label="Tags"
                        onChange={event => setTags([...event.target.value].map(getTagFromString))}
                    >
                        {allTags.map(x =>
                            <MenuItem
                                value={x.value}
                                key={generateUniqueID()}
                            >
                                {x.label}
                            </MenuItem>
                        )}
                    </Select>
                </FormControl>
            </Box>


            :
            <div></div>
    );
};

export default TagFilter;