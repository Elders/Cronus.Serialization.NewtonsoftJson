using System;
using System.Collections.Generic;
using System.IO;
using Machine.Specifications;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests
{
    [Subject(typeof(JsonSerializer))]
    public class When_serializing__StructType__
    {

        Establish context = () =>
        {
            ser = new StructType("12m, Currency.Usd");
            var contracts = new List<Type>();
            contracts.AddRange(typeof(StructType).Assembly.GetExportedTypes());
            serializer = new JsonSerializer(contracts);
            serializer2 = new JsonSerializer(contracts);
            serStream = new MemoryStream();
            serializer.Serialize(serStream, ser);
            serStream.Position = 0;

            //var json = Encoding.ASCII.GetString(serStream.ToArray());
            //serStream.Position = 0;
        };
        Because of_deserialization = () => deser = (StructType)serializer2.Deserialize(serStream);


        It should_not_be_null = () => deser.ShouldNotBeNull();
        It should_have_the_same_value = () => deser.ShouldEqual(ser);

        static StructType ser;
        static StructType deser;
        static MemoryStream serStream;
        static JsonSerializer serializer;
        static JsonSerializer serializer2;
    }
}
