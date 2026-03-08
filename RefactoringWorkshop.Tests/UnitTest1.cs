using RefactoringWorkshop.Core;

namespace RefactoringWorkshop.Tests;

public class RenameVariableRefactoringTests
{
    private readonly RenameVariableRefactoring _sut = new();

    /// <summary>Renames a local variable declaration and its usages.</summary>
    [Fact]
    public void RenameVariable_LocalDeclarationAndUsage_UpdatesAllOccurrences()
    {
        var source = "int main(){ int count = 5; return count; }";
        var expected = "int main(){ int total = 5; return total; }";
        var actual = _sut.Apply(source, "count", "total");
        Assert.Equal(expected, actual);
    }

    /// <summary>Renames loop iterator variable in for statement body.</summary>
    [Fact]
    public void RenameVariable_ForLoopIterator_UpdatesIteratorReferences()
    {
        var source = "for(int i = 0; i < 10; ++i){ sum += i; }";
        var expected = "for(int index = 0; index < 10; ++index){ sum += index; }";
        var actual = _sut.Apply(source, "i", "index");
        Assert.Equal(expected, actual);
    }

    /// <summary>Keeps other identifiers intact when they only contain the old name as part.</summary>
    [Fact]
    public void RenameVariable_DoesNotTouchPartialMatches()
    {
        var source = "int count = 1; int counter = count + 1;";
        var expected = "int total = 1; int counter = total + 1;";
        var actual = _sut.Apply(source, "count", "total");
        Assert.Equal(expected, actual);
    }

    /// <summary>Renames a class field usage inside member method.</summary>
    [Fact]
    public void RenameVariable_Field_UpdatesMemberAccess()
    {
        var source = "class A{ int value; int Get(){ return value; } };";
        var expected = "class A{ int amount; int Get(){ return amount; } };";
        var actual = _sut.Apply(source, "value", "amount");
        Assert.Equal(expected, actual);
    }

    /// <summary>Renames a method parameter and all parameter usages.</summary>
    [Fact]
    public void RenameVariable_Parameter_UpdatesMethodBody()
    {
        var source = "int Add(int x){ return x + 1; }";
        var expected = "int Add(int number){ return number + 1; }";
        var actual = _sut.Apply(source, "x", "number");
        Assert.Equal(expected, actual);
    }

    /// <summary>Renames variable in nested block while preserving unrelated names.</summary>
    [Fact]
    public void RenameVariable_NestedBlock_UpdatesScopedSymbol()
    {
        var source = "int x = 1; { int y = x; x = y + 2; }";
        var expected = "int value = 1; { int y = value; value = y + 2; }";
        var actual = _sut.Apply(source, "x", "value");
        Assert.Equal(expected, actual);
    }

    /// <summary>Renames a pointer variable and dereference usage.</summary>
    [Fact]
    public void RenameVariable_Pointer_UpdatesDereference()
    {
        var source = "int v = 0; int* p = &v; *p = 4;";
        var expected = "int v = 0; int* ptr = &v; *ptr = 4;";
        var actual = _sut.Apply(source, "p", "ptr");
        Assert.Equal(expected, actual);
    }

    /// <summary>Renames variable used in conditional expression.</summary>
    [Fact]
    public void RenameVariable_Conditional_UpdatesConditionAndBody()
    {
        var source = "if(flag){ flag = false; }";
        var expected = "if(isEnabled){ isEnabled = false; }";
        var actual = _sut.Apply(source, "flag", "isEnabled");
        Assert.Equal(expected, actual);
    }

    /// <summary>Renames variable in arithmetic expression.</summary>
    [Fact]
    public void RenameVariable_Expression_UpdatesAllExpressionUsages()
    {
        var source = "int result = a + b; result = result * 2;";
        var expected = "int total = a + b; total = total * 2;";
        var actual = _sut.Apply(source, "result", "total");
        Assert.Equal(expected, actual);
    }

    /// <summary>Renames variable in return statement and declaration.</summary>
    [Fact]
    public void RenameVariable_Return_UpdatesDeclarationAndReturn()
    {
        var source = "int main(){ int status = 0; return status; }";
        var expected = "int main(){ int exitCode = 0; return exitCode; }";
        var actual = _sut.Apply(source, "status", "exitCode");
        Assert.Equal(expected, actual);
    }
}

/// <summary>
/// Unit tests for Extract Method variant (Variant 4) – all tests are expected to fail (TDD Red)
/// </summary>
public class ExtractMethodRefactoringTests
{
    private ExtractMethodRefactoring _sut = null!;

    public ExtractMethodRefactoringTests()
    {
        _sut = new ExtractMethodRefactoring();
    }

    [Fact] public void AssignmentBlockExtraction() => Assert.NotEqual(
        _sut.Apply("int x = 0; x = x + 1;", "x = x + 1;", "IncrementX"),
        "int x = 0; x = x + 1;");

    [Fact] public void LoopBodyExtraction() => Assert.NotEqual(
        _sut.Apply("for(int i=0;i<10;i++){ sum += i; }", "sum += i;", "AddToSum"),
        "for(int i=0;i<10;i++){ sum += i; }");

    [Fact] public void IfBranchExtraction() => Assert.NotEqual(
        _sut.Apply("if(x>0){ x--; }", "x--;", "DecrementX"),
        "if(x>0){ x--; }");

    [Fact] public void OutputStatementsExtraction() => Assert.NotEqual(
        _sut.Apply("std::cout << x;", "std::cout << x;", "PrintX"),
        "std::cout << x;");

    [Fact] public void ValidationBlockExtraction() => Assert.NotEqual(
        _sut.Apply("if(x<0){ return; }", "if(x<0){ return; }", "ValidateX"),
        "if(x<0){ return; }");

    [Fact] public void SetupBlockExtraction() => Assert.NotEqual(
        _sut.Apply("int a=1,b=2;", "int a=1,b=2;", "SetupVars"),
        "int a=1,b=2;");

    [Fact] public void MathBlockExtraction() => Assert.NotEqual(
        _sut.Apply("y = a*b + c;", "y = a*b + c;", "ComputeY"),
        "y = a*b + c;");

    [Fact] public void LoggingBlockExtraction() => Assert.NotEqual(
        _sut.Apply("log(x);", "log(x);", "LogX"),
        "log(x);");

    [Fact] public void FormattingBlockExtraction() => Assert.NotEqual(
        _sut.Apply("formatOutput(x);", "formatOutput(x);", "FormatX"),
        "formatOutput(x);");

    [Fact] public void ReturnRelatedBlockExtraction() => Assert.NotEqual(
        _sut.Apply("return x + y;", "return x + y;", "ReturnSum"),
        "return x + y;");
}