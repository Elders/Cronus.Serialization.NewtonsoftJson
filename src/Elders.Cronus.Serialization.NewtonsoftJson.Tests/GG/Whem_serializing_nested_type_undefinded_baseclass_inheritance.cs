using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Elders.Cronus.Serialization.NewtonsoftJson.Models;
using Machine.Specifications;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests
{
    [Subject(typeof(JsonSerializer))]
    public class Whem_asf
    {

        Establish context = () =>
        {
            ser = new EventWithSpecificAggregateRootId()
            {
                Id = new EventStoreIndexManagerId("name", "tenant")
            };

            var contracts = new List<Type>();
            contracts.AddRange(typeof(EventWithSpecificAggregateRootId).Assembly.GetExportedTypes());
            serializer = new JsonSerializer(contracts);
            serStream = new MemoryStream();
            serializer.Serialize(serStream, ser);
            serStream.Position = 0;
            var json = Encoding.ASCII.GetString(serStream.ToArray());
            string newJson = json.Replace("635c3731-2e25-4b47-a1ef-80a0f248092b", "8aa438ee-fbc8-4425-aa65-b71c7a3f09d8");

            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(newJson)))
            {
                deser = (EventWithIAggregateRootId)serializer.Deserialize(stream);
            }
        };
        Because of_deserialization = () => { deser = (EventWithIAggregateRootId)serializer2.Deserialize(serStream); };


        It should_not_be_null = () => deser.ShouldNotBeNull();
        //It should_have_the_same_int = () => deser.Int.ShouldEqual(ser.Int);
        //It should_have_the_same_string = () => deser.String.ShouldEqual(ser.String);
        //It should_have_the_same_date = () => deser.Date.ShouldEqual(ser.Date);
        //It should_have_the_same_date_as_utc = () => deser.Date.ToFileTimeUtc().ShouldEqual(ser.Date.ToFileTimeUtc());

        //It nested_object_should_not_be_null = () => deser.Nested.ShouldNotBeNull();
        //It nested_object_should_be_of_the_right_type = () => deser.Nested.ShouldBeOfExactType(typeof(UndefinedBaseclassInheritance));
        //It nested_objects_should_contain_base_properties = () => ser.Nested.CustomBaseClassString.ShouldEqual(deser.Nested.CustomBaseClassString);
        //It nested_objects_should_contain_bases_base_properties = () => ser.Nested.CustomBaseBaseClassString.ShouldEqual(deser.Nested.CustomBaseBaseClassString);



        static EventWithSpecificAggregateRootId ser;
        static EventWithIAggregateRootId deser;
        static MemoryStream serStream;
        static JsonSerializer serializer;
        static JsonSerializer serializer2;
    }

    [Subject(typeof(JsonSerializer))]
    public class Whem_asfdfh
    {

        Establish context = () =>
        {
            ser = new EventWithIAggregateRootId()
            {
                Id = new EventStoreIndexManagerId("name", "tenant")
            };

            var contracts = new List<Type>();
            contracts.AddRange(typeof(EventWithIAggregateRootId).Assembly.GetExportedTypes());
            serializer = new JsonSerializer(contracts);
            serStream = new MemoryStream();
            serializer.Serialize(serStream, ser);
            serStream.Position = 0;
            var json = Encoding.ASCII.GetString(serStream.ToArray());
            string newJson = json.Replace("8aa438ee-fbc8-4425-aa65-b71c7a3f09d8", "635c3731-2e25-4b47-a1ef-80a0f248092b");

            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(newJson)))
            {
                deser = (EventWithSpecificAggregateRootId)serializer.Deserialize(stream);
            }
        };
        Because of_deserialization = () => { deser = (EventWithSpecificAggregateRootId)serializer2.Deserialize(serStream); };


        It should_not_be_null = () => deser.ShouldNotBeNull();
        //It should_have_the_same_int = () => deser.Int.ShouldEqual(ser.Int);
        //It should_have_the_same_string = () => deser.String.ShouldEqual(ser.String);
        //It should_have_the_same_date = () => deser.Date.ShouldEqual(ser.Date);
        //It should_have_the_same_date_as_utc = () => deser.Date.ToFileTimeUtc().ShouldEqual(ser.Date.ToFileTimeUtc());

        //It nested_object_should_not_be_null = () => deser.Nested.ShouldNotBeNull();
        //It nested_object_should_be_of_the_right_type = () => deser.Nested.ShouldBeOfExactType(typeof(UndefinedBaseclassInheritance));
        //It nested_objects_should_contain_base_properties = () => ser.Nested.CustomBaseClassString.ShouldEqual(deser.Nested.CustomBaseClassString);
        //It nested_objects_should_contain_bases_base_properties = () => ser.Nested.CustomBaseBaseClassString.ShouldEqual(deser.Nested.CustomBaseBaseClassString);



        static EventWithSpecificAggregateRootId deser;
        static EventWithIAggregateRootId ser;
        static MemoryStream serStream;
        static JsonSerializer serializer;
        static JsonSerializer serializer2;
    }
}
