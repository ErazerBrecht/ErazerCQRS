import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { READ_API } from "../../configuration/config";
import { Ticket } from "../../entities/read/ticket";

@Injectable()
export class AllTicketsService {
    constructor(private http: HttpClient) { }

    all(): Observable<Array<Ticket>> {
        return this.http.get<Ticket[]>(`${READ_API}/ticket`);
    }
}