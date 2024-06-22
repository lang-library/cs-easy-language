using Jint;
using System.IO;
using System.Reflection;
using Global;
namespace Global;
public class JintScript
{
    public static Jint.Engine CreateEngine(params Assembly[] list)
    {
        var engine = new Jint.Engine(cfg =>
        {
            cfg.AllowClr();
            for (int i = 0; i < list.Length; i++)
            {
                cfg.AllowClr(list[i]);
            }
        });
        engine.SetValue("_globals", new JintScriptGlobal());
        engine.Execute("""
            var print = _globals.print;
            var log = _globals.log;
            var getenv = _globals.getenv;
            var appFile = _globals.appFile;
            var appDir = _globals.appDir;
            var $ns = importNamespace;
            /*
            function $ns(name)
            {
              return importNamespace(name);
            }
            */
            
            """);
        return engine;
    }
}
internal class JintScriptGlobal
{
    public void print(dynamic x, string? title = null)
    {
        ELang.Echo(x, title);
    }
    public void log(dynamic x, string? title = null)
    {
        ELang.Log(x, title);
    }
    public string getenv(string name)
    {
        return System.Environment.GetEnvironmentVariable(name);
    }
    public string appFile()
    {
        return Assembly.GetExecutingAssembly().Location;
    }
    public string appDir()
    {
        return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
