using Machine.Specifications;
using System.Runtime.Serialization;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests
{
    [Subject(typeof(JsonSerializer))]
    public class When_json_has_type_problem
    {
        Establish context = () =>
        {
            var contracts = new List<Type>();
            contracts.AddRange(typeof(TypeProblem_TopLevel).Assembly.GetExportedTypes());
            serializer = new JsonSerializer(contracts);
            json = @"
{
""$type"": ""07504e95-e40e-40b5-b7f6-209f6023c13c"",
	""1"": {
""$type"": ""bbed4f91-926e-484e-a4a2-da47a9302bcf"",
        ""1"": ""Testing photo broadcasts to Promoters.. 1""
    }
}";

        };
        Because of_deserialization = () =>
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                result = (TypeProblem_TopLevel)serializer.Deserialize(stream);
            }
        };

        It should_not_be_null = () => result.ShouldNotBeNull();

        It inner_should_not_be_null = () => result.Prop.ShouldNotBeNull();

        static string json;
        static JsonSerializer serializer;
        static TypeProblem_TopLevel result;
    }

    [DataContract(Name = "07504e95-e40e-40b5-b7f6-209f6023c13c")]
    public class TypeProblem_TopLevel
    {
        [DataMember(Order = 1)]
        public TypeProblem_Property Prop { get; set; }

    }

    [DataContract(Name = "bbed4f91-926e-484e-a4a2-da47a9302bcf")]
    public class TypeProblem_Property
    {
        [DataMember(Order = 1)]
        public string Text { get; set; }
    }
}
