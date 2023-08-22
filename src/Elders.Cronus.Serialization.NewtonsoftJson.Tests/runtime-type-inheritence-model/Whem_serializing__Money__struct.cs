using System;
using System.Collections.Generic;
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
            data = serializer.SerializeToBytes(ser);
        };

        Because of_deserialization = () => deser = serializer.DeserializeFromBytes<StructType>(data);

        It should_not_be_null = () => deser.ShouldNotBeNull();
        It should_have_the_same_value = () => deser.ShouldEqual(ser);

        static StructType ser;
        static StructType deser;
        static JsonSerializer serializer;
        static byte[] data;
    }
}
