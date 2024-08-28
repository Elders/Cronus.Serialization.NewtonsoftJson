using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Machine.Specifications;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests.custom_cases
{
    [Subject(typeof(JsonSerializer))]
    public class When_AggregateRootId_to_Urn
    {
        static Guid WorkGuid = new Guid("eeceebfd-9f2f-4c0a-8242-6d7df1470d13");
        static Guid JobId = new Guid("debd3009-120b-45d9-84ba-e2216f597de0");
        Establish context = () =>
        {
            var id = AR_ID.New("elders", "abc123");

            var ser = new Urn(id.Value);
            var contracts = new List<Type>();
            contracts.AddRange(typeof(AR_ID).Assembly.GetExportedTypes().Concat(typeof(Urn).Assembly.GetExportedTypes()));
            serializer = new JsonSerializer(contracts);
            serializer2 = new JsonSerializer(contracts.Except(new[] { typeof(AR_ID) }));
            serStream = new MemoryStream();
            data = serializer.SerializeToBytes(ser);
            serStream.Position = 0;
        };
        Because of_deserialization = () => deser = serializer2.DeserializeFromBytes<Urn>(data);

        It should_not_be_null = () => deser.ShouldNotBeNull();

        static Urn deser;
        static Stream serStream;
        static JsonSerializer serializer;
        static JsonSerializer serializer2;
        static byte[] data;
    }

    [DataContract(Name = "0c499fdf-56a1-40e6-9c7c-e559f07ddfec")]
    public class AR_ID : AggregateRootId<AR_ID>
    {
        AR_ID() { }
        AR_ID(ReadOnlySpan<char> tenant, ReadOnlySpan<char> id) : base(tenant, id) { }

        public override ReadOnlySpan<char> AggregateRootName => "ar_id";
    }
}
