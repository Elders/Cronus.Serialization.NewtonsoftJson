using System;
using System.Collections.Generic;
using Elders.Cronus.Serialization.NewtonsoftJson.Models;
using Machine.Specifications;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests
{
    [Subject(typeof(JsonSerializer))]
    public class Whem_serializing__readonly_record_struct
    {
        Establish context = () =>
        {
            ser = new ReadonlyRecordStructType("12m, Currency.Usd");
            var contracts = new List<Type>();
            contracts.AddRange(typeof(StructType).Assembly.GetExportedTypes());
            serializer = new JsonSerializer(contracts);
            data = serializer.SerializeToBytes(ser);
        };

        Because of_deserialization = () => deser = serializer.DeserializeFromBytes<ReadonlyRecordStructType>(data);

        It should_not_be_null = () => deser.ShouldNotBeNull();
        It should_have_the_same_value = () => deser.ShouldEqual(ser);

        static ReadonlyRecordStructType ser;
        static ReadonlyRecordStructType deser;
        static JsonSerializer serializer;
        static byte[] data;
    }
}
