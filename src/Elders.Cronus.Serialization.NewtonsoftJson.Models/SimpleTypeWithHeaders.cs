using System;
using System.Runtime.Serialization;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests
{
    [DataContract(Name = "760270f2-cbd4-4759-a0ce-4895af4d2ab7")]
    public class SimpleTypeWithHeaders
    {
        [DataMember(Order = 1)]
        public string String { get; set; }

        [DataMember(Order = 2)]
        public int Int { get; set; }

        [DataMember(Order = 3)]
        public DateTime Date { get; set; }
    }
}
