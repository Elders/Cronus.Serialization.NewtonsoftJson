using System;
using System.Collections.Generic;
using Machine.Specifications;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests
{
    [Subject("protobuf-net")]
    public class When_serializing_simple_type
    {
        Establish context = () =>
        {
            ser = new SimpleType() { Int = 5, Date = DateTime.UtcNow.AddDays(1), String = "a", StructProp = new StructType("12, Currency.Usd") };
            var contracts = new List<Type>();
            contracts.AddRange(typeof(NestedType).Assembly.GetExportedTypes());
            serializer = new JsonSerializer(contracts);
            data = serializer.SerializeToBytes(ser);
        };

        Because of_deserialization = () => deser = serializer.DeserializeFromBytes<SimpleType>(data);

        It should_not_be_null = () => deser.ShouldNotBeNull();
        It should_have_the_same_int = () => deser.Int.ShouldEqual(ser.Int);
        It should_have_the_same_string = () => deser.String.ShouldEqual(ser.String);
        It should_have_the_same_date = () => deser.Date.ShouldEqual(ser.Date);
        It should_have_the_same_date_kind = () => deser.Date.Kind.ShouldEqual(ser.Date.Kind);
        It should_have_the_same_date_as_utc = () => deser.Date.ToFileTimeUtc().ShouldEqual(ser.Date.ToFileTimeUtc());
        It should_have_the_same_struct = () => deser.StructProp.ShouldEqual(ser.StructProp);

        static SimpleType ser;
        static SimpleType deser;
        static JsonSerializer serializer;
        static byte[] data;
    }
}
