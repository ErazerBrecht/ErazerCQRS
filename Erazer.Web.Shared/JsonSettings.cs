using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Erazer.Web.Shared
{
    public static class JsonSettings
    {
        public static JsonSerializerSettings DefaultSettings => new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
            FloatFormatHandling = FloatFormatHandling.DefaultValue,
            NullValueHandling = NullValueHandling.Include,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Error,
            StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
            TypeNameHandling = TypeNameHandling.All
        };

        public static JsonSerializerSettings AggregateSerializer
        {
            get
            {
                var settings = DefaultSettings;
                settings.ContractResolver = new PrivateContractResolver();
                settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
                return settings;
            }
        }

        public static JsonSerializerSettings CamelCaseSerializer
        {
            get
            {
                var settings = DefaultSettings;
                settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                settings.TypeNameHandling = TypeNameHandling.None;
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                return settings;
            }
        }
    }

    public class PrivateContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                            .Select(p => CreateProperty(p, memberSerialization))
                        .Union(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance )
                             .Select(f => CreateProperty(f, memberSerialization)))
                        .ToList();
            props.ForEach(p => { p.Writable = true; p.Readable = true; });
            return props;
        }
    }
}
