Echo('this is test.js');

function transpile(ast)
{
    ast = FromObject(ast);
    var sb = new StringBuilder();
    transpileBody(ast, sb);
    return sb.ToString();
}

function transpileBody(ast, sb)
{
    Echo(ast, "ast");
    let type = gettype(ast);
    Echo(type, "type");
    switch (type) {
        case "list":
            transpileList(ast, sb);
            return;
        default:
            throw new Error(`type:${type} is not supported`);
    }
    return "dummy-script";
}

function transpileList(ast, sb) {
    Echo(ast.Count);
    if (ast.Count == 0) throw new Error("list length is 0");
    transpileFunCall(ast, sb);
    sb.Append("<<list>>");
}

function transpileFunCall(ast, sb) {
    let first = ast[0];
    Echo(first, "first");
    first = transpieFunName(first);
    Echo(first, "first");
}

function transpieFunName(ast, sb) {
    let type = gettype(ast);
    if (type == "string") return ast;
    if (type == "list") {
        if (ast.Count == 0) throw new Error("dot notation length is 0");
        let first = ast[0];
        Echo(first, "first(dot notation)");
        let result = "";
        for (let i = 0; i < ast.Count; i++) {
            result += ast[i];
        }
        return result;
    }
}

function isArray(value) {
    //var V8ArrayeT = host.type("Microsoft.ClearScript.V8.V8ScriptItem.V8Arraye");
    //return host.isType(V8ArrayeT, value);
    let fullName = FullName(value);
    Echo(fullName);
    return fullName == "Microsoft.ClearScript.V8.V8ScriptItem+V8Array";
}
