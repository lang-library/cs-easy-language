﻿using Global;
using System;
using System.IO;
using static Global.EasyObject;
namespace Main;


static class Program
{
    [STAThread]
    static void Main(string[] originalArgs)
    {
        var el1 = EasyObject.FromJson("""
            { "a": //line comment
              123
              b: `(777 888) }
            """);
        Echo(el1, "el1");
        var el2 = EasyObject.FromJson("""
            { "a": //line comment
              123
              b: `(add2 777 888) }
            """);
        Echo(el2, "el2");
        var el3 = Global.EasyLanguageParser.Parse("""
            { "a": //line comment
              123
              b: `(add2 777 888)
              x: (1+ 123)
              y: _!%&-=+*/?
              z: :keyword-_!%&-=+*/? }
            """);
        Echo(el3, "el3");
        var elang = new EasyLanguage();
#if false
        var ans01 = elang.EvalString("""
            (@+ 11 22)
            """);
        Echo(ans01, "ans01");
#endif
        var ans02 = elang.EvalFile("assets/test.elAng");
        Echo(ans02, "ans02");
        var csharp = new ELang.CSharpScript();
        var answer = csharp.Evaluate("11+22");
        Echo(answer, "answer");
        var order = EasyObject.FromJson("{b: 123, a: 456, _:789}");
        Echo(order, "order");
        var interp = NukataLisp.MakeInterp().Result;
        interp.Def("add2", 2, a => {
            var x = Convert.ToDecimal(a[0]);
            Echo(FullName(x), "FullName(x)");
            var y = Convert.ToDecimal(a[1]);
            Echo(FullName(y), "FullName(y)");
            return x + y;
        });
        StringReader sr = new StringReader("(print (add2 11 22))");
        var r = NukataLisp.Run(interp, sr).Result;
        Echo(r, "r");
    }
}