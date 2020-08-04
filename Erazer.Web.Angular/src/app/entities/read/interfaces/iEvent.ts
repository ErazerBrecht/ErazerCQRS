import { EventType } from '../../../configuration/eventTypeConstants';

export interface IEvent
{
    id: string;
    created: Date
    userName: string;
    type: EventType;
}