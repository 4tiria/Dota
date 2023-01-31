import React, {useEffect, useState} from 'react';
import {Tag} from "../../../models/Tag";
import {getTags} from "../../../api/heroApi";
import "./FilterStyles.scss";
import {useDispatch, useSelector} from "react-redux";
import {updateHeroTagsFilter} from "../../../store/actionCreators/heroFilter";
import {IRootState} from "../../../store/store";
import {Box, FormControl, InputLabel, MenuItem, Select} from "@mui/material";
import {generateUniqueID} from "web-vitals/dist/modules/lib/generateUniqueID";

class TagElements {
    tag: Tag;
    element: JSX.Element;
}

let allTags: { value: string, label: JSX.Element }[] = [];

const TagFilter = () => {
    const heroTagFilter = useSelector<IRootState, string[]>(state => state.heroFilter.tags.map(x => x.name));
    const dispatch = useDispatch();

    const [value, setValue] = useState<string[]>([]);

    useEffect(() => {
        getTags().then(data => allTags = data.map(t => {
            return {value: t.name, label: <div key={t.name}>{t.name}</div>}
        }));
    }, []);

    useEffect(() => {
        dispatch(updateHeroTagsFilter(value.map(x => {
            return {name: x}
        })));
    }, [value]);

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
                        value={allTags.filter(x => heroTagFilter.includes(x.value)).map(x => x.value)}
                        label="Tags"
                        onChange={event => {
                            setValue(event.target.value as string[]);
                        }}
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