using BenchmarkDotNet.Attributes;
using Elders.Cronus.Serialization.NewtonsoftJson;

//BenchmarkRunner.Run<DeserializeBenchmark>();

//var d = new DeserializeBenchmark();
//d.DeserializeCronus();
//d.DeserializeCronusNew();


[MemoryDiagnoser]
public class DeserializeBenchmark
{
    private readonly JsonSerializer serializer;

    public DeserializeBenchmark()
    {
        var contracts = new List<Type>();
        contracts.AddRange(typeof(DeserializeBenchmark).Assembly.GetExportedTypes());
        serializer = new JsonSerializer(contracts);

        serializedData = serializer.SerializeToBytes(list);
    }

    private static IEnumerable<Data> Generate(int numberOfItems)
    {
        for (int i = 0; i < numberOfItems; i++)
        {
            yield return new Data(i);
        }
    }

    [Params(1)]
    public static int NumberOfItems { get; set; } = 1;
    static ListData list = new ListData(Generate(NumberOfItems).ToList());

    static byte[] serializedData;

    [Benchmark(Baseline = true)]
    public void DeserializeCronusNew()
    {
        _ = serializer.DeserializeFromBytes<IListData>(serializedData);
    }

    public void ggg()
    {
        var bytes = serializer.SerializeToString(list);
    }
}


