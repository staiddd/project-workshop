# Refactoring Workshop 1

Лабораторна робота №1 з дисципліни по рефакторингу:
- простий frontend (консольний UI) для вибору сценарію;
- прототипи рефакторингів без реалізації (етап TDD Red);
- модульні тести для варіантів `1` , `4`, та `8`.

## Що реалізовано

- **Варіант 1:** перейменування змінної (`RenameVariableRefactoring`)
- **Варіант 4:** винесення блоку в метод (`ExtractMethodRefactoring`)
-  **Варіант 8:** винесення перейменування  методу (`RenameMethodRefactoring`)
- **Entry point:** `RefactoringWorkshop.App/Program.cs`
- **Core-каркас:** `RefactoringWorkshop.Core/Class1.cs`
- **Тести (30 шт):** `RefactoringWorkshop.Tests/UnitTest1.cs`  
  - 10 тестів для варіанту 1  
  - 10 тестів для варіанту 4 (TDD Red)
  - 10 тестів для варіанту 8

> Важливо: логіка рефакторингів ще не реалізована навмисно.  
> Це стадія **TDD Red**, тому всі тести мають падати.

## Структура рішення

- `RefactoringWorkshop.sln` — solution
- `RefactoringWorkshop.Core` — інтерфейси, моделі запиту, заглушки рефакторингів
- `RefactoringWorkshop.App` — простий консольний frontend/точка входу
- `RefactoringWorkshop.Tests` — xUnit тести (10 тестів для варіанту 1)

## Передумови

Потрібно встановити:
- .NET SDK 7.0+ (рекомендовано 7.0.x або новіше)
- Git

Перевірка:

```bash
dotnet --version
git --version
```

## Як запустити локально

Виконувати з кореня репозиторію (`project-workshop`):

1. Відновити пакети:

```bash
dotnet restore RefactoringWorkshop.sln
```

2. Запустити frontend:

```bash
dotnet run --project RefactoringWorkshop.App
```

3. Запустити тести:

```bash
dotnet test RefactoringWorkshop.sln
```

Очікувано: `30 failed, 0 passed` (TDD Red).

## Як працює frontend

Після запуску:
- вибираєш `1` (або `Q` для виходу);
- вставляєш C++ код;
- вводиш `END` на новому рядку, щоб завершити введення;
- вводиш параметри (старе/нове ім'я змінної);
- програма друкує результат застосування поточної заглушки.

## Як запустити на інших комп'ютерах тімейтів

### Варіант A: через Git (рекомендовано)

На ПК тімейта:

```bash
git clone <URL-вашого-репозиторію>
cd project-workshop-1
dotnet restore RefactoringWorkshop.sln
dotnet run --project RefactoringWorkshop.App
dotnet test RefactoringWorkshop.sln
```

### Варіант B: через архів (zip)

1. Запакуй і передай всю папку `project-workshop-1` (без видалення `.sln` та `.csproj` файлів).
2. Тімейт розпаковує архів.
3. Відкриває термінал у корені проєкту.
4. Виконує:

```bash
dotnet restore RefactoringWorkshop.sln
dotnet run --project RefactoringWorkshop.App
dotnet test RefactoringWorkshop.sln
```

## Нотатки по ОС

- **Windows (PowerShell):** команди з README працюють без змін.
- **Windows (cmd):** ті самі команди також працюють.
- **macOS/Linux:** ті самі команди працюють, якщо встановлено .NET SDK 7+.

## Поточний статус

- Архітектурний каркас готовий.
- Точка входу для варіанту 1 готова.
- 30 тестів написано.
- Усі тести червоні (очікувано для Lab #1, етап Red).

## Don't pay attention
export DOTNET_ROOT=$HOME/.dotnet
export PATH=$DOTNET_ROOT:$PATH
