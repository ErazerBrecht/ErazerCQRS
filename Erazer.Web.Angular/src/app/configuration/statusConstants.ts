import {IOption} from "./ioption";

const Backlog = class {
   public static readonly id: string = "66670a9a-eeda-4f70-9428-05cb66731c60";
   public static readonly title: string = "Backlog";
}

const Open = class {
   public static readonly id: string = "7572d50b-77d4-4181-a01d-a97f457edd80";
   public static readonly title: string = "Open";
}

const InProgress = class {
   public static readonly id: string = "8abb2921-3c51-46ab-bd94-91923e81cf6f";
   public static readonly title: string = "In Progress";
}

const Done = class {
   public static readonly id: string = "a3ba43de-aeda-4432-9fa9-523faa303223";
   public static readonly title: string = "Done";
}

const Closed = class {
   public static readonly id: string = "b773d928-d01f-4b2d-b984-0abb0598f4d4";
   public static readonly title: string = "Closed";
}

export class StatusValues {
    public static readonly backlog: IOption = Backlog;    
    public static readonly open: IOption = Open;
    public static readonly inProgress: IOption = InProgress;
    public static readonly done: IOption = Done;
    public static readonly closed: IOption = Closed;
}



