using Xunit;
using Xunit.Abstractions;
//using MyJson;
//using static MyJson.MyData;
using static Global.SharpJson;
using System;
using Global;

public class ExceptionTest
{
    private readonly ITestOutputHelper Out;
    public ExceptionTest(ITestOutputHelper testOutputHelper)
    {
        Out = testOutputHelper;
        SharpJson.ClearAllSettings();
        Print("Setup() called");
    }
    private void Print(object x, string title = null)
    {
        Out.WriteLine(SharpJson.ToPrintable(x, title));
    }
    [Fact]
    public void Test01()
    {
    }
}
