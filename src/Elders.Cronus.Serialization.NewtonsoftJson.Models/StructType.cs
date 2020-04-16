using System.Runtime.Serialization;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests
{
    [DataContract(Name = "8efe793f-7179-4986-bd78-73184c928602")]
    public struct StructType
    {
        public StructType(string value)
        {
            Value = value;
            Value2 = 0;
        }
        [DataMember(Order = 1)]
        private readonly string Value;

        [DataMember(Order = 2)]
        private readonly int Value2;
    }
}
