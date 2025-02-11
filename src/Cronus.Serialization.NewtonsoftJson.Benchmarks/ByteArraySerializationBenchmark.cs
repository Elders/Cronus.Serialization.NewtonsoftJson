using System.Runtime.Serialization;
using BenchmarkDotNet.Attributes;
using Elders.Cronus.Serialization.NewtonsoftJson;

[MemoryDiagnoser]
public class ByteArraySerializationBenchmark
{
    private readonly JsonSerializer serializer;
    private readonly JsonSerializer deserializer;

    public ByteArraySerializationBenchmark()
    {
        var contracts = new List<Type>();
        contracts.AddRange(typeof(ByteArraySerializationBenchmark).Assembly.GetExportedTypes());
        serializer = new JsonSerializer(contracts);
        deserializer = new JsonSerializer(contracts);
    }

    static byte[] bytes = Convert.FromBase64String("dXJuOnBydXZpdDpzcG9uc29yOmR4anVvbmJ5ZHh6cGRkcHdjbTltYXd4bG9qZTNtanU1bWc9PQ==");
    static ReadOnlyMemory<byte> memory = Convert.FromBase64String("dXJuOnBydXZpdDpzcG9uc29yOmR4anVvbmJ5ZHh6cGRkcHdjbTltYXd4bG9qZTNtanU1bWc9PQ==");
    static ByteArrayData byteArrayData = new ByteArrayData { Data = GenerateDataWithByteArray(N).ToList() };
    static ReadOnlyMemoryData readOnlyMemoryData = new ReadOnlyMemoryData { Data = GenerateDataWithReadOnlyMemory(N).ToList() };

    private static IEnumerable<DataWithByteArray> GenerateDataWithByteArray(int numberOfItems)
    {
        for (int i = 0; i < numberOfItems; i++)
        {
            yield return new DataWithByteArray(bytes);
        }
    }

    private static IEnumerable<DataWithReadOnlyMemory> GenerateDataWithReadOnlyMemory(int numberOfItems)
    {
        for (int i = 0; i < numberOfItems; i++)
        {
            yield return new DataWithReadOnlyMemory(memory);
        }
    }

    [Params(1, 10, 100, 1000, 10000)]
    public static int N;

    [Benchmark(Baseline = true)]
    public ByteArrayData ByteArray()
    {
        var bytes = serializer.SerializeToBytes(byteArrayData);
        return deserializer.DeserializeFromBytes<ByteArrayData>(bytes);
    }

    [Benchmark]
    public ReadOnlyMemoryData ReadOnlyMemory()
    {
        var bytes = serializer.SerializeToBytes(readOnlyMemoryData);
        return deserializer.DeserializeFromBytes<ReadOnlyMemoryData>(bytes);
    }

    [DataContract(Name = "29b6797f-5dad-4a1c-a0e1-3528bf060e2e")]
    public class ByteArrayData
    {
        [DataMember(Order = 1)]
        public List<DataWithByteArray> Data { get; set; }
    }

    [DataContract(Name = "cb89d050-b078-492e-88ad-95ac6d69cea4")]
    public class ReadOnlyMemoryData
    {
        [DataMember(Order = 1)]
        public List<DataWithReadOnlyMemory> Data { get; set; }
    }

    [DataContract(Name = "e894ccff-b6ad-4941-9725-50a3167f6035")]
    public class DataWithByteArray
    {
        DataWithByteArray() { }

        public DataWithByteArray(byte[] bytes)
        {
            Bytes = bytes;
        }

        [DataMember(Order = 1)]
        public byte[] Bytes { get; private set; }
    }

    [DataContract(Name = "77c458ce-f8df-4f3b-9291-ef841ac99b3e")]
    public class DataWithReadOnlyMemory
    {
        DataWithReadOnlyMemory() { }

        public DataWithReadOnlyMemory(ReadOnlyMemory<byte> bytes)
        {
            Bytes = bytes;
        }

        [DataMember(Order = 1)]
        public ReadOnlyMemory<byte> Bytes { get; private set; }
    }
}
