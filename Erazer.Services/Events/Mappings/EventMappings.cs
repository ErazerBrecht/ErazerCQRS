using AutoMapper;
using Erazer.Domain.Events;
using Erazer.Services.Queries.ViewModels;

namespace Erazer.Services.Events.Mappings
{
    public class EventMappings : Profile
    {
        public EventMappings()
        {
            #region Event --> Redux 
            CreateMap<TicketCreateEvent, ReduxAction>()
                .ForMember(d => d.Type, opt => opt.UseValue(ReduxActionTypeConstants.AddTicket))
                .ForMember(d => d.Payload,
                    opt => opt.MapFrom(s => new TicketListViewModel {Id = s.AggregateRootId.ToString(), Title = s.Title}));


            #endregion
        }
    }
}
