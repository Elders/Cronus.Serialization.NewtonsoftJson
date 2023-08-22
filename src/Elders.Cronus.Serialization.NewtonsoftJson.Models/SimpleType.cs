using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests
{
    [DataContract(Name = "TypeWithCollection")]
    public class TypeWithCollection
    {
        public TypeWithCollection()
        {
            Collection = new List<TypeWithCollectionItem>();
        }

        public TypeWithCollection(int id) : this()
        {
            Id = id;
        }

        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public List<TypeWithCollectionItem> Collection { get; set; }
    }

    [DataContract(Name = "TypeWithCollectionItem")]
    public class TypeWithCollectionItem
    {
        [DataMember(Order = 1)]
        public string String { get; set; }

        [DataMember(Order = 2)]
        public int Int { get; set; }

        [DataMember(Order = 3)]
        public DateTime Date { get; set; }

        [DataMember(Order = 4)]
        public StructType StructProp { get; set; }
    }

    [DataContract(Name = "SimpleType")]
    public class SimpleType
    {
        [DataMember(Order = 1)]
        public string String { get; set; }

        [DataMember(Order = 2)]
        public int Int { get; set; }

        [DataMember(Order = 3)]
        public DateTime Date { get; set; }

        [DataMember(Order = 4)]
        public StructType StructProp { get; set; }
    }

    [DataContract(Name = "099c2ee0-514f-4b73-b495-01341e3bef0f")]
    public class SimpleObject
    {
        [DataMember(Order = 1)]
        public string StringProp { get; set; }

        [DataMember(Order = 2)]
        public int IntProp { get; set; }
    }

    [DataContract(Name = "13184f27-ff1d-4085-a9a4-6356f808947a")]
    public class ComplexObjectGraph
    {
        [DataMember(Order = 1)]
        public string StringProp { get; set; }

        [DataMember(Order = 2)]
        public int IntProp { get; set; }

        [DataMember(Order = 3)]
        public object Nested { get; set; }
    }
}
