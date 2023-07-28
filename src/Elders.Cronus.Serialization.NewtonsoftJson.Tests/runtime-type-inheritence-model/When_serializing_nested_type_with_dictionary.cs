using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests
{
    [Subject(typeof(JsonSerializer))]
    public class When_serializing_nested_type_with_dictionary
    {
        Establish context = () =>
        {
            ser = new NestedTypeWithDctionaryInheritance() { Int = 5, Date = DateTime.UtcNow.AddDays(1), String = "a", Nested = new Dictionary<object, object>() };
            var key = new UndefinedDictionaryInheritance() { String = "key", Nested = new UndefinedDictionaryInheritance() { String = "Nested key" } };
            var value = new UndefinedDictionaryInheritance() { String = "value", Nested = new UndefinedDictionaryInheritance() { String = "Nested value" } };
            ser.Nested.Add(key, value);

            serializer = new JsonSerializer((typeof(NestedType).Assembly.GetExportedTypes()));
            data = serializer.SerializeToBytes(ser);
        };

        Because of_deserialization = () => deser = serializer.DeserializeFromBytes<NestedTypeWithDctionaryInheritance>(data);

        It should_not_be_null = () => deser.ShouldNotBeNull();
        It should_have_the_same_int = () => deser.Int.ShouldEqual(ser.Int);
        It should_have_the_same_string = () => deser.String.ShouldEqual(ser.String);
        It should_have_the_same_date = () => deser.Date.ShouldEqual(ser.Date);
        It should_have_the_same_date_as_utc = () => deser.Date.ToFileTimeUtc().ShouldEqual(ser.Date.ToFileTimeUtc());

        It nested_object_should_not_be_null = () => deser.Nested.ShouldNotBeNull();
        It nested_object_should_be_of_the_right_type = () => deser.Nested.ShouldBeOfExactType(typeof(Dictionary<object, object>));
        It nested_object_key_should_be_of_the_right_type = () => deser.Nested.First().Key.ShouldBeOfExactType(typeof(UndefinedDictionaryInheritance));
        It nested_object_value_should_be_of_the_right_type = () => deser.Nested.First().Value.ShouldBeOfExactType(typeof(UndefinedDictionaryInheritance));

        static NestedTypeWithDctionaryInheritance ser;
        static NestedTypeWithDctionaryInheritance deser;
        static JsonSerializer serializer;
        static byte[] data;
    }
}
