using System;
using System.Runtime.Serialization;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests
{
    [DataContract(Name = "NestedType")]
    public class NestedType
    {
        [DataMember(Order = 1)]
        public string String { get; set; }
        [DataMember(Order = 2)]
        public int Int { get; set; }

        [DataMember(Order = 3)]
        public DateTime Date { get; set; }

        [DataMember(Order = 4)]
        public SimpleNestedType Nested { get; set; }
    }

    [DataContract(Name = "SimpleNestedType")]
    public class SimpleNestedType
    {
        [DataMember(Order = 1)]
        public string String { get; set; }
        [DataMember(Order = 2)]
        public int Int { get; set; }

        [DataMember(Order = 3)]
        public DateTime Date { get; set; }

        [DataMember(Order = 4)]
        public SimpleType Nested { get; set; }
    }
}
