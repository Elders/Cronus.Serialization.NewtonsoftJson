using System;
using System.IO;
using System.Runtime.Serialization;
using Machine.Specifications;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests
{
    [Subject(typeof(JsonSerializer))]
    public class Whem_stackoverflow
    {
        Establish context = () =>
        {
            ser = new C() { CString = "C", Aprop = new A() { AString = "A" } };

            serializer = new JsonSerializer(typeof(NestedType).Assembly, typeof(Whem_stackoverflow).Assembly);
            serializer2 = new JsonSerializer(typeof(NestedType).Assembly, typeof(Whem_stackoverflow).Assembly);
            serStream = new MemoryStream();
            serializer.Serialize(serStream, ser);
            serStream.Position = 0;
        };
        Because of_deserialization = () => { deser = (C)serializer.Deserialize(serStream); };

        It should_not_be_null = () => deser.ShouldNotBeNull();


        static C ser;
        static C deser;
        static Stream serStream;
        static JsonSerializer serializer;
        static JsonSerializer serializer2;
    }

    [DataContract(Name = "79bb4bef-13d0-4a24-8e5a-f52e38b73eff")]
    public class A
    {
        [DataMember(Order = 1)]
        public string AString { get; set; }
    }
    [DataContract(Name = "70eb3522-0670-4449-a514-cd0082cededb")]
    public class B : A
    {
        [DataMember(Order = 2)]
        public string BString { get; set; }
    }
    [DataContract(Name = "78eb1d0c-9284-4f17-b8e5-9a29e5cc7a5b")]
    public class C
    {
        [DataMember(Order = 3)]
        public string CString { get; set; }

        [DataMember(Order = 4)]
        public A Aprop { get; set; }
    }
}