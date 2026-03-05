using System.Text;
using RefactoringWorkshop.Core;

var engine = new RefactoringEngine();

Console.WriteLine("=== Refactoring Workshop (Lab #1 / TDD Red) ===");
Console.WriteLine("Available variant: 1 - Rename Variable");
Console.WriteLine("Type Q to exit.");

while (true)
{
    Console.WriteLine();
    Console.Write("Press 1 to run variant 1 (or Q to exit): ");
    var command = Console.ReadLine()?.Trim();

    if (string.Equals(command, "Q", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    if (!string.Equals(command, "1", StringComparison.Ordinal))
    {
        Console.WriteLine("Invalid choice. Try 1 or Q.");
        continue;
    }

    var sourceCode = ReadSourceCodeFromConsole();
    var request = BuildRenameVariableRequest(sourceCode);
    var result = engine.Apply(RefactoringVariant.RenameVariable, request);

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
        {
            break;
        }

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

    return new RefactoringRequest(sourceCode, oldName: oldName, newName: newName);
}
