using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using HiringDashboard.ViewModels;
using HiringDashboard.Views;

namespace HiringDashboard;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    // This is a reference to our MainViewModel which we use to save the list on shutdown. You can also use Dependency Injection 
    // in your App.
    private readonly MainViewModel _mainViewModel = new MainViewModel();

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public override async void OnFrameworkInitializationCompleted()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = _mainViewModel // Remember to change this line to use our private reference to the MainViewModel
            };

            // Listen to the ShutdownRequested-event
            desktop.ShutdownRequested += DesktopOnShutdownRequested;
        }

        base.OnFrameworkInitializationCompleted();


    }


    // We want to save our ToDoList before we actually shutdown the App. As File I/O is async, we need to wait until file is closed 
    // before we can actually close this window

    private bool _canClose; // This flag is used to check if window is allowed to close
    private async void DesktopOnShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
    {
        e.Cancel = !_canClose; // cancel closing event first time

        if (!_canClose)
        {

        }
    }


}