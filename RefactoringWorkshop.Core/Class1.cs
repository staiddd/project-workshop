namespace RefactoringWorkshop.Core;

/// <summary>
/// Lab variant implemented in this repository.
/// </summary>
public enum RefactoringVariant
{
    RenameVariable = 1,
    ExtractMethod = 4 // <-- новый вариант
}

/// <summary>
/// Common request model used by the entry point.
/// </summary>
public sealed class RefactoringRequest
{
    public RefactoringRequest(
        string sourceCode,
        string oldName = "",
        string newName = "",
        string selectedBlock = "",
        string newMethodName = "")
    {
        SourceCode = sourceCode;
        OldName = oldName;
        NewName = newName;
        SelectedBlock = selectedBlock;
        NewMethodName = newMethodName;
    }

    public string SourceCode { get; }
    public string OldName { get; }
    public string NewName { get; }
    public string SelectedBlock { get; }
    public string NewMethodName { get; }
}

public interface IRenameVariableRefactoring
{
    string Apply(string sourceCode, string oldName, string newName);
}

public sealed class RenameVariableRefactoring : IRenameVariableRefactoring
{
    public string Apply(string sourceCode, string oldName, string newName)
    {
        // TODO (TDD/Red stage): implement variable rename for C++ source.
        return sourceCode;
    }
}

/// <summary>
/// Extract Method refactoring interface + stub.
/// </summary>
public interface IExtractMethodRefactoring
{
    string Apply(string sourceCode, string selectedBlock, string newMethodName);
}

public sealed class ExtractMethodRefactoring : IExtractMethodRefactoring
{
    public string Apply(string sourceCode, string selectedBlock, string newMethodName)
    {
        // TODO (TDD/Red stage): implement extract method for C++ source.
        return sourceCode;
    }
}

/// <summary>
/// Orchestrator used by UI entry point.
/// </summary>
public sealed class RefactoringEngine
{
    private readonly IRenameVariableRefactoring _renameVariableRefactoring;
    private readonly IExtractMethodRefactoring _extractMethodRefactoring;

    public RefactoringEngine(
        IRenameVariableRefactoring? renameVariableRefactoring = null,
        IExtractMethodRefactoring? extractMethodRefactoring = null)
    {
        _renameVariableRefactoring = renameVariableRefactoring ?? new RenameVariableRefactoring();
        _extractMethodRefactoring = extractMethodRefactoring ?? new ExtractMethodRefactoring();
    }

    public string Apply(RefactoringVariant variant, RefactoringRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return variant switch
        {
            RefactoringVariant.RenameVariable => _renameVariableRefactoring.Apply(
                request.SourceCode,
                request.OldName,
                request.NewName),
            RefactoringVariant.ExtractMethod => _extractMethodRefactoring.Apply(
                request.SourceCode,
                request.SelectedBlock,
                request.NewMethodName),
            _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, "Unsupported refactoring variant.")
        };
    }
}