import { IEvent } from "../interfaces/iEvent";
import { EventType } from "../../../configuration/eventTypeConstants";
import { Priority } from "../priority";

export class PriorityEvent implements IEvent{
    id: string;
    ticketId: string;
    created: Date;
    userName: string;
    type: EventType;
    fromPriority: Priority;
    toPriority: Priority;
}