using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Machine.Specifications;
using Newtonsoft.Json;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests.custom_cases
{
    [DataContract(Name = "a8507632-d88c-4305-855b-5f3456fcc125")]
    [JsonObject(MemberSerialization.OptOut)]
    public class SomeClass
    {
        public string Value1 { get; set; }
        public string Value2 { get; set; }
    }

    [Subject(typeof(JsonSerializer))]
    public class Whem_Deserializing_json_which_has_no_DataContract
    {
        Establish context = () =>
        {
            var contracts = new List<Type>();
            contracts.AddRange(typeof(SomeClass).Assembly.GetExportedTypes());
            serializer = new JsonSerializer(contracts);

            bytes = Encoding.UTF8.GetBytes(json);

        };
        Because of_deserialization = () => deser = serializer.DeserializeFromBytes<SomeClass>(bytes);

        It should_not_be_null = () => deser.ShouldNotBeNull();

        static SomeClass deser;
        static JsonSerializer serializer;
        static byte[] bytes;
        static string json = @"
{
    ""value1"": ""the value 1"",
    ""value2"": ""the value 2"",
}
";
    }
}
