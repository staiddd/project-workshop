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
        // TODO (етап TDD Red): реалізувати перейменування змінної для C++ коду.
        return sourceCode;
    }
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
    public string Apply(string sourceCode, string selectedBlock, string newMethodName)
    {
        // TODO (етап TDD Red): реалізувати виділення методу для C++ коду.
        return sourceCode;
    }
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