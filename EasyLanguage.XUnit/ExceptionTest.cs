using Xunit;
using Xunit.Abstractions;
//using MyJson;
//using static MyJson.MyData;
using System;
using Global;

public class ExceptionTest
{
    private readonly ITestOutputHelper Out;
    public ExceptionTest(ITestOutputHelper testOutputHelper)
    {
        Out = testOutputHelper;
       EasyObject.ClearSettings();
        Print("Setup() called");
    }
    private void Print(object x, string title = null)
    {
        Out.WriteLine(EasyObject.ToPrintable(x, title));
    }
    [Fact]
    public void Test01()
    {
    }
}
