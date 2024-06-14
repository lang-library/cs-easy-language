using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Global.EasyObject;

namespace Global;

public class EasyLanguage
{
    public EasyLanguage()
    {
    }
    public EasyObject Eval(EasyObject exp)
    {
        Echo(exp.TypeValue, "exp.TypeValue");
        Echo(exp.m_data.GetType().ToString());
        Echo(exp, "exp");
        if (exp.TypeValue == EasyObjectType.@null)
        {
            return EasyObject.Null;
        }
        else if (exp.TypeValue == EasyObjectType.@number)
        {
            return exp;
        }
        else if (exp.TypeValue == EasyObjectType.@array)
        {
            if (exp.Count == 0)
            {
                return EasyObject.EmptyArray;
            }
            var e0 = exp.AsList[0];
            if (!e0.IsString)
            {
                return exp;
            }
            string funcName = (string)e0.Dynamic;
            var args = new List<EasyObject>();
            for (int i = 1; i<exp.Count; i++)
            {
                args.Add(Eval(exp[i]));
            }
            //return new EasyObject(args);
            return EvalFunctionCall(funcName, args);
        }
        throw new Exception($"EasyObjectType.@{exp.TypeName} is not supported.");
    }
    public EasyObject EvalString(string expText)
    {
        Echo(expText, "expText");
        EasyObject exp = EasyObject.FromJson(expText);
        return Eval(exp);
    }
    public EasyObject EvalFile(string expPath)
    {
        string expTest = File.ReadAllText(expPath);
        return EvalString(expTest);
    }
    protected EasyObject EvalFunctionCall(string funcName, List<EasyObject> args)
    {
        switch (funcName)
        {
            case "+":
                return new EasyObject(((decimal)args[0].Dynamic) + ((decimal)args[1].Dynamic));
            default:
                return Null;
        }
    }
}
