Echo('this is test.js');

function transpile(ast)
{
    Echo(globalThis['JSON'].stringify(11));
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
        case "number":
            sb.Append(ast);
            return;
        case "string":
            sb.Append(ast);
            return;
        case "as-is":
            sb.Append(ast["?"].trim());
            return;
        case "quote":
            sb.Append(JSON.stringify(ast["?"]));
            return;
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
}

function transpileFunCall(ast, sb) {
    let first = ast[0];
    Echo(first, "first");
    first = transpieFunName(first);
    Echo(first, "first");
    if (first in funcList) {
        funcList[first](ast, sb);
        return;
    }
    sb.Append(first);
    sb.Append("(");
    for (let i = 1; i < ast.Count; i++) {
        if (i > 1) sb.Append(",");
        transpileBody(ast[i], sb);
    }
    sb.Append(")");
    //sb.Append(";");
}

function transpieFunName(ast, sb) {
    let type = gettype(ast);
    if (type == "string") return ast;
    if (type == "list") {
        if (ast.Count == 0) throw new Error("dot notation length is 0");
        let first = ast[0];
        Echo(first, "first(dot notation)");
        let result = "";
        for (let i = 1; i < ast.Count; i++) {
            if (i > 1) result += ".";
            result += ast[i];
        }
        return result;
    }
}

var funcList = {
    "program": function (ast, sb) {
        for (let i = 1; i < ast.Count; i++) {
            transpileBody(ast[i], sb);
            sb.Append(";");
        }
    },
    "define": function (ast, sb) {
        sb.Append("var ");
        sb.Append(ast[1]);
        sb.Append("=");
        transpileBody(ast[2], sb);
    }
};

function add(a, b) {
    return a + b;
}
