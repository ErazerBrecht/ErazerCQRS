export class CreateTicket {
    title: string;
    description: string;
    priorityId: string;
    files?: Array<File>

    /**
     * Initialize a 'createTicket' entity.
     * This entity is used when an new ticket needs to be submitted to the backend.
     */
    constructor(title: string, description: string, priorityId: string, files: Array<File>) {
        this.title = title;
        this.description = description;
        this.priorityId = priorityId;     
        this.files = files;
    }

    private static propertyNamesOf = <TObj>() => (name: keyof TObj) => name;
    public static  GetPropertyName = CreateTicket.propertyNamesOf<CreateTicket>();
}