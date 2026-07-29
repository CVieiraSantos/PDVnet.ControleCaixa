using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PDVnet.ControleCaixa.Infra.Ioc.DependencyInjection;
using PDVnet.ControleCaixa.UI.Services;
using PDVnet.ControleCaixa.UI.ViewModels;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace PDVnet.ControleCaixa.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {

        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.RegistrarDependencias();
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<INotificationService, NotificationService>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        CultureInfo cultura = new("pt-BR");
        CultureInfo.DefaultThreadCurrentCulture = cultura;
        CultureInfo.DefaultThreadCurrentUICulture = cultura;

        FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

        base.OnStartup(e);

        await _host.StartAsync();

        MainViewModel mainViewModel =
            _host.Services.GetRequiredService<MainViewModel>();

        await mainViewModel.InicializarAsync();

        MainWindow mainWindow = new()
        {
            DataContext = _host.Services.GetRequiredService<MainViewModel>()
        };

        mainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();

        _host.Dispose();

        base.OnExit(e);
    }
}
