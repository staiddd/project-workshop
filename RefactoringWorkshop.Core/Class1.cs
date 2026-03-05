namespace RefactoringWorkshop.Core;

/// <summary>
/// Lab variant implemented in this repository.
/// </summary>
public enum RefactoringVariant
{
    RenameVariable = 1
}

/// <summary>
/// Common request model used by the entry point.
/// </summary>
public sealed class RefactoringRequest
{
    public RefactoringRequest(
        string sourceCode,
        string oldName,
        string newName)
    {
        SourceCode = sourceCode;
        OldName = oldName;
        NewName = newName;
    }

    public string SourceCode { get; }

    public string OldName { get; }

    public string NewName { get; }
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
/// Orchestrator used by UI entry point.
/// </summary>
public sealed class RefactoringEngine
{
    private readonly IRenameVariableRefactoring _renameVariableRefactoring;

    public RefactoringEngine(IRenameVariableRefactoring? renameVariableRefactoring = null)
    {
        _renameVariableRefactoring = renameVariableRefactoring ?? new RenameVariableRefactoring();
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
            _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, "Unsupported refactoring variant.")
        };
    }
}
