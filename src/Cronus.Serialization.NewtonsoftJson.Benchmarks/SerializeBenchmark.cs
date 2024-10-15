using BenchmarkDotNet.Attributes;
using Elders.Cronus.Serialization.NewtonsoftJson;

[MemoryDiagnoser]
public class SerializeBenchmark
{
    private JsonSerializer serializer;
    private JsonSerializer deserializer;
    private ListData list;

    [GlobalSetup]
    public void Setup()
    {
        var contracts = new List<Type>();
        contracts.AddRange(typeof(CollectionSerializationBenchmark).Assembly.GetExportedTypes());
        serializer = new JsonSerializer(contracts);
        deserializer = new JsonSerializer(contracts);
        list = new ListData(Generate(NumberOfItems).ToList());
    }

    private IEnumerable<Data> Generate(int numberOfItems)
    {
        for (int i = 0; i < numberOfItems; i++)
        {
            yield return new Data(i);
        }
    }

    [Params(1, 1000, 100_000)]
    public int NumberOfItems { get; set; }

    [Benchmark(Baseline = true)]
    public byte[] SerializeToBytes()
    {
        return serializer.SerializeToBytes(list);
    }

    [Benchmark]
    public ReadOnlyMemory<byte> SerializeToReadOnlyMemory()
    {
        return serializer.SerializeToBytes(list);
    }

    [Benchmark]
    public string SerializeToString()
    {
        return serializer.SerializeToString(list);
    }
}
