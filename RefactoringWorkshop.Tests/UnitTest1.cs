using RefactoringWorkshop.Core;

namespace RefactoringWorkshop.Tests;

public class RenameVariableRefactoringTests
{
    private readonly RenameVariableRefactoring _sut = new();

    /// <summary>Перевіряє перейменування локальної змінної в оголошенні та використаннях.</summary>
    [Fact]
    public void RenameVariable_LocalDeclarationAndUsage_UpdatesAllOccurrences()
    {
        var source = "int main(){ int count = 5; return count; }";
        var expected = "int main(){ int total = 5; return total; }";
        var actual = _sut.Apply(source, "count", "total");
        Assert.Equal(expected, actual);
    }

    /// <summary>Перевіряє перейменування ітератора циклу for у всіх входженнях.</summary>
    [Fact]
    public void RenameVariable_ForLoopIterator_UpdatesIteratorReferences()
    {
        var source = "for(int i = 0; i < 10; ++i){ sum += i; }";
        var expected = "for(int index = 0; index < 10; ++index){ sum += index; }";
        var actual = _sut.Apply(source, "i", "index");
        Assert.Equal(expected, actual);
    }

    /// <summary>Перевіряє, що часткові збіги в інших ідентифікаторах не змінюються.</summary>
    [Fact]
    public void RenameVariable_DoesNotTouchPartialMatches()
    {
        var source = "int count = 1; int counter = count + 1;";
        var expected = "int total = 1; int counter = total + 1;";
        var actual = _sut.Apply(source, "count", "total");
        Assert.Equal(expected, actual);
    }

    /// <summary>Перевіряє перейменування поля класу та його використання в методі.</summary>
    [Fact]
    public void RenameVariable_Field_UpdatesMemberAccess()
    {
        var source = "class A{ int value; int Get(){ return value; } };";
        var expected = "class A{ int amount; int Get(){ return amount; } };";
        var actual = _sut.Apply(source, "value", "amount");
        Assert.Equal(expected, actual);
    }

    /// <summary>Перевіряє перейменування параметра методу та всіх його використань.</summary>
    [Fact]
    public void RenameVariable_Parameter_UpdatesMethodBody()
    {
        var source = "int Add(int x){ return x + 1; }";
        var expected = "int Add(int number){ return number + 1; }";
        var actual = _sut.Apply(source, "x", "number");
        Assert.Equal(expected, actual);
    }

    /// <summary>Перевіряє перейменування змінної у вкладеному блоці без побічних змін.</summary>
    [Fact]
    public void RenameVariable_NestedBlock_UpdatesScopedSymbol()
    {
        var source = "int x = 1; { int y = x; x = y + 2; }";
        var expected = "int value = 1; { int y = value; value = y + 2; }";
        var actual = _sut.Apply(source, "x", "value");
        Assert.Equal(expected, actual);
    }

    /// <summary>Перевіряє перейменування вказівника та операції розіменування.</summary>
    [Fact]
    public void RenameVariable_Pointer_UpdatesDereference()
    {
        var source = "int v = 0; int* p = &v; *p = 4;";
        var expected = "int v = 0; int* ptr = &v; *ptr = 4;";
        var actual = _sut.Apply(source, "p", "ptr");
        Assert.Equal(expected, actual);
    }

    /// <summary>Перевіряє перейменування змінної в умові та тілі умовного оператора.</summary>
    [Fact]
    public void RenameVariable_Conditional_UpdatesConditionAndBody()
    {
        var source = "if(flag){ flag = false; }";
        var expected = "if(isEnabled){ isEnabled = false; }";
        var actual = _sut.Apply(source, "flag", "isEnabled");
        Assert.Equal(expected, actual);
    }

    /// <summary>Перевіряє перейменування змінної в арифметичних виразах.</summary>
    [Fact]
    public void RenameVariable_Expression_UpdatesAllExpressionUsages()
    {
        var source = "int result = a + b; result = result * 2;";
        var expected = "int total = a + b; total = total * 2;";
        var actual = _sut.Apply(source, "result", "total");
        Assert.Equal(expected, actual);
    }

    /// <summary>Перевіряє перейменування змінної в оголошенні та операторі return.</summary>
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
/// Модульні тести для варіанта 4 (Виділення методу). На етапі TDD Red тести очікувано падають.
/// </summary>
public class ExtractMethodRefactoringTests
{
    private ExtractMethodRefactoring _sut = null!;

    public ExtractMethodRefactoringTests()
    {
        _sut = new ExtractMethodRefactoring();
    }

    /// <summary>Перевіряє виділення простого блоку присвоєння в окремий метод.</summary>
    [Fact] public void AssignmentBlockExtraction() => Assert.NotEqual(
        _sut.Apply("int x = 0; x = x + 1;", "x = x + 1;", "IncrementX"),
        "int x = 0; x = x + 1;");

    /// <summary>Перевіряє виділення тіла циклу в новий метод.</summary>
    [Fact] public void LoopBodyExtraction() => Assert.NotEqual(
        _sut.Apply("for(int i=0;i<10;i++){ sum += i; }", "sum += i;", "AddToSum"),
        "for(int i=0;i<10;i++){ sum += i; }");

    /// <summary>Перевіряє виділення гілки if в окремий метод.</summary>
    [Fact] public void IfBranchExtraction() => Assert.NotEqual(
        _sut.Apply("if(x>0){ x--; }", "x--;", "DecrementX"),
        "if(x>0){ x--; }");

    /// <summary>Перевіряє виділення оператора виведення в новий метод.</summary>
    [Fact] public void OutputStatementsExtraction() => Assert.NotEqual(
        _sut.Apply("std::cout << x;", "std::cout << x;", "PrintX"),
        "std::cout << x;");

    /// <summary>Перевіряє виділення блоку валідації в окремий метод.</summary>
    [Fact] public void ValidationBlockExtraction() => Assert.NotEqual(
        _sut.Apply("if(x<0){ return; }", "if(x<0){ return; }", "ValidateX"),
        "if(x<0){ return; }");

    /// <summary>Перевіряє виділення ініціалізаційного блоку в окремий метод.</summary>
    [Fact] public void SetupBlockExtraction() => Assert.NotEqual(
        _sut.Apply("int a=1,b=2;", "int a=1,b=2;", "SetupVars"),
        "int a=1,b=2;");

    /// <summary>Перевіряє виділення математичного обчислення в новий метод.</summary>
    [Fact] public void MathBlockExtraction() => Assert.NotEqual(
        _sut.Apply("y = a*b + c;", "y = a*b + c;", "ComputeY"),
        "y = a*b + c;");

    /// <summary>Перевіряє виділення блоку логування в окремий метод.</summary>
    [Fact] public void LoggingBlockExtraction() => Assert.NotEqual(
        _sut.Apply("log(x);", "log(x);", "LogX"),
        "log(x);");

    /// <summary>Перевіряє виділення блоку форматування в окремий метод.</summary>
    [Fact] public void FormattingBlockExtraction() => Assert.NotEqual(
        _sut.Apply("formatOutput(x);", "formatOutput(x);", "FormatX"),
        "formatOutput(x);");

    /// <summary>Перевіряє виділення блоку, пов'язаного з return-виразом.</summary>
    [Fact] public void ReturnRelatedBlockExtraction() => Assert.NotEqual(
        _sut.Apply("return x + y;", "return x + y;", "ReturnSum"),
        "return x + y;");

    /// <summary>Перевіряє, що при виділенні блоку новий виклик методу передає залежні змінні як параметри.</summary>
    [Fact]
    public void ExtractMethod_PassesDependentVariablesToNewMethodCall()
    {
        var source = "int total=0; int delta=5; total = total + delta;";
        var actual = _sut.Apply(source, "total = total + delta;", "ApplyDelta");

        Assert.Matches(@".*ApplyDelta\s*\([^)]*total[^)]*delta[^)]*\)\s*;.*", actual);
    }

    /// <summary>Перевіряє передачу параметра-об'єкта в новий метод при виділенні блоку з кількома викликами.</summary>
    [Fact]
    public void ExtractMethod_PassesObjectParameterForProcessingBlock()
    {
        var source = "if(order == nullptr){ return -1; } validate(order); recalculate(order); log(order->id); return 0;";
        var actual = _sut.Apply(source, "validate(order); recalculate(order); log(order->id);", "ProcessOrder");

        Assert.Matches(@".*ProcessOrder\s*\([^)]*order[^)]*\)\s*;.*", actual);
    }
}


public class RenameMethodRefactoringTests
{
    private readonly RenameMethodRefactoring _sut = new();

    /// <summary>Перевіряє перейменування назви методу в його оголошенні.</summary>
    [Fact]
    public void RenameMethod_DefinitionOnly_UpdatesMethodName()
    {
        var source = "void Calculate() { int Calculate=1 }";
        var expected = "void Compute() { int Calculate=1 }";
        var actual = _sut.Apply(source, "Calculate", "Compute");
        Assert.Equal(expected, actual);
    }

    /// <summary>Перевіряє перейменування виклику методу в місці використання.</summary>
    [Fact]
    public void RenameMethod_CallSite_UpdatesUsage()
    {
        var source = "int main() { Calculate(); }";
        var expected = "int main() { Compute(); }";
        var actual = _sut.Apply(source, "Calculate", "Compute");
        Assert.Equal(expected, actual);
    }

    /// <summary>Перевіряє перейменування оголошення методу та всіх його викликів.</summary>
    [Fact]
    public void RenameMethod_DeclarationAndMultipleCalls_UpdatesAll()
    {
        var source = "void Run() { } int main() { Run(); Run(); }";
        var expected = "void Start() { } int main() { Start(); Start(); }";
        var actual = _sut.Apply(source, "Run", "Start");
        Assert.Equal(expected, actual);
    }

    /// <summary>Перевіряє перейменування методу без зміни списку параметрів.</summary>
    [Fact]
    public void RenameMethod_WithParameters_KeepsSignatureIntact()
    {
        var source = "void Log(string msg, int level) { }";
        var expected = "void Print(string msg, int level) { }";
        var actual = _sut.Apply(source, "Log", "Print");
        Assert.Equal(expected, actual);
    }

    /// <summary>Перевіряє перейменування методу в контексті оголошення класу.</summary>
    [Fact]
    public void RenameMethod_InClassContext_UpdatesMethodName()
    {
        var source = "class Player { void Move() { } };";
        var expected = "class Player { void Jump() { } };";
        var actual = _sut.Apply(source, "Move", "Jump");
        Assert.Equal(expected, actual);
    }

    /// <summary>Перевіряє перейменування при використанні вказівника на метод.</summary>
    [Fact]
    public void RenameMethod_PointerToMethod_UpdatesUsage()
    {
        var source = "auto func = &Actions::Execute;";
        var expected = "auto func = &Actions::Run;";
        var actual = _sut.Apply(source, "Execute", "Run");
        Assert.Equal(expected, actual);
    }

    /// <summary>Перевіряє перейменування шаблонної функції.</summary>
    [Fact]
    public void RenameMethod_TemplateFunction_UpdatesName()
    {
        var source = "template<typename T> void Sort(T arr) { }";
        var expected = "template<typename T> void Order(T arr) { }";
        var actual = _sut.Apply(source, "Sort", "Order");
        Assert.Equal(expected, actual);
    }

    /// <summary>Перевіряє, що схожі назви (підрядки) не перейменовуються помилково.</summary>
    [Fact]
    public void RenameMethod_SimilarNameSubstring_DoesNotRenamePartialMatch()
    {
        var source = "void Test() { } void TestAll() { }";
        var expected = "void Check() { } void TestAll() { }";
        var actual = _sut.Apply(source, "Test", "Check");
        Assert.Equal(expected, actual);
    }

    /// <summary>Перевіряє, що в коментарі назва методу не змінюється.</summary>
    [Fact]
    public void RenameMethod_InsideComment_DoesNotRename()
    {
        var source = "// This calls Calculate() \n void Calculate() { }";
        var expected = "// This calls Calculate() \n void Compute() { }";
        var actual = _sut.Apply(source, "Calculate", "Compute");
        Assert.Equal(expected, actual);
    }

    /// <summary>Перевіряє, що в рядковому літералі назва методу не змінюється.</summary>
    [Fact]
    public void RenameMethod_InStringLiteral_DoesNotRename()
    {
        var source = "log(\"Action: Calculate\"); void Calculate() { }";
        var expected = "log(\"Action: Calculate\"); void Compute() { }";
        var actual = _sut.Apply(source, "Calculate", "Compute");
        Assert.Equal(expected, actual);
    }
}