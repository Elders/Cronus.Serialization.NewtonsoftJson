using System;
using System.Collections.Generic;
using System.IO;
using Machine.Specifications;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests
{
    [Subject(typeof(JsonSerializer))]
    public class When_serializing_CronusMessage_with_raw_body
    {
        Establish context = () =>
        {
            var contracts = new List<Type>();
            contracts.AddRange(typeof(NestedType).Assembly.GetExportedTypes());
            contracts.AddRange(typeof(CronusMessage).Assembly.GetExportedTypes());
            serializer = new JsonSerializer(contracts);

            var body = new NestedTypeWithHeaders()
            {
                Int = 5,
                Date = DateTime.UtcNow.AddDays(1),
                String = "a",
                Nested = new SimpleNestedTypeWithHeaders()
                {
                    Int = 4,
                    Date = DateTime.UtcNow.AddDays(2),
                    String = "b"
                }
            };

            using var serStream = new MemoryStream();
            serializer.Serialize(serStream, body);

            ser = new CronusMessage(serStream.ToArray(), typeof(NestedTypeWithHeaders), new Dictionary<string, string>());
            cmBytes = serializer.SerializeToBytes(ser);
        };
        Because of_deserialization = () => deser = (CronusMessage)serializer.DeserializeFromBytes(cmBytes);

        It should_not_be_null = () => deser.ShouldNotBeNull();
        It should_have_the_same_byte_array = () => ByteArrayHelper.Compare(deser.PayloadRaw, ser.PayloadRaw).ShouldBeTrue();
        It should_have_the_same_byte_array_length = () => deser.PayloadRaw.Length.ShouldEqual(ser.PayloadRaw.Length);

        static CronusMessage ser;
        static CronusMessage deser;
        static JsonSerializer serializer;
        static byte[] cmBytes;
    }
}
