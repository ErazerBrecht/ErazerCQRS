import { combineReducers, ActionReducer } from "@ngrx/store";
import { DataState } from "../state/dataState";
import { ticketsReducer } from "./ticket.reducer";
import { ticketDetailsReducer } from "./ticketDetail.reducer";

export const dataReducers: ActionReducer<DataState> = combineReducers({
    tickets: ticketsReducer,
    ticketDetails: ticketDetailsReducer
});