using System;
using System.Collections.Generic;
using System.IO;
using Elders.Cronus.Serialization.Newtonsofst.Jsson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            //DO NOT DISPOSE THE JsonTextReader!!! DISPOSING IT CLOSES THE EXTERNAL STREAM
            var jsonReader = new JsonTextReader(new StreamReader(str));
            object result = serializer.Deserialize(jsonReader);
            if (result is JObject)
            {
                result = (result as JObject).ToObject<CronusMessage>();
            }

            return result;
        }

        public void Serialize<T>(System.IO.Stream str, T message)
        {
            // DO NOT DISPOSE THE STREAM WRITER!!! DISPOSING IT CLOSES THE EXTERNAL STREAM
            var streamWriter = new StreamWriter(str);
            serializer.Serialize(streamWriter, message);
            streamWriter.Flush();
        }
    }
}
