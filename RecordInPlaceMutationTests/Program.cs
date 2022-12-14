using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Benchmark>();

record TestRecord(string Name, int Age);

class TestClass
{
    public string Name { get; set; }
    public int Age { get; set; }
}

[MemoryDiagnoser()]
public class Benchmark
{
    [Params(1000, 10_000, 100_000)]
    public int ItemCount { get; set; }

    [Benchmark()]
    public void CopyMultipleRecords()
    {
        Enumerable.Range(0, ItemCount).Select(i => new TestRecord(i.ToString(), i))
            .Select(r => r with { Age = r.Age + 5 })
            .ToArray();
    }
    
    [Benchmark()]
    public void ModifyClasses()
    {
        Enumerable.Range(0, ItemCount).Select(i => new TestClass{Name = i.ToString(), Age = i})
            .Select(r =>
            {
                r.Age = r.Age + 5;
                return r;
            })
            .ToArray();
    }

    [Benchmark]
    public void MultipleModificationsInARow_Record()
    {
        var r = new TestRecord("name", 1);
        var r2 = r with { Name = "100" };

        int i = r.Age + 5;

        var r3 = r2 with { Age = 100 };
        int i2 = r3.Age + 100;
    }
    
    
    [Benchmark]
    public void MultipleModificationsInARow_Class()
    {
        var r = new TestClass{Name = "name", Age = 1};
        r.Name = "100";

        int i = r.Age + 5;

        r.Age = 100 ;
        int i2 = r.Age + 100;
    }
}