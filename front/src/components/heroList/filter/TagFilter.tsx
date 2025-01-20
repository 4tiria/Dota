import { Box, FormControl, InputLabel, MenuItem, Select } from "@mui/material";
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from "react-redux";
import { generateUniqueID } from "web-vitals/dist/modules/lib/generateUniqueID";
import { getTagFromString } from '../../../helpers/enumHelper';
import { Role } from '../../../models/Role';
import { updateHeroTagsFilter } from "../../../store/actionCreators/heroFilter";
import { IRootState } from "../../../store/store";
import "./FilterStyles.scss";


let allTags: { value: string, label: JSX.Element }[] = [];

const TagFilter = () => {
    const heroTagFilter = useSelector<IRootState, Role[]>(state => state.heroFilter.tags);
    const dispatch = useDispatch();

    const [tags, setTags] = useState<Role[]>([]);

    useEffect(() => {
        setTags(Object.values(Role).filter(value => typeof value === 'number') as Role[]);
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