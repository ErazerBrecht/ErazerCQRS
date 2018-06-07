import { IEvent } from "../interfaces/iEvent";
import { EventType } from "../../../configuration/eventTypeConstants";
import { Status } from "../status";

export class StatusEvent implements IEvent{
    id: string;
    ticketId: string;
    created: Date;
    userName: string;
    type: EventType;
    fromStatus: Status;
    toStatus: Status;
}