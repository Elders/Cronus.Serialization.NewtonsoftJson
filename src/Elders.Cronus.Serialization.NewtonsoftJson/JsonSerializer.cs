using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Elders.Cronus.Serialization.Newtonsofst.Jsson;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;

namespace Elders.Cronus.Serialization.NewtonsoftJson
{
    public class JsonSerializer : ISerializer
    {
        ObjectPool<StringBuilder> pool = ObjectPool.Create(new StringBuilderPooledObjectPolicy());

        Newtonsoft.Json.JsonSerializer serializer;
        Newtonsoft.Json.JsonSerializer deserializer;

        public JsonSerializer(IEnumerable<Type> contracts)
        {
            var binder = new TypeNameSerializationBinder(contracts);

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.Converters.Add(new ReadOnlyMemoryJsonConverter<byte>());
            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            serializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            serializerSettings.ContractResolver = new DataMemberContractResolver();
            serializerSettings.TypeNameHandling = TypeNameHandling.Objects;
            serializerSettings.TypeNameAssemblyFormatHandling = Newtonsoft.Json.TypeNameAssemblyFormatHandling.Simple;
            serializerSettings.Formatting = Formatting.None;
            serializerSettings.SerializationBinder = binder;
            serializerSettings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
            serializer = Newtonsoft.Json.JsonSerializer.Create(serializerSettings);

            var deserializerSettings = new JsonSerializerSettings();
            deserializerSettings.Converters.Add(new ReadOnlyMemoryJsonConverter<byte>());
            deserializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            deserializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            deserializerSettings.ContractResolver = new DataMemberContractResolver();
            deserializerSettings.TypeNameHandling = TypeNameHandling.Objects;
            deserializerSettings.TypeNameAssemblyFormatHandling = Newtonsoft.Json.TypeNameAssemblyFormatHandling.Simple;
            deserializerSettings.Formatting = Formatting.None;
            deserializerSettings.SerializationBinder = binder;
            deserializerSettings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;

            deserializer = Newtonsoft.Json.JsonSerializer.Create(deserializerSettings);
        }

        public byte[] SerializeToBytes<T>(T message)
        {
            StringBuilder stringBuilder = null;

            try
            {
                stringBuilder = pool.Get();
                using (StringWriter sw = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
                {
                    using (JsonTextWriter tw = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(tw, message);
                    }
                    return Encoding.UTF8.GetBytes(sw.ToString());
                }
            }
            finally
            {
                if (stringBuilder is not null)
                    pool.Return(stringBuilder);
            }
        }

        public string SerializeToString<T>(T message)
        {
            StringBuilder stringBuilder = null;

            try
            {
                stringBuilder = pool.Get();
                using (StringWriter sw = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
                {
                    using (JsonTextWriter tw = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(tw, message);
                    }
                    return sw.ToString();
                }
            }
            finally
            {
                if (stringBuilder is not null)
                    pool.Return(stringBuilder);
            }
        }

        public T DeserializeFromBytes<T>(byte[] bytes)
        {
            try
            {
                using (var stream = new MemoryStream(bytes))
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                    return (T)deserializer.Deserialize(reader, typeof(T));
            }
            catch (Exception) { return default; }
        }
    }
}
