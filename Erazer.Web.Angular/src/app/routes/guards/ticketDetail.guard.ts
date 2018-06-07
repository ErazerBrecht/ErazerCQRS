import { Injectable } from '@angular/core';
import { CanActivate, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Store } from '@ngrx/store';

import { State } from "../../redux/state/state";
import * as TicketDetailSelectors from "../../redux/selectors/ticketDetail.selector";
import { AddTicketDetails } from '../../redux/actions/ticketDetail';
import { TicketDetail } from '../../entities/read/ticketDetail';
import { DetailTicketService } from '../../containers/detail-ticket/detail-ticket.service';

import { Observable, pipe, of } from 'rxjs';
import { take, switchMap, catchError, filter, tap } from 'rxjs/operators';

@Injectable()
export class TicketDetailGuard implements CanActivate {
    constructor(private store: Store<State>, private service: DetailTicketService) { }

    // wrapping the logic so we can .switchMap() it
    getFromStoreOrAPI(id: string): Observable<any> {

        // return an Observable stream from the store
        return this.store
            // selecting the ticket state using a feature selector
            .select(TicketDetailSelectors.getTicketDetail(id))
            // the .tap() operator allows for a side effect, at this
            // point, I'm checking if the ticket property exists on my 
            // Store slice of state
            .pipe(
                tap((data: TicketDetail) => {
                    // if the ticket is not in the store add it trough the backend
                    if (!data) {
                        this.service.get(id).pipe(take(1))
                            .subscribe(ticket => this.store.dispatch(new AddTicketDetails(ticket)));
                    }
                }),
                filter(data => data !== undefined),
                take(1)
            );
    }

    // our guard that gets called each time we 
    // navigate to a new route
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
        const id: string = route.params['id'];
        // return our Observable stream from above
        return this.getFromStoreOrAPI(id)
            .pipe(
                // if it was successful, we can return Observable.of(true)
                switchMap(() => of(true)),
                // otherwise, something went wrong
                catchError(() => of(false))
            );
    }
}