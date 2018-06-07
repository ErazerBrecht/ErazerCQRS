using System;
using System.Text;
using AutoMapper;
using Erazer.Framework.Events;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Erazer.Shared;

namespace Erazer.Infrastructure.EventStore
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<IDomainEvent, EventData>()
                .ConstructUsing(src => new EventData(Guid.NewGuid(), src.GetType().Name, true, new UTF8Encoding().GetBytes(JsonConvert.SerializeObject(src, JsonSettings.DefaultSettings)), null));

            CreateMap<ResolvedEvent, IDomainEvent>()
                .ConstructUsing(src => JsonConvert.DeserializeObject<IDomainEvent>(new UTF8Encoding().GetString(src.Event.Data), JsonSettings.DefaultSettings))
                .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.Event.EventNumber));
        }
    }
}
