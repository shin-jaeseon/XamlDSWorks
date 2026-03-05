using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XamlDS.Itk.Messages;
using XamlDS.Itk.Services;

namespace XamlDS.Itk.Aui;

/// <summary>
/// Manages the lifecycle of an Avalonia desktop application with built-in support for
/// dependency injection, localization, and window management.
/// </summary>
/// <typeparam name="TWindowView">The type of the main window view.</typeparam>
/// <typeparam name="TWindowVm">The type of the main window view model.</typeparam>
public class ApplicationManager<TWindowView, TWindowVm> : IDisposable
    where TWindowView : Window, new()
    where TWindowVm : class
{
    protected Application Application { get; }
    protected IHost? Host { get; }
    protected IMessenger? Messenger { get; private set; }

    private IDisposable? _languageChangedSubscription;

    /// <summary>
    /// Initializes a new instance of the ApplicationManager.
    /// </summary>
    /// <param name="application">The Avalonia application instance.</param>
    /// <param name="host">The dependency injection host.</param>
    public ApplicationManager(Application application, IHost? host = null)
    {
        Application = application ?? throw new ArgumentNullException(nameof(application));
        Host = host;

        if (application.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            InitializeDesktopApplication(desktop);
            desktop.Exit += OnApplicationExit;
        }
    }

    /// <summary>
    /// Initialize desktop application. Override to customize initialization.
    /// </summary>
    protected virtual void InitializeDesktopApplication(IClassicDesktopStyleApplicationLifetime desktop)
    {
        EnsureHostInitialized();

        Messenger = GetRequiredService<IMessenger>();
        var viewModel = CreateMainViewModel();

        desktop.MainWindow = CreateMainWindow(viewModel);

        SubscribeToMessages(desktop);
    }

    /// <summary>
    /// Create the main view model. Override to provide custom ViewModel creation logic.
    /// </summary>
    protected virtual TWindowVm CreateMainViewModel()
    {
        return GetRequiredService<TWindowVm>();
    }

    /// <summary>
    /// Create the main window. Override to customize window creation.
    /// </summary>
    /// <param name="dataContext">The DataContext (view model) for the window.</param>
    protected virtual TWindowView CreateMainWindow(TWindowVm? dataContext)
    {
        var window = new TWindowView
        {
            FlowDirection = GetCurrentFlowDirection(),
            DataContext = dataContext
        };

        ConfigureWindow(window);
        return window;
    }

    /// <summary>
    /// Configure the window after creation. Override to add custom configuration.
    /// </summary>
    /// <param name="window">The window to configure.</param>
    protected virtual void ConfigureWindow(TWindowView window)
    {
        // Override in derived class to add custom configuration
    }

    /// <summary>
    /// Subscribe to application messages. Override to add custom subscriptions.
    /// </summary>
    protected virtual void SubscribeToMessages(IClassicDesktopStyleApplicationLifetime desktop)
    {
        if (Messenger == null)
            return;

        _languageChangedSubscription = Messenger.Subscribe<LanguageChangedMessage>(msg =>
        {
            OnLanguageChanged(desktop, msg);
        });
    }

    /// <summary>
    /// Handle language change event. Override to customize behavior.
    /// </summary>
    /// <param name="desktop">The desktop application lifetime.</param>
    /// <param name="message">The language changed message.</param>
    protected virtual void OnLanguageChanged(IClassicDesktopStyleApplicationLifetime desktop, LanguageChangedMessage message)
    {
        RecreateMainWindow(desktop);
    }

    /// <summary>
    /// Recreate the main window. Override to customize window recreation logic.
    /// </summary>
    protected virtual void RecreateMainWindow(IClassicDesktopStyleApplicationLifetime desktop)
    {
        var oldWindow = desktop.MainWindow;
        var dataContext = oldWindow?.DataContext as TWindowVm;

        desktop.MainWindow = CreateMainWindow(dataContext);
        desktop.MainWindow.Show();

        oldWindow?.Close();
    }

    /// <summary>
    /// Get the current flow direction based on the current culture.
    /// </summary>
    protected static FlowDirection GetCurrentFlowDirection()
    {
        return Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft
            ? FlowDirection.RightToLeft
            : FlowDirection.LeftToRight;
    }

    /// <summary>
    /// Get a required service from the dependency injection container.
    /// </summary>
    /// <typeparam name="T">The type of service to retrieve.</typeparam>
    /// <exception cref="InvalidOperationException">Thrown when the service is not registered.</exception>
    protected T GetRequiredService<T>() where T : class
    {
        return Host?.Services.GetService<T>()
            ?? throw new InvalidOperationException($"{typeof(T).Name} is not registered in the DI container.");
    }

    /// <summary>
    /// Ensure that the Host is initialized.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when Host is null.</exception>
    protected void EnsureHostInitialized()
    {
        if (Host == null)
            throw new InvalidOperationException("Host is not initialized. Please provide a valid IHost instance.");
    }

    /// <summary>
    /// Handle application exit event.
    /// </summary>
    protected virtual void OnApplicationExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        Dispose();
    }

    /// <summary>
    /// Dispose resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose resources.
    /// </summary>
    /// <param name="disposing">True if disposing managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _languageChangedSubscription?.Dispose();
            _languageChangedSubscription = null;
        }
    }
}
