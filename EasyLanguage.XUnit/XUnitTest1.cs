using Xunit;
using Xunit.Abstractions;
using Global;

public class XUnitTest1
{
    private readonly ITestOutputHelper Out;
    public XUnitTest1(ITestOutputHelper testOutputHelper)
    {
        Out = testOutputHelper;
        Print("Setup() called");
    }
    private void Print(object x, string title = null)
    {
        Out.WriteLine(ELang.ToPrintable(x, title));
    }
    [Fact]
    public void Test01()
    {
        var el1 = EasyObject.FromJson("""
            { "a": //line comment
              123
              b.c.d: `(777 888) }
            """);
        Print(el1, "el1");
        Assert.Equal("""
            {"a":123,"b.c.d":["quote",[777,888]]}
            """, el1.ToJson());
        var el2 = EasyObject.FromJson("""
            (add2 777 888) }
            """);
        Print(el2, "el2");
        Assert.Equal("""
            ["add2",777,888]
            """, el2.ToJson());
        var el3 = EasyObject.FromJson("""
            @_.!$%&-=^~+*<>/?
            """);
        Print(el3, "el3");
        Assert.Equal("""
            "_.!$%&-=^~+*<>/?"
            """, el3.ToJson());
    }
}
