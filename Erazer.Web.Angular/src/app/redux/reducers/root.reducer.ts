import { ActionReducerMap } from '@ngrx/store';

import { appReducer } from "./app.reducer";

import { State } from "../state/state";
import { dataReducers } from './data.reducer';

export const rootReducer: ActionReducerMap<State> = {
    app: appReducer,
    data: dataReducers
};