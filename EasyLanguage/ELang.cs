using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;

namespace Global;

internal class _ELangConverter : IObjectConverter
{
    public object ConvertResult(object x, string origTypeName)
    {
        //ELang.Echo(ELang.FullName(x));
        //ELang.Echo(origTypeName);
        string fullName = ELang.FullName(x);
        if (origTypeName == "Microsoft.ClearScript.V8.V8ScriptItem+V8ScriptObject")
        {
            //ELang.Echo("found");
            //ELang.Echo(x);
#if true
            var result = new PropertyBag();
            foreach (object o in (dynamic)x)
            {
                //ELang.Echo(ELang.FullName(o));
                if (o is Microsoft.ClearScript.PropertyBag dict)
                {
                    ELang.Echo(dict["Key"]);
                    //result[(string)dict["Key"]] = dict["Value"];
                    result.Add((string)dict["Key"], dict["Value"]);
                }
            }
            return result;
#else
            var result = new Dictionary<string, object>();
            foreach (object o in (dynamic)x)
            {
                //ELang.Echo(ELang.FullName(o));
                if (o is Microsoft.ClearScript.PropertyBag dict)
                {
                    ELang.Echo(dict["Key"]);
                    result.Add((string)dict["Key"], dict["Value"]);
                }
            }
            return result;
#endif
        }
        else if (x is System.Collections.Generic.Dictionary<string, object> dict)
        {
            var result = new PropertyBag();
            var keys = dict.Keys;
            foreach (string key in keys)
            {
                result[key] = dict[key];
            }
            return result;
        }
        return x;
    }
}

public class ELang
{
    public static bool DebugOutput = false;
    public static bool ShowDetail = false;
    //public static bool ForceASCII = false;
    public static object FromJson(string json)
    {
        return new CSharpEasyLanguageHandler(true, false).Parse(json);
    }
    public static object FromCode(string code)
    {
        object obj = FromJson(code);
        Echo(obj);
        return TransformToAST(obj);
    }
    public static object FromObject(object x)
    {
        // Microsoft.ClearScript.V8.V8ScriptItem+V8ScriptObject
        return new ObjectParser(false, new _ELangConverter()).Parse(x);
    }
    public static string ToJson(object x, bool indent = false)
    {
        return new ObjectParser(false).Stringify(x, indent);
    }
    public static List<object> NewList(params object[] objects)
    {
        return new List<object>(objects);
    }
    public static string FullName(dynamic x)
    {
        if (x is null) return "null";
        string fullName = ((object)x).GetType().FullName;
        return fullName.Split('`')[0];
    }
    public static string ToPrintable(object x, string title = null)
    {
        return ObjectParser.ToPrintable(ShowDetail, x, title);
    }
    public static void Echo(object x, string title = null)
    {
        String s = ToPrintable(x, title);
        Console.WriteLine(s);
        System.Diagnostics.Debug.WriteLine(s);
    }
    public static void Log(object x, string? title = null)
    {
        String s = ToPrintable(x, title);
        Console.Error.WriteLine("[Log] " + s);
        System.Diagnostics.Debug.WriteLine("[Log] " + s);
    }
    public static void Debug(object x, string? title = null)
    {
        if (!DebugOutput) return;
        String s = ToPrintable(x, title);
        Console.Error.WriteLine("[Debug] " + s);
        System.Diagnostics.Debug.WriteLine("[Debug] " + s);
    }
    public static void Message(object x, string? title = null)
    {
        if (title == null) title = "Message";
        String s = ToPrintable(x, null);
        NativeMethods.MessageBoxW(IntPtr.Zero, s, title, 0);
    }
    internal static class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int MessageBoxW(
            IntPtr hWnd, string lpText, string lpCaption, uint uType);
    }
    public static object TransformToAST(object x)
    {
        if (x == null) return null;
        else if (x is decimal)
        {
            return x;
        }
        else if (x is string)
        {
            var result = new Dictionary<string, object>();
            result["!"] = "quote";
            result["?"] = x;
            return result;
        }
        else if (x is List<object> list)
        {
            var result = new List<object>();
            foreach(var e in list)
            {
                result.Add(TransformToAST(e));
            }
            return result;
        }
        else if (x is Dictionary<string, object> dict)
        {
            if (dict.ContainsKey("!"))
            {
                var type = dict["!"];
                if (type is string)
                {
                    switch (type)
                    {
                        case "quote":
                            return x;
                        case "dot":
                            return ".";
                        case "symbol":
                            return dict["?"];
                        default:
                            //throw new Exception($"{type} is not expected");
                            break;
                    }
                }
                else
                {
                    throw new Exception($"type is string");
                }
            }
            var result = new Dictionary<string, object>();
            foreach (var key in dict.Keys)
            {
                result[key] = TransformToAST(dict[key]);
            }
            return result;
        }
        else
        {
            throw new Exception($"{FullName(x)} is not supported");
        }
    }
}
