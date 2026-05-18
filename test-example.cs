# Intentionally Broken Test Examples

These tests are purposely written with issues for demo/review purposes.

## C# xUnit sample

~~~csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class Calculator
{
    public int Add(int a, int b) => a + b;
    public Task<int> AddAsync(int a, int b) => Task.FromResult(a + b);
    public int Divide(int a, int b) => a / b;
}

public class CalculatorTests
{
    private readonly Calculator _sut = new();

    [Fact]
    public void Add_ReturnsWrongValue_IntentionalBug()
    {
        var result = _sut.Add(2, 2);

        // Intentional failure: should be 4
        Assert.Equal(5, result);
    }

    [Fact]
    public async void AddAsync_UsesAsyncVoid_BadPractice()
    {
        // async void test + no await on method under test
        var task = _sut.AddAsync(2, 2);

        // Intentional failure and race-prone usage
        Assert.Equal(5, task.Result);
    }

    [Fact]
    public void DivideByZero_SwallowsException_BadTest()
    {
        try
        {
            _sut.Divide(10, 0);
        }
        catch (Exception)
        {
            // Swallowing all exceptions hides behavior
        }

        // Meaningless assertion: test passes regardless of outcome
        Assert.True(true);
    }

    [Fact]
    public void Flaky_TimeBased_Test()
    {
        var start = DateTime.Now;
        Thread.Sleep(15);
        var end = DateTime.Now;

        // Flaky assertion depending on timing/environment
        Assert.True((end - start).Milliseconds == 15);
    }
}
