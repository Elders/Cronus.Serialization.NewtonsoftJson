using System.Reflection;
using Elders.Cronus.Serializer;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Serialization;
using Elders.Cronus.Serialization.Newtonsofst.Jsson;

namespace Elders.Cronus.Serialization.NewtonsoftJson
{
    public class JsonSerializer : ISerializer
    {
        JsonSerializerSettings settings;

        Newtonsoft.Json.JsonSerializer serializer;

        public JsonSerializer(params Assembly[] assembliesContainingContracts)
        {
            settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            settings.ContractResolver = new DataMemberContractResolver();
            settings.TypeNameHandling = TypeNameHandling.Objects;
            settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            settings.Formatting = Formatting.Indented;
            settings.MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead;
            settings.Binder = new TypeNameSerializationBinder(assembliesContainingContracts);
            settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
            serializer = Newtonsoft.Json.JsonSerializer.Create(settings);
        }

        public object Deserialize(System.IO.Stream str)
        {
            //DO NOT DISPOSE THE JsonTextReader!!! DISPOSING IT CLOSES THE EXTERNAL STREAM
            var jsonReader = new JsonTextReader(new StreamReader(str));
            return serializer.Deserialize(jsonReader);
        }

        public void Serialize<T>(System.IO.Stream str, T message)
        {
            //DO NOT DISPOSE THE STREAM WRITER!!! DISPOSING IT CLOSES THE EXTERNAL STREAM
            var streamWriter = new StreamWriter(str);
            serializer.Serialize(streamWriter, message);
            streamWriter.Flush();
        }
    }
}
