namespace RefactoringWorkshop.Core;

/// <summary>
/// Варіанти рефакторингу, доступні в цьому прототипі.
/// </summary>
public enum RefactoringVariant
{
    RenameVariable = 1,
    ExtractMethod = 4,
    RenameMethod = 8
}

/// <summary>
/// Універсальна модель запиту, яку формує точка входу.
/// </summary>
public sealed class RefactoringRequest
{
    public RefactoringRequest(
        RefactoringVariant variant,
        string sourceCode,
        string oldName = "",
        string newName = "",
        string selectedBlock = "",
        string newMethodName = "",
        string oldMethodName = "")
    {
        Variant = variant;
        SourceCode = sourceCode;
        OldName = oldName;
        NewName = newName;
        SelectedBlock = selectedBlock;
        NewMethodName = newMethodName;
        OldMethodName = oldMethodName;
    }

    public RefactoringVariant Variant { get; }
    public string SourceCode { get; }
    public string OldName { get; }
    public string NewName { get; }
    public string SelectedBlock { get; }
    public string NewMethodName { get; }
    public string OldMethodName { get; }

}

public interface IRenameVariableRefactoring
{
    string Apply(string sourceCode, string oldName, string newName);
}

public sealed class RenameVariableRefactoring : IRenameVariableRefactoring
{
    public string Apply(string sourceCode, string oldName, string newName)
    {
        if (string.IsNullOrEmpty(oldName) || oldName == newName)
            return sourceCode;

        var sb = new System.Text.StringBuilder(sourceCode.Length);
        var i = 0;

        while (i < sourceCode.Length)
        {
            // Line comment //
            if (i + 1 < sourceCode.Length && sourceCode[i] == '/' && sourceCode[i + 1] == '/')
            {
                sb.Append(sourceCode[i++]);
                sb.Append(sourceCode[i++]);
                while (i < sourceCode.Length && sourceCode[i] != '\n' && sourceCode[i] != '\r')
                    sb.Append(sourceCode[i++]);
                continue;
            }

            // Block comment /* */
            if (i + 1 < sourceCode.Length && sourceCode[i] == '/' && sourceCode[i + 1] == '*')
            {
                sb.Append(sourceCode[i++]);
                sb.Append(sourceCode[i++]);
                while (i < sourceCode.Length)
                {
                    if (i + 1 < sourceCode.Length && sourceCode[i] == '*' && sourceCode[i + 1] == '/')
                    {
                        sb.Append(sourceCode[i++]);
                        sb.Append(sourceCode[i++]);
                        break;
                    }
                    sb.Append(sourceCode[i++]);
                }
                continue;
            }

            // String literal "..."
            if (sourceCode[i] == '"')
            {
                sb.Append(sourceCode[i++]);
                while (i < sourceCode.Length)
                {
                    var c = sourceCode[i++];
                    sb.Append(c);
                    if (c == '\\' && i < sourceCode.Length)
                        sb.Append(sourceCode[i++]);
                    else if (c == '"')
                        break;
                }
                continue;
            }

            // Character literal 'x' or '\n'
            if (sourceCode[i] == '\'')
            {
                sb.Append(sourceCode[i++]);
                while (i < sourceCode.Length)
                {
                    var c = sourceCode[i++];
                    sb.Append(c);
                    if (c == '\\' && i < sourceCode.Length)
                        sb.Append(sourceCode[i++]);
                    else if (c == '\'')
                        break;
                }
                continue;
            }

            if (IsIdentifierStart(sourceCode[i]))
            {
                var start = i;
                i++;
                while (i < sourceCode.Length && IsIdentifierChar(sourceCode[i]))
                    i++;

                var len = i - start;
                if (len == oldName.Length && string.CompareOrdinal(sourceCode, start, oldName, 0, len) == 0)
                    sb.Append(newName);
                else
                    sb.Append(sourceCode, start, len);

                continue;
            }

            sb.Append(sourceCode[i++]);
        }

        return sb.ToString();
    }

    private static bool IsIdentifierStart(char c) =>
        (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';

    private static bool IsIdentifierChar(char c) =>
        IsIdentifierStart(c) || (c >= '0' && c <= '9');
}

/// <summary>
/// Контракт і заглушка для рефакторингу "Виділення методу".
/// </summary>
public interface IExtractMethodRefactoring
{
    string Apply(string sourceCode, string selectedBlock, string newMethodName);
}

public sealed class ExtractMethodRefactoring : IExtractMethodRefactoring
{
    private static readonly HashSet<string> CppKeywords = new(StringComparer.Ordinal)
    {
        "auto", "break", "case", "catch", "class", "const", "continue", "default",
        "delete", "do", "double", "else", "enum", "explicit", "extern", "false",
        "float", "for", "friend", "goto", "if", "inline", "int", "long",
        "mutable", "namespace", "new", "nullptr", "operator", "private", "protected",
        "public", "register", "return", "short", "signed", "sizeof", "static",
        "struct", "switch", "template", "this", "throw", "true", "try", "typedef",
        "typename", "union", "unsigned", "using", "virtual", "void", "volatile",
        "while", "bool", "char", "std", "string"
    };

    public string Apply(string sourceCode, string selectedBlock, string newMethodName)
    {
        var blockIndex = sourceCode.IndexOf(selectedBlock, StringComparison.Ordinal);
        if (blockIndex < 0)
            return sourceCode;

        var blockIdentifiers = ExtractIdentifiersInOrder(selectedBlock);
        var functionNamesInBlock = ExtractFunctionNames(selectedBlock);

        var outsideSource = sourceCode.Remove(blockIndex, selectedBlock.Length);
        var outsideIdentifiers = new HashSet<string>(ExtractIdentifiersInOrder(outsideSource));

        var parameters = blockIdentifiers
            .Where(id => !CppKeywords.Contains(id)
                      && !functionNamesInBlock.Contains(id)
                      && outsideIdentifiers.Contains(id))
            .ToList();

        var call = $"{newMethodName}({string.Join(", ", parameters)});";

        return string.Concat(
            sourceCode.AsSpan(0, blockIndex),
            call,
            sourceCode.AsSpan(blockIndex + selectedBlock.Length));
    }

    private static List<string> ExtractIdentifiersInOrder(string code)
    {
        var result = new List<string>();
        var seen = new HashSet<string>();
        var i = 0;
        while (i < code.Length)
        {
            if (IsIdentifierStart(code[i]))
            {
                var start = i++;
                while (i < code.Length && IsIdentifierChar(code[i])) i++;
                var id = code.Substring(start, i - start);
                if (seen.Add(id)) result.Add(id);
            }
            else i++;
        }
        return result;
    }

    private static HashSet<string> ExtractFunctionNames(string code)
    {
        var result = new HashSet<string>();
        var i = 0;
        while (i < code.Length)
        {
            if (IsIdentifierStart(code[i]))
            {
                var start = i++;
                while (i < code.Length && IsIdentifierChar(code[i])) i++;
                var id = code.Substring(start, i - start);
                var j = i;
                while (j < code.Length && code[j] == ' ') j++;
                if (j < code.Length && code[j] == '(')
                    result.Add(id);
            }
            else i++;
        }
        return result;
    }

    private static bool IsIdentifierStart(char c) =>
        (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';

    private static bool IsIdentifierChar(char c) =>
        IsIdentifierStart(c) || (c >= '0' && c <= '9');
}




public interface IRenameMethodRefactoring
{
    string Apply(string sourceCode, string oldMethodName, string newMethodName);
}


public sealed class RenameMethodRefactoring : IRenameMethodRefactoring
{
    public string Apply(string sourceCode, string oldMethodName, string newMethodName)
    {
        // TODO (етап TDD Red): реалізувати перейменування методу для C++ коду.
        return sourceCode;
    }
}



/// <summary>
/// Оркестратор, який викликає потрібний рефакторинг із точки входу.
/// </summary>
public sealed class RefactoringEngine
{
    private readonly IRenameVariableRefactoring _renameVariableRefactoring;
    private readonly IExtractMethodRefactoring _extractMethodRefactoring;
    private readonly IRenameMethodRefactoring _renameMethodRefactoring;

    public RefactoringEngine(
        IRenameVariableRefactoring? renameVariableRefactoring = null,
        IExtractMethodRefactoring? extractMethodRefactoring = null,
        IRenameMethodRefactoring? renameMethodRefactoring = null)
    {
        _renameVariableRefactoring = renameVariableRefactoring ?? new RenameVariableRefactoring();
        _extractMethodRefactoring = extractMethodRefactoring ?? new ExtractMethodRefactoring();
        _renameMethodRefactoring = renameMethodRefactoring ?? new RenameMethodRefactoring();

    }

    public string Apply(RefactoringRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return request.Variant switch
        {
            RefactoringVariant.RenameVariable => _renameVariableRefactoring.Apply(
                request.SourceCode,
                request.OldName,
                request.NewName),
            RefactoringVariant.RenameMethod => _renameMethodRefactoring.Apply(
                request.SourceCode,
                request.OldMethodName,
                request.NewMethodName),
            RefactoringVariant.ExtractMethod => _extractMethodRefactoring.Apply(
                request.SourceCode,
                request.SelectedBlock,
                request.NewMethodName),
            _ => throw new ArgumentOutOfRangeException(nameof(request.Variant), request.Variant, "Unsupported refactoring variant.")
        };
    }
}