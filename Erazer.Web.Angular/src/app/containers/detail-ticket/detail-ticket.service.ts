import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { READ_API, WRITE_API } from "../../configuration/config";
import { TicketDetail } from "../../entities/read/ticketDetail";

@Injectable()
export class DetailTicketService {
    constructor(private http: HttpClient) { }

    get (id: string): Observable<TicketDetail> {
        return this.http.get<TicketDetail>(`${READ_API}/ticket/${id}`);
    }

    updatePriority (ticketId: string, priorityId: string){
        return this.http.patch(`${WRITE_API}/ticket/priority`, { ticketId, priorityId });
    }

    updateStatus (ticketId: string, statusId: string){
        return this.http.patch(`${WRITE_API}/ticket/status`, { ticketId, statusId });
    }

    addComment (ticketId: string, comment: string){
        return this.http.patch(`${WRITE_API}/ticket/comment`, { ticketId, comment });
    }
}