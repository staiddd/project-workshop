using System.Windows;
using System.Windows.Threading;

namespace RefactoringWorkshop.Gui;

public partial class App : Application
{
    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        try
        {
            MessageBox.Show(
                e.Exception.ToString(),
                "Неперехоплена помилка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch
        {
            // ignored
        }

        e.Handled = true;
    }
}
