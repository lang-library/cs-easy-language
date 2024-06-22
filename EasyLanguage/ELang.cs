using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Global;

public class ELang
{
    public static bool DebugOutput = false;
    public static bool ShowDetail = false;
    //public static bool ForceASCII = false;
    public static object FromJson(string json)
    {
        return new CSharpEasyLanguageHandler(true, false).Parse(json);
    }
    public static object FromObject(object x)
    {
        return new ObjectParser(false).Parse(x);
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
}
