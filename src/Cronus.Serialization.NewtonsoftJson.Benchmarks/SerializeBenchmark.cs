using BenchmarkDotNet.Attributes;
using Elders.Cronus.Serialization.NewtonsoftJson;

//BenchmarkRunner.Run<SerializeBenchmark>();

//var d = new SerializeBenchmark();
//d.SerializeCronus();
//d.SerializeStringBuilder();
//d.SerializeStringBuilderPool();
//d.ggg();

[MemoryDiagnoser]
public class SerializeBenchmark
{
    private readonly JsonSerializer serializer;
    private readonly JsonSerializer deserializer;

    public SerializeBenchmark()
    {
        var contracts = new List<Type>();
        contracts.AddRange(typeof(CollectionSerializationBenchmark).Assembly.GetExportedTypes());
        serializer = new JsonSerializer(contracts);
        deserializer = new JsonSerializer(contracts);
    }

    private static IEnumerable<Data> Generate(int numberOfItems)
    {
        for (int i = 0; i < numberOfItems; i++)
        {
            yield return new Data(i);
        }
    }

    [Params(1)]
    public static int NumberOfItems { get; set; }
    static ListData list = new ListData(Generate(NumberOfItems).ToList());

    [Benchmark(Baseline = true)]
    public void SerializeCronus()
    {
        var bytes = serializer.SerializeToBytes(list);
    }

    [Benchmark()]
    public void SerializeCronusNew()
    {
        var bytes = serializer.SerializeToBytes(list);
    }

    public void ggg()
    {
        var bytes = serializer.SerializeToString(list);
    }
}


