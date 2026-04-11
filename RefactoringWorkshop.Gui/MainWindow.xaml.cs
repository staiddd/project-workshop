using System.Windows;
using System.Windows.Controls;
using RefactoringWorkshop.Core;

namespace RefactoringWorkshop.Gui;

public partial class MainWindow : Window
{
    private readonly RefactoringEngine _engine = new();

    public MainWindow()
    {
        InitializeComponent();
        VariantCombo.SelectionChanged += VariantCombo_OnSelectionChanged;
        UpdateVariantPanels();
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        Activate();
        Focus();
    }

    private void VariantCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e) =>
        UpdateVariantPanels();

    private void UpdateVariantPanels()
    {
        // SelectionChanged може спрацювати під час InitializeComponent() (IsSelected на першому пункті),
        // коли ще не існують іменовані панелі — тоді обходимо без звернення до них.
        if (RenameVariablePanel is null || ExtractMethodPanel is null || RenameMethodPanel is null)
            return;

        var tag = (VariantCombo.SelectedItem as ComboBoxItem)?.Tag as string;
        RenameVariablePanel.Visibility = tag == "RenameVariable" ? Visibility.Visible : Visibility.Collapsed;
        ExtractMethodPanel.Visibility = tag == "ExtractMethod" ? Visibility.Visible : Visibility.Collapsed;
        RenameMethodPanel.Visibility = tag == "RenameMethod" ? Visibility.Visible : Visibility.Collapsed;
    }

    private void Apply_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var tag = (VariantCombo.SelectedItem as ComboBoxItem)?.Tag as string;
            var source = SourceText.Text ?? string.Empty;
            RefactoringRequest request = tag switch
            {
                "RenameVariable" => new RefactoringRequest(
                    RefactoringVariant.RenameVariable,
                    source,
                    oldName: OldVarName.Text ?? string.Empty,
                    newName: NewVarName.Text ?? string.Empty),
                "ExtractMethod" => new RefactoringRequest(
                    RefactoringVariant.ExtractMethod,
                    source,
                    selectedBlock: SelectedBlock.Text ?? string.Empty,
                    newMethodName: NewMethodNameExtract.Text ?? string.Empty),
                "RenameMethod" => new RefactoringRequest(
                    RefactoringVariant.RenameMethod,
                    source,
                    oldMethodName: OldMethodName.Text ?? string.Empty,
                    newMethodName: NewMethodNameRename.Text ?? string.Empty),
                _ => throw new InvalidOperationException("Невідомий варіант.")
            };

            OutputText.Text = _engine.Apply(request);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
