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