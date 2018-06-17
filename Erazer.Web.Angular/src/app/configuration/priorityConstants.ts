import {IOption} from "./ioption";

const Lowest = class {
   public static readonly id: string = "50c8608e-8de0-4e6d-bd89-21efe29afa81";
   public static readonly title: string = "Lowest";
   public static readonly description: string = "TODO1";
}

const Low = class {
   public static readonly id: string = "646be77c-bc46-429f-a3ab-ae19516dcb6a";
   public static readonly title: string = "Low";
   public static readonly description: string = "TODO2";
}

const Medium = class {
   public static readonly id: string = "7d4b4b49-323b-45e8-9f1a-23ce7592f9f3";
   public static readonly title: string = "Medium";
   public static readonly description: string = "TODO3";
}

const High = class {
   public static readonly id: string = "84f5d2fe-ce2a-452a-9a1b-4f6610742602";
   public static readonly title: string = "High";
   public static readonly description: string = "TODO4";
}

const Highest = class {
   public static readonly id: string = "974e6dc2-aa1e-4a20-94dd-9be227987c51";
   public static readonly title: string = "Highest";
   public static readonly description: string = "TODO5";
}

class PriorityOption implements IOption {
    id: string;
    title: string;
    description: string;
}

export class PriorityValues {
    public static readonly lowest: PriorityOption = Lowest;
    public static readonly low: PriorityOption = Low;
    public static readonly medium: PriorityOption = Medium;
    public static readonly high: PriorityOption = High;
    public static readonly highest: PriorityOption = Highest;
}



