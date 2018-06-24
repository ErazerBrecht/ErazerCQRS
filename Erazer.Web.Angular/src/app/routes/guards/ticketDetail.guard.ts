import { Injectable } from '@angular/core';
import { CanActivate, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Store } from '@ngrx/store';

import { State } from "../../redux/state/state";
import * as TicketDetailSelectors from "../../redux/selectors/ticketDetail.selector";
import { AddTicketDetails } from '../../redux/actions/ticketDetail';
import { DetailTicketService } from '../../containers/detail-ticket/detail-ticket.service';

import { Observable } from 'rxjs';
import { map, switchMap, filter, take } from 'rxjs/operators';

@Injectable()
export class TicketDetailGuard implements CanActivate {
    constructor(private store: Store<State>, private service: DetailTicketService) { }

    // our guard that gets called each time we 
    // navigate to a new route
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
        const id: string = route.params['id'];

        const isLoaded$ = this.store
            .select(TicketDetailSelectors.getTicketDetail(id))
            .pipe(
                map(t => !!t)
            );

        // TODO ERRORHANDLING
        isLoaded$.pipe(
            filter(x => x === false),
            switchMap(() => this.service.get(id)),
            take(1)
        ).subscribe(x => this.store.dispatch(new AddTicketDetails(x)));

        const activate$ = isLoaded$.pipe(
            filter(x => x === true),
        );

        return activate$;
    }
}