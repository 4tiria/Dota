import {EventHandler, SyntheticEvent} from "react";

export interface IReset<T>{
    onReset123: EventHandler<SyntheticEvent<T>>;
}