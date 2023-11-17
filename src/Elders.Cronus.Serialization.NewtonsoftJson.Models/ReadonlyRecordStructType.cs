using System.Runtime.Serialization;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Models
{
    [DataContract(Name = "b22776e2-a76a-4484-a8eb-c8ee2544ae62")]
    public readonly record struct ReadonlyRecordStructType
    {
        public ReadonlyRecordStructType(string value)
        {
            Value = value;
            Value2 = 1;
            ValueShort = 2;
            ValueString = "stringValue";
        }

        [DataMember(Order = 1)]
        private readonly string Value;

        [DataMember(Order = 2)]
        private readonly int Value2;

        [DataMember(Order = 3)]
        public ushort ValueShort { get; init; }

        [DataMember(Order = 4)]
        public string ValueString { get; init; }
    }
}
