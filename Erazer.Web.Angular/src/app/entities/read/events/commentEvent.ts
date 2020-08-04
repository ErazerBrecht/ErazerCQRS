import { IEvent } from "../interfaces/iEvent";
import { EventType } from "../../../configuration/eventTypeConstants";
import { Priority } from "../priority";

export class CommentEvent implements IEvent {
    id: string;
    created: Date;
    userName: string;
    type: EventType;
    comment: string;
}