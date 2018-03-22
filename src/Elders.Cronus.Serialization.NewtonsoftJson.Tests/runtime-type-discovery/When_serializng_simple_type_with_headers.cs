using System;
using System.Collections.Generic;
using System.IO;
using Machine.Specifications;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests.runtime_type_discovery
{
    [Subject(typeof(JsonSerializer))]
    public class When_serializng_simple_type_with_headers
    {
        Establish context = () =>
        {
            ser = new SimpleTypeWithHeaders() { Int = 5, Date = DateTime.UtcNow.AddDays(1), String = "a" };
            var contracts = new List<Type>();
            contracts.AddRange(typeof(NestedType).Assembly.GetExportedTypes());
            serializer = new JsonSerializer(contracts);
            serStream = new MemoryStream();
            serializer.Serialize(serStream, ser);
            serStream.Position = 0;
        };
        Because of_deserialization = () => { deser = (SimpleTypeWithHeaders)serializer.Deserialize(serStream); };


        It should_not_be_null = () => deser.ShouldNotBeNull();
        It should_have_the_same_int = () => deser.Int.ShouldEqual(ser.Int);
        It should_have_the_same_string = () => deser.String.ShouldEqual(ser.String);
        It should_have_the_same_date = () => deser.Date.ShouldEqual(ser.Date);
        It should_have_the_same_date_as_utc = () => deser.Date.ToFileTimeUtc().ShouldEqual(ser.Date.ToFileTimeUtc());

        static SimpleTypeWithHeaders ser;
        static SimpleTypeWithHeaders deser;
        static Stream serStream;
        static JsonSerializer serializer;
    }
}
