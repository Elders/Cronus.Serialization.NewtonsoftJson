using System;
using Newtonsoft.Json;

namespace Elders.Cronus.Serialization.NewtonsoftJson
{
    internal sealed class ReadOnlyMemoryJsonConverter<T> : JsonConverter<ReadOnlyMemory<T>>
    {
        public override ReadOnlyMemory<T> ReadJson(JsonReader reader, Type objectType, ReadOnlyMemory<T> existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var buffer = serializer.Deserialize<T[]>(reader);
            return new ReadOnlyMemory<T>(buffer);
        }

        public override void WriteJson(JsonWriter writer, ReadOnlyMemory<T> value, Newtonsoft.Json.JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToArray());
        }
    }
}
