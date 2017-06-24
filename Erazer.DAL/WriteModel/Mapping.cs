using System;
using System.Text;
using AutoMapper;
using Erazer.Framework.Events;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Erazer.Web.Shared;

namespace Erazer.DAL.WriteModel
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<IEvent, EventData>()
                .ConstructUsing(src => new EventData(Guid.NewGuid(), src.GetType().AssemblyQualifiedName, true, new UTF8Encoding().GetBytes(JsonConvert.SerializeObject(src, DefaultJsonSerializerSettings.DefaultSettings)), null));

            CreateMap<ResolvedEvent, IEvent>()
                .ConstructUsing(src => JsonConvert.DeserializeObject<IEvent>(new UTF8Encoding().GetString(src.Event.Data), DefaultJsonSerializerSettings.DefaultSettings))
                .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.Event.EventNumber))
                .ForMember(dest => dest.AggregateRootId, opt => opt.MapFrom(src => src.Event.EventStreamId));
        }
    }
}
