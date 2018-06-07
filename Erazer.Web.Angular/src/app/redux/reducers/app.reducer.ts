import { AppState } from "../state/appState";
import * as sidebar from "../actions/sidebar";

let initialState: AppState = {
    isCollapsed: false
};

export function appReducer(state: AppState = initialState,
                                          action: sidebar.Actions): AppState {
    switch (action.type) {
        case sidebar.TOGGLE_SIDEBAR:
            return {
                isCollapsed: !state.isCollapsed
            };
        default:
            return state;
    }
};