Echo('this is test.js');

function transpile(ast)
{
    //var json = ToJson(ast);
    //ast = JSON.parse(json);
    ast = FromObject(ast);
    return transpileBody(ast);
}

function transpileBody(ast)
{
    Echo(ast, "ast");
    Echo(typeof (ast));
    Echo(isArray(ast), "isArray(ast)")
    Echo(gettype(ast), "gettype(ast)")
    Echo(gettype(ast[0]), "gettype(ast[0])")
    Echo(gettype(ast[3][1]), "gettype(ast[3][1])")
    Echo(ast.length, "ast.length");
    Echo(ast.Count, "ast.Count");
    return "dummy-script";
}

function isArray(value) {
    //var V8ArrayeT = host.type("Microsoft.ClearScript.V8.V8ScriptItem.V8Arraye");
    //return host.isType(V8ArrayeT, value);
    let fullName = FullName(value);
    Echo(fullName);
    return fullName == "Microsoft.ClearScript.V8.V8ScriptItem+V8Array";
}
