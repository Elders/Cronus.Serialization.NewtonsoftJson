using Machine.Specifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests.NewFolder
{
    [Subject(typeof(JsonSerializer))]
    public class When_deserializing_json_which_is_not_DataContract
    {
        Establish context = () =>
        {
            ser = new NestedType()
            {
                Int = 5,
                Date = new DateTime(2019, 11, 29, 15, 16, 17, 0, DateTimeKind.Utc),
                String = "a",
                Nested = new SimpleNestedType()
                {
                    Int = 4,
                    Date = new DateTime(2019, 11, 28, 15, 16, 17, 0, DateTimeKind.Utc),
                    String = "b"
                }
            };
            var contracts = new List<Type>();
            contracts.AddRange(typeof(When_deserializing_json_which_is_not_DataContract).Assembly.GetExportedTypes());
            serializer = new JsonSerializer(contracts);
            json = @"
{
	""$type"": ""Elders.Cronus.Serialization.NewtonsoftJson.Tests.NestedType, Elders.Cronus.Serialization.NewtonsoftJson.Models"",
    ""String"": ""a"",
	""Int"": 5,
	""Date"": ""2019-11-29T15:16:17.000Z"",
	""Nested"": {
        ""$type"": ""Elders.Cronus.Serialization.NewtonsoftJson.Tests.SimpleNestedType, Elders.Cronus.Serialization.NewtonsoftJson.Models"",
		""String"": ""b"",
		""Int"": 4,
		""Date"": ""2019-11-28T15:16:17.000Z"",
		""Nested"": null

    }
}";
        };
        Because of_deserialization = () =>
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                deser = (NestedType)serializer.Deserialize(stream);
            }
        };


        It should_not_be_null = () => deser.ShouldNotBeNull();
        It should_have_the_same_int = () => deser.Int.ShouldEqual(ser.Int);
        It should_have_the_same_string = () => deser.String.ShouldEqual(ser.String);
        It should_have_the_same_date = () => deser.Date.ShouldEqual(ser.Date);
        It should_have_the_same_date_as_utc = () => deser.Date.ToFileTimeUtc().ShouldEqual(ser.Date.ToFileTimeUtc());

        It nested_object_should_not_be_null = () => deser.Nested.ShouldNotBeNull();
        It nested_object_should_have_the_same_int = () => deser.Nested.Int.ShouldEqual(ser.Nested.Int);
        It nested_object_should_have_the_same_string = () => deser.Nested.String.ShouldEqual(ser.Nested.String);
        It nested_object_should_have_the_same_date = () => deser.Nested.Date.ShouldEqual(ser.Nested.Date);
        It nested_object_should_have_the_same_date_as_utc = () => deser.Nested.Date.ToFileTimeUtc().ShouldEqual(ser.Nested.Date.ToFileTimeUtc());

        static string json;
        static NestedType ser;
        static NestedType deser;
        static MemoryStream serStream;
        static JsonSerializer serializer;
    }
}


