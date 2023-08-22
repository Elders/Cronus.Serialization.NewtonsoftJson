using BenchmarkDotNet.Attributes;
using Elders.Cronus.Serialization.NewtonsoftJson;
using System.Runtime.Serialization;

//BenchmarkRunner.Run<CollectionSerializationBenchmark>();
//var test = new CollectionSerializationBenchmark();
//test.SerializeRecordData();

//Console.WriteLine("test");

[MemoryDiagnoser]
public class CollectionSerializationBenchmark
{
    private readonly JsonSerializer serializer;
    private readonly JsonSerializer deserializer;

    public CollectionSerializationBenchmark()
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

    [Params(1, 10, 100, 1000, 10000)]
    public static int NumberOfItems { get; set; }

    static EnumerableData enumerable = new EnumerableData(Generate(NumberOfItems));
    static SealedEnumerableData sealedenumerable = new SealedEnumerableData(Generate(NumberOfItems));

    static ListData list = new ListData(Generate(NumberOfItems).ToList());
    static ArrayData array = new ArrayData(Generate(NumberOfItems).ToArray());

    static SealedListData sealedlist = new SealedListData(Generate(NumberOfItems).ToList());
    static SealedArrayData sealedarray = new SealedArrayData(Generate(NumberOfItems).ToArray());

    static RecordData recordData = new RecordData() { TheData = Generate(NumberOfItems).ToList() };

    [Benchmark()]
    public void SerializeEnumerable()
    {
        var bytes = serializer.SerializeToBytes(enumerable);
        _ = deserializer.DeserializeFromBytes<EnumerableData>(bytes);
    }

    [Benchmark(Baseline = true)]
    public void SerializeList()
    {
        var bytes = serializer.SerializeToBytes(list);
        _ = deserializer.DeserializeFromBytes<ListData>(bytes);
    }

    [Benchmark()]
    public void SerializeArray()
    {
        var bytes = serializer.SerializeToBytes(array);
        _ = deserializer.DeserializeFromBytes<ArrayData>(bytes);
    }

    [Benchmark()]
    public void SerializeSealedEnumerable()
    {
        var bytes = serializer.SerializeToBytes(sealedenumerable);
        _ = deserializer.DeserializeFromBytes<SealedEnumerableData>(bytes);
    }

    [Benchmark()]
    public void SerializeSealedList()
    {
        var bytes = serializer.SerializeToBytes(sealedlist);
        _ = deserializer.DeserializeFromBytes<SealedListData>(bytes);
    }

    [Benchmark()]
    public void SerializeSealedArray()
    {
        var bytes = serializer.SerializeToBytes(sealedarray);
        _ = deserializer.DeserializeFromBytes<SealedArrayData>(bytes);
    }

    [Benchmark()]
    public void SerializeRecordData()
    {
        var bytes = serializer.SerializeToBytes(recordData);
        _ = deserializer.DeserializeFromBytes<RecordData>(bytes);
    }
}

[DataContract(Name = "36088a07-7a91-4f07-92d4-0ee54d60730b")]
public class EnumerableData
{
    EnumerableData() { }

    public EnumerableData(IEnumerable<Data> theData)
    {
        TheData = theData;
    }

    [DataMember(Order = 1)]
    public IEnumerable<Data> TheData { get; private set; }
}

[DataContract(Name = "1efba909-f0db-4315-b53a-ec80c003544a")]
public sealed class SealedEnumerableData
{
    SealedEnumerableData() { }

    public SealedEnumerableData(IEnumerable<Data> theData)
    {
        TheData = theData;
    }

    [DataMember(Order = 1)]
    public IEnumerable<Data> TheData { get; private set; }
}

[DataContract(Name = "027c02d6-f3f9-4505-aefb-877f9223a230")]
public class ListData : IListData
{
    ListData() { }

    public ListData(List<Data> theData)
    {
        TheData = theData;
    }

    [DataMember(Order = 1)]
    public List<Data> TheData { get; private set; }
}

[DataContract(Name = "c54445d2-7977-4230-ab42-9a499a3d858d")]
public sealed class SealedListData
{
    SealedListData() { }

    public SealedListData(List<Data> theData)
    {
        TheData = theData;
    }

    [DataMember(Order = 1)]
    public List<Data> TheData { get; private set; }
}

[DataContract(Name = "939712ef-73fe-447a-ab7d-520c89041e8c")]
public class ArrayData
{
    ArrayData() { }

    public ArrayData(Data[] theData)
    {
        TheData = theData;
    }

    [DataMember(Order = 1)]
    public Data[] TheData { get; private set; }
}

[DataContract(Name = "79854c35-624d-4b30-83f0-9469b2a807df")]
public sealed class SealedArrayData
{
    SealedArrayData() { }

    public SealedArrayData(Data[] theData)
    {
        TheData = theData;
    }

    [DataMember(Order = 1)]
    public Data[] TheData { get; private set; }
}

[DataContract(Name = "7c171b36-5a9a-40da-9fce-1bd482288b54")]
public record class RecordData
{
    [DataMember(Order = 1)]
    public List<Data> TheData { get; init; }
}


[DataContract(Name = "f0435607-cf90-4b04-8f5e-d68f24268e97")]
public class Data
{
    Data() { }

    public Data(int data)
    {
        Text = data.ToString();
        Number = data;
    }

    [DataMember(Order = 1)]
    public string Text { get; private set; }

    [DataMember(Order = 2)]
    public int Number { get; private set; }
}
