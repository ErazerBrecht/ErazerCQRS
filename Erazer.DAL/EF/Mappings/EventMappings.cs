using AutoMapper;
using Erazer.DAL.EF.Entities;
using Erazer.Domain.Constants.Enums;
using Erazer.Domain.Events;

namespace Erazer.DAL.EF.Mappings
{
    public class EventMappings : Profile
    {
        public EventMappings()
        {
            CreateMap<TicketCommentEvent, TicketEventEntity>()
                .ForMember(e => e.TicketId, opt => opt.MapFrom(@event => @event.AggregateRootId))
                .ForMember(e => e.Created, opt => opt.MapFrom(@event => @event.Created))
                .ForMember(e => e.UserId, opt => opt.MapFrom(@event => @event.UserId))
                .ForMember(e => e.CommentEvent, opt => opt.MapFrom(@event => new TicketCommentEventEntity {Comment = @event.Comment}))
                .ForMember(e => e.Type, opt => opt.UseValue(EventType.Comment));

            CreateMap<TicketPriorityEvent, TicketEventEntity>()
                .ForMember(e => e.TicketId, opt => opt.MapFrom(@event => @event.AggregateRootId))
                .ForMember(e => e.Created, opt => opt.MapFrom(@event => @event.Created))
                .ForMember(e => e.UserId, opt => opt.MapFrom(@event => @event.UserId))
                .ForMember(e => e.PriorityEvent, opt => opt.MapFrom(@event => new TicketPriorityEventEntity { FromPriorityId = @event.FromPriorityId, ToPriorityId = @event.ToPriorityId}))
                .ForMember(e => e.Type, opt => opt.UseValue(EventType.Priority));
        }
    }
}
