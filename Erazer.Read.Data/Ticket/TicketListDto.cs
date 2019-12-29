﻿namespace Erazer.Read.Data.Ticket
{
    public class TicketListDto: IProjection

    {
    public string Id { get; set; }
    public string Title { get; set; }
    public int FileCount { get; set; }

    public PriorityDto Priority { get; set; }
    public StatusDto Status { get; set; }
    }
}