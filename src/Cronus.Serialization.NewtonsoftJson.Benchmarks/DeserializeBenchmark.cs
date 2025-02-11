using BenchmarkDotNet.Attributes;
using Elders.Cronus.Serialization.NewtonsoftJson;

[MemoryDiagnoser]
public class DeserializeBenchmark
{
    private JsonSerializer serializer;
    private ListData list;
    private byte[] serializedData;
    private ReadOnlyMemory<byte> serializedDataAsMemory;

    [GlobalSetup]
    public void Setup()
    {
        var contracts = new List<Type>();
        contracts.AddRange(typeof(DeserializeBenchmark).Assembly.GetExportedTypes());
        serializer = new JsonSerializer(contracts);

        serializedData = serializer.SerializeToBytes(list);
        serializedDataAsMemory = new ReadOnlyMemory<byte>(serializedData);
        list = new ListData(Generate(NumberOfItems).ToList());
    }

    [Params(1, 1000, 100_000)]
    public int NumberOfItems { get; set; }

    private IEnumerable<Data> Generate(int numberOfItems)
    {
        for (int i = 0; i < numberOfItems; i++)
        {
            yield return new Data(i);
        }
    }

    [Benchmark(Baseline = true)]
    public IListData DeserializeFromBytes()
    {
        return serializer.DeserializeFromBytes<IListData>(serializedData);
    }
}


