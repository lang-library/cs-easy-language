Echo('this is gettype.js');

function gettype(x)
{
    let fullName = FullName(x);
    Echo(fullName);
    switch (fullName) {
        case "System.String":
            return "string";
        case "Microsoft.ClearScript.V8.V8ScriptItem+V8Array":
            return "array";
        case "System.Collections.Generic.List":
            return "list";
        case "Microsoft.ClearScript.PropertyBag": {
            if ('!' in x) {
                Echo("contains!");
            }
            return "bag";
        }
        default:
            throw new Error(`${fullName} is not supported`);
    }
}