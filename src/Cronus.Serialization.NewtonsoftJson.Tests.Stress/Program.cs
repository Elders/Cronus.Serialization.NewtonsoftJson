using Elders.Cronus;
using Elders.Cronus.Serialization.NewtonsoftJson;
using Elders.Cronus.Serialization.NewtonsoftJson.Tests;
using CommunityToolkit.HighPerformance;

namespace Cronus.Serialization.NewtonsoftJson.Tests.Stress
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var contracts = new List<Type>();
            contracts.AddRange(typeof(NestedType).Assembly.GetExportedTypes());
            var serializer = new JsonSerializer(contracts);

            for (int cc = 0; cc < int.MaxValue; cc++)
            {
                var instance = new TypeWithCollection(cc);
                for (int i = 0; i < 5; i++)
                {
                    var item = new TypeWithCollectionItem() { Int = cc + i, Date = DateTime.UtcNow.AddDays(i), String = $"string_{cc}_{i}", StructProp = new StructType($"{cc}, {i}") };
                    instance.Collection.Add(item);
                }

                byte[] data = serializer.SerializeToBytes(instance);

                TypeWithCollection deserialized = (TypeWithCollection)serializer.DeserializeFromBytes(data);
            }
        }
    }
}
