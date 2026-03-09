using System.Text;
using RefactoringWorkshop.Core;

var engine = new RefactoringEngine();

Console.WriteLine("=== Refactoring Workshop (Lab #1 / TDD Red) ===");
Console.WriteLine("Available variants: 1 - Rename Variable, 4 - Extract Method, 8 - Rename Method");
Console.WriteLine("Type Q to exit.");

while (true)
{
    Console.WriteLine();
    Console.Write("Press 1, 4, 8 to run variant (or Q to exit): ");
    var command = Console.ReadLine()?.Trim();

    if (string.Equals(command, "Q", StringComparison.OrdinalIgnoreCase))
        break;

    if (!string.Equals(command, "1", StringComparison.Ordinal) &&
        !string.Equals(command, "4", StringComparison.Ordinal) &&
        !string.Equals(command, "8", StringComparison.Ordinal))
    {
        Console.WriteLine("Invalid choice. Try 1, 4, 8 or Q.");
        continue;
    }

    var sourceCode = ReadSourceCodeFromConsole();

    RefactoringRequest request = null;

    if (command == "1")
    {
        request = BuildRenameVariableRequest(sourceCode);
    }
    else if (command == "4")
    {
        request = BuildExtractMethodRequest(sourceCode);
    }
    else if (command == "8")
    {
        request = BuildRenameMethodRequest(sourceCode);
    }

    var result = engine.Apply(request);

    Console.WriteLine();
    Console.WriteLine("----- Refactored output -----");
    Console.WriteLine(result);
    Console.WriteLine("-----------------------------");
}

static string ReadSourceCodeFromConsole()
{
    Console.WriteLine();
    Console.WriteLine("Paste C++ code. Type END on a new line to finish input.");
    var builder = new StringBuilder();

    while (true)
    {
        var line = Console.ReadLine();
        if (string.Equals(line, "END", StringComparison.Ordinal))
            break;

        builder.AppendLine(line);
    }

    return builder.ToString();
}

static RefactoringRequest BuildRenameVariableRequest(string sourceCode)
{
    Console.Write("Old variable name: ");
    var oldName = Console.ReadLine() ?? string.Empty;
    Console.Write("New variable name: ");
    var newName = Console.ReadLine() ?? string.Empty;

    return new RefactoringRequest(RefactoringVariant.RenameVariable,  sourceCode, oldName: oldName, newName: newName);
}

static RefactoringRequest BuildExtractMethodRequest(string sourceCode)
{
    Console.Write("Selected block: ");
    var selectedBlock = Console.ReadLine() ?? string.Empty;

    Console.Write("New method name: ");
    var newMethodName = Console.ReadLine() ?? string.Empty;

    return new RefactoringRequest(
        RefactoringVariant.ExtractMethod,
        sourceCode,
        selectedBlock: selectedBlock,
        newMethodName: newMethodName);
}

static RefactoringRequest BuildRenameMethodRequest(string sourceCode)
{
    Console.Write("Old method name: ");
    var oldMethodName = Console.ReadLine() ?? string.Empty;
    Console.Write("New method name: ");
    var newMethodName = Console.ReadLine() ?? string.Empty;

    return new RefactoringRequest(RefactoringVariant.RenameMethod, sourceCode, oldMethodName: oldMethodName, newMethodName: newMethodName);
}