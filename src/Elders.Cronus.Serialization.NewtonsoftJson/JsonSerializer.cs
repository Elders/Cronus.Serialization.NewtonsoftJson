using System;
using System.Collections.Generic;
using System.IO;
using Elders.Cronus.Serialization.Newtonsofst.Jsson;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Elders.Cronus.Serialization.NewtonsoftJson
{
    public class JsonSerializer : ISerializer
    {
        JsonSerializerSettings settings;

        Newtonsoft.Json.JsonSerializer serializer;

        public JsonSerializer(IEnumerable<Type> contracts)
        {
            settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            settings.ContractResolver = new DataMemberContractResolver();
            settings.TypeNameHandling = TypeNameHandling.Objects;
            settings.TypeNameAssemblyFormatHandling = Newtonsoft.Json.TypeNameAssemblyFormatHandling.Simple;
            settings.Formatting = Formatting.None;
            settings.MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead;
            settings.SerializationBinder = new TypeNameSerializationBinder(contracts);
            settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
            serializer = Newtonsoft.Json.JsonSerializer.Create(settings);
        }

        public object Deserialize(System.IO.Stream str)
        {
            using StreamReader sr = new StreamReader(str);
            using JsonReader reader = new JsonTextReader(sr);

            return serializer.Deserialize(reader);
        }

        public object Deserialize(System.IO.Stream str, Type objectType)
        {
            using StreamReader sr = new StreamReader(str);
            using JsonReader reader = new JsonTextReader(sr);

            return serializer.Deserialize(reader, objectType);
        }

        public void Serialize<T>(System.IO.Stream str, T message)
        {
            StreamWriter streamWriter = new StreamWriter(str);

            serializer.Serialize(streamWriter, message);
            streamWriter.Flush();
        }
    }
}
