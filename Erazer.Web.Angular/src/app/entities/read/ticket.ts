import { Priority } from "./priority";
import { Status } from "./status";

export class Ticket {
    id: string;
    title: string;
    priority: Priority;
    status: Status;
    // eventCount: number;
    // lastUpdate: Date;
}