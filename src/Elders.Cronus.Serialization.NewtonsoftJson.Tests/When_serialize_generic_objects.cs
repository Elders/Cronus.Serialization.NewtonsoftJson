using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests
{
    [Subject(typeof(JsonSerializer))]
    public class When_serialize_generic_objects
    {
        Establish context = () =>
        {
            var response = new ImplementationOfGenericObject()
            {
                List = new List<KeyValuePair<string, int>>()
                {
                    new KeyValuePair<string, int>("123",1)
                },
                RawRequest = "123",
                RawResponse = "123"
            };
            ser = new GGA("123", new GenericObject<ImplementationOfGenericObject>("123", "source", response));
            var contracts = new List<Type>();
            contracts.AddRange(typeof(GGA).Assembly.GetExportedTypes());
            serializer = new JsonSerializer(contracts);
            data = serializer.SerializeToBytes(ser);
        };

        Because of_deserialization = () => deser = serializer.DeserializeFromBytes<GGA>(data);

        It should_not_be_null = () => deser.ShouldNotBeNull();

        static GGA ser;
        static GGA deser;
        static JsonSerializer serializer;
        static byte[] data;
    }

    [Subject(typeof(JsonSerializer))]
    public class When_serialize_generic_objects_with_string_param
    {
        Establish context = () =>
        {
            ser = new GGA("123", new GenericObject<string>("123", "source", "test"));
            var contracts = new List<Type>();
            contracts.AddRange(typeof(GGA).Assembly.GetExportedTypes());
            serializer = new JsonSerializer(contracts);
            data = serializer.SerializeToBytes(ser);
        };

        Because of_deserialization = () => deser = serializer.DeserializeFromBytes<GGA>(data);

        It should_not_be_null = () => deser.ShouldNotBeNull();

        static GGA ser;
        static GGA deser;
        static JsonSerializer serializer;
        static byte[] data;
    }

    [Subject(typeof(JsonSerializer))]
    public class When_serialize_generic_objects_with_object_param
    {
        Establish context = () =>
        {
            var response = new ImplementationOfGenericObject()
            {
                List = new List<KeyValuePair<string, int>>()
                {
                    new KeyValuePair<string, int>("123",1)
                },
                RawRequest = "123",
                RawResponse = "123"
            };
            ser = new GGA("123", new GenericObject<object>("123", "source", response));
            var contracts = new List<Type>();
            contracts.AddRange(typeof(GGA).Assembly.GetExportedTypes());
            serializer = new JsonSerializer(contracts);
        };

        Because of_deserialization = () => ex = Catch.Exception(() => data = serializer.SerializeToBytes(ser));

        It should_not_be_null = () => ex.ShouldNotBeNull();

        static GGA ser;
        static JsonSerializer serializer;
        static byte[] data;
        static Exception ex;
    }

    [DataContract(Namespace = "elders", Name = "0c3b5162-4386-4fc6-8d1d-f7f3430e4d40")]
    public class GGA
    {
        public GGA() { }

        public GGA(string id, IGenericObject<object> externalReward)
        {
            Id = id;
            ExternalReward = externalReward;
        }

        [DataMember(Order = 1)]
        public string Id { get; set; }

        [DataMember(Order = 2)]
        public IGenericObject<object> ExternalReward { get; private set; }
    }

    [DataContract(Namespace = "elders", Name = "45a21282-fc4d-4c1c-a315-8eb6bc9bbcc6")]
    public record class GenericObject<T> : IGenericObject<T>
    {
        GenericObject() { }

        public GenericObject(string id, string source, T response)
        {
            Id = id;
            Source = source;
            Response = response;
        }

        [DataMember(Order = 1)]
        public string Id { get; private set; }

        [DataMember(Order = 2)]
        public string Source { get; private set; }

        [DataMember(Order = 3)]
        public T Response { get; private set; }
    }

    public interface IGenericObject<out T>
    {
        string Id { get; }

        string Source { get; }

        T Response { get; }
    }

    [DataContract(Namespace = "elders", Name = "4f3575cb-5086-40ac-bbe8-179ef623f140")]
    public class ImplementationOfGenericObject
    {
        public ImplementationOfGenericObject()
        {
            List = new List<KeyValuePair<string, int>>();
        }

        [DataMember(Order = 1)]
        public List<KeyValuePair<string, int>> List { get; set; }

        [DataMember(Order = 2)]
        public string RawRequest { get; set; }

        [DataMember(Order = 3)]
        public string RawResponse { get; set; }
    }
}
