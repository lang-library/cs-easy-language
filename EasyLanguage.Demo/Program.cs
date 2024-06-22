using Global;
using System;
using System.IO;
using static Global.EasyObject;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;

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
#if false
        var csharp = new ELang.CSharpScript();
        var answer = csharp.Evaluate("11+22");
        Echo(answer, "answer");
#endif
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

        using (var engine = new V8ScriptEngine())

        {
            // expose a host type
            engine.AddHostType("Console", typeof(Console));
            engine.Execute("Console.WriteLine('{0} is an interesting number.', Math.PI)");

            // expose a host object
            engine.AddHostObject("random", new Random());
            engine.Execute("Console.WriteLine(random.NextDouble())");

            // expose entire assemblies
            engine.AddHostObject("lib", new HostTypeCollection("mscorlib", "System.Core"));
            engine.Execute("Console.WriteLine(lib.System.DateTime.Now)");

            // create a host object from script
            engine.Execute(@"
        birthday = new lib.System.DateTime(2007, 5, 22);
        Console.WriteLine(birthday.ToLongDateString());
    ");

            // use a generic class from script
            engine.Execute(@"
        Dictionary = lib.System.Collections.Generic.Dictionary;
        dict = new Dictionary(lib.System.String, lib.System.Int32);
        dict.Add('foo', 123);
    ");

            // call a host method with an output parameter
            engine.AddHostObject("host", new ExtendedHostFunctions());
            engine.Execute(@"
        intVar = host.newVar(lib.System.Int32);
        found = dict.TryGetValue('foo', intVar.out);
        Console.WriteLine('{0} {1}', found, intVar);
    ");

            // create and populate a host array
            engine.Execute(@"
        numbers = host.newArr(lib.System.Int32, 20);
        for (var i = 0; i < numbers.Length; i++) { numbers[i] = i; }
        Console.WriteLine(lib.System.String.Join(', ', numbers));
    ");

            // create a script delegate
            engine.Execute(@"
        Filter = lib.System.Func(lib.System.Int32, lib.System.Boolean);
        oddFilter = new Filter(function(value) {
            return (value & 1) ? true : false;
        });
    ");

            // use LINQ from script
            engine.Execute(@"
        oddNumbers = numbers.Where(oddFilter);
        Console.WriteLine(lib.System.String.Join(', ', oddNumbers));
    ");

            // use a dynamic host object
            engine.Execute(@"
        expando = new lib.System.Dynamic.ExpandoObject();
        expando.foo = 123;
        expando.bar = 'qux';
        delete expando.foo;
    ");

            // call a script function
            engine.Execute("function print(x) { Console.WriteLine(x); }");
            engine.Script.print(DateTime.Now.DayOfWeek);

            // examine a script object
            engine.Execute("person = { name: 'Fred', age: 5 }");
            Console.WriteLine(engine.Script.person.name);

            // read a JavaScript typed array
            engine.Execute("values = new Int32Array([1, 2, 3, 4, 5])");
            var values = (ITypedArray<int>)engine.Script.values;
            Console.WriteLine(string.Join(", ", values.ToArray()));

            // EasyObject
            //engine.AddHostType("EasyObject", typeof(EasyObject));
            engine.AddHostType(typeof(EasyObject));
            var eo = EasyObject.FromObject(new []  { 1, 2, 3 });
            engine.AddHostObject("eo", eo);
            engine.Execute("""
                function echo(msg, title = null) { EasyObject.Echo(msg, title); }
                EasyObject.Echo(eo, "eo");
                EasyObject.Echo(eo[1], "eo[1]");
                echo(eo[1]);
                var Int32T = host.type("System.Int32");
                var intValue = host.cast(Int32T, eo[1]);
                echo(intValue, "intValue");
                var intValue2 = host.cast(lib.System.Int32, eo[2]);
                echo(intValue2, "intValue2");
                """);
            // expose entire assemblies
            engine.AddHostObject("lib2", new HostTypeCollection(typeof(EasyObject).Assembly));
            engine.Execute("lib2.Global.EasyObject.Echo('from lib2')");
        }
    }
}