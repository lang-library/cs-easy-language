using System;
using System.Collections.Generic;
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
            var args = exp.AsList.GetRange(1, exp.AsList.Count - 1);
            return new EasyObject(args);
        }
        throw new Exception($"EasyObjectType.@{exp.TypeName} is not supported.");
    }
    public EasyObject EvalString(string expText)
    {
        EasyObject exp = EasyObject.FromJson(expText);
        return Eval(exp);
    }
}
