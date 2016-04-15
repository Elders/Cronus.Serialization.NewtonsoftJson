﻿using System;
using System.IO;
using Machine.Specifications;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests
{
    [Subject(typeof(JsonSerializer))]
    public class Whem_serializing_nested_type_undefinded_object_inheritance
    {

        Establish context = () =>
        {
            ser = new NestedTypeWithUndefinedObjectInheritance() { Int = 5, Date = DateTime.UtcNow.AddDays(1), String = "a", Nested = new UndefinedObjectInheritance() { Int = 4, Date = DateTime.UtcNow.AddDays(2), String = "b" } };
            serializer = new JsonSerializer((typeof(NestedType).Assembly));
            serializer2 = new JsonSerializer((typeof(NestedType).Assembly));
            serStream = new MemoryStream();
            serializer.Serialize(serStream, ser);
            serStream.Position = 0;
        };
        Because of_deserialization = () => { deser = (NestedTypeWithUndefinedObjectInheritance)serializer2.Deserialize(serStream); };


        It should_not_be_null = () => deser.ShouldNotBeNull();
        It should_have_the_same_int = () => deser.Int.ShouldEqual(ser.Int);
        It should_have_the_same_string = () => deser.String.ShouldEqual(ser.String);
        It should_have_the_same_date = () => deser.Date.ShouldEqual(ser.Date);
        It should_have_the_same_date_as_utc = () => deser.Date.ToFileTimeUtc().ShouldEqual(ser.Date.ToFileTimeUtc());

        It nested_object_should_not_be_null = () => deser.Nested.ShouldNotBeNull();
        It nested_object_should_be_of_the_right_type = () => deser.Nested.ShouldBeOfExactType(typeof(UndefinedObjectInheritance));



        static NestedTypeWithUndefinedObjectInheritance ser;
        static NestedTypeWithUndefinedObjectInheritance deser;
        static Stream serStream;
        static JsonSerializer serializer;
        static JsonSerializer serializer2;
    }
}