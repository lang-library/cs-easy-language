using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using static Global.EasyObject;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        Console.WriteLine("Setup() called");
        ClearSettings();
    }

    [Test]
    public void Test01()
    {
        var el1 = EasyObject.FromJson("""
            { "a": //line comment
              123
              b: `(777 888) }
            """);
        Echo(el1, "el1");
        Assert.That(el1.ToJson(), Is.EqualTo("""
            {"a":123,"b":{"!":"quasi-quote","?":[777,888]}}
            """));
        var el2 = EasyObject.FromJson("""
            (add2 777 888) }
            """);
        Echo(el2, "el2");
        Assert.That(el2.ToJson(), Is.EqualTo("""
            [{"!":"symbol","?":"add2"},777,888]
            """));
        var el3 = EasyObject.FromJson("""
            @_!%&-=+*<>/?
            """);
        Echo(el3, "el3");
        Assert.That(el3.ToJson(), Is.EqualTo("""
            {"!":"deref","?":{"!":"symbol","?":"_!%&-=+*<>/?"}}
            """));
        var el4 = EasyObject.FromJson("""
             { "a": //line comment
                123
                b: `(add2 777 888)
                x: (@1+ 123)
                y: @_!%&-=+*<>/? }
             """);
        Echo(el4.ToJson(), "el4");
        //Echo(el4.ToJson(), "el4.ToJson()");
        Assert.That(el4.ToJson(), Is.EqualTo("""
            {"a":123,"b":{"!":"quasi-quote","?":[{"!":"symbol","?":"add2"},777,888]},"x":[{"!":"deref","?":{"!":"symbol","?":"1+"}},123],"y":{"!":"deref","?":{"!":"symbol","?":"_!%&-=+*<>/?"}}}
            """));
    }
}