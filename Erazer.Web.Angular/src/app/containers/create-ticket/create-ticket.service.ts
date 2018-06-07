import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";

import { CreateTicket } from '../../entities/write/createTicket';
import { WRITE_API } from "../../configuration/config";
import { Headers, RequestOptions } from '@angular/http';


@Injectable()
export class CreateTicketService {

  constructor(private http: HttpClient) { }

  add(ticket: CreateTicket): Observable<string> {
    const formData = new FormData();
    Object.keys(ticket).filter(k => k !== CreateTicket.GetPropertyName('files')).forEach(key => formData.append(key, ticket[key]));

    for (let i = 0; i < ticket.files.length; i++)
      formData.append("files", ticket.files[i], ticket.files[i].name);

    return this.http.post<string>(`${WRITE_API}/ticket`, formData);
  }
}
