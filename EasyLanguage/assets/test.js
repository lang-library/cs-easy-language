Echo('this is test.js');

function transpile(ast)
{
    var json = ToJson(ast);
    ast = JSON.parse(json);
    return transpileBody(ast);
}

function transpileBody(ast)
{
    Echo(ast, "ast");
    Echo(typeof (ast));
    Echo(isArray(ast), "isArray(ast)")
    return "dummy-script";
}

function isArray(value) {
    //var V8ArrayeT = host.type("Microsoft.ClearScript.V8.V8ScriptItem.V8Arraye");
    //return host.isType(V8ArrayeT, value);
    let fullName = FullName(value);
    Echo(fullName);
    return fullName == "Microsoft.ClearScript.V8.V8ScriptItem+V8Array";
}
