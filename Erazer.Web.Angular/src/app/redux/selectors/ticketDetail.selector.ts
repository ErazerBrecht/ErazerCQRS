import { createSelector, createFeatureSelector, State } from '@ngrx/store';
import { DataState } from '../state/dataState';
import { TicketDetail } from '../../entities/read/ticketDetail';

const selectData = createFeatureSelector<DataState>('data');
const selectTicketDetails = createSelector(selectData, (state: DataState) => state.ticketDetails);

export const getTicketDetail = id => createSelector(selectTicketDetails, (tickets) => {
    return tickets.find(t => t.id === id);
});