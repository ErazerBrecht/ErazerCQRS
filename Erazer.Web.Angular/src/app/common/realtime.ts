import { Store } from '@ngrx/store';
import { Injectable } from '@angular/core';
import { State } from '../redux/state/state';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { WEBSOCKETS_API } from '../configuration/config';

@Injectable()
export class RealTime {
    private _hubConnection: HubConnection;

    constructor(private store: Store<State>) {
        this._hubConnection = new HubConnectionBuilder()
            .withUrl(`${WEBSOCKETS_API}/events`)
            .configureLogging(LogLevel.Information)
            .build();
    }

    connect(): void {
        this._hubConnection.on('SendAction', (data: string) => {
            const action = JSON.parse(data);
            this.store.dispatch(action);
        });

        this._hubConnection.onclose((error: Error) => {
            console.log('WebSocket connection was closed');

            if (error) {
                console.log(error.message);
            } else {
                console.log('Unkown reason');
            }

            console.log('Reconnect');
            this.connect();
        });

        this._hubConnection.start()
            .then(() => {
                console.log('Hub connection started');
            })
            .catch(err => {
                console.log('Error while establishing connection');
                setTimeout(() => { this.connect(); }, 5000);
            });
    }

    // TODO DISCONNECT
}