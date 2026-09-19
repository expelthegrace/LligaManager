using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using LligaManager.Application.Abstractions;
using LligaManager.Application.UseCases.Convocatorias;
using LligaManager.Application.UseCases.Players;
using LligaManager.Domain.Services;
using LligaManager.Infrastructure.Persistence;

namespace LligaManager.UI;

public partial class App : Microsoft.UI.Xaml.Application
{
    private readonly IServiceProvider services;
    private MainWindow? mainWindow;

    public App()
    {
        InitializeComponent();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddDbContext<LligaManagerDbContext>(options =>
            options.UseSqlite("Data Source=lligamanager.db"));
        serviceCollection.AddScoped<IPlayerRepository, PlayerRepository>();
        serviceCollection.AddScoped<IConvocatoriaRepository, ConvocatoriaRepository>();
        serviceCollection.AddScoped<Raffle>();
        serviceCollection.AddTransient<CreatePlayer>();
        serviceCollection.AddTransient<UpdatePlayer>();
        serviceCollection.AddTransient<GetPlayers>();
        serviceCollection.AddTransient<GetPlayerDetails>();
        serviceCollection.AddTransient<CreateConvocatoria>();
        serviceCollection.AddTransient<UpdateConvocatoria>();
        serviceCollection.AddTransient<GetConvocatorias>();
        serviceCollection.AddTransient<GetConvocatoriaDetails>();
        serviceCollection.AddTransient<ExecuteConvocatoriaRaffle>();
        serviceCollection.AddSingleton<IConvocatoriaConfigurationProvider>(_ =>
            new ConvocatoriaConfigurationProvider(
                Path.Combine(AppContext.BaseDirectory, "Persistence", "convocatoria-config.json")));
        serviceCollection.AddTransient<MainWindow>();
        services = serviceCollection.BuildServiceProvider();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        mainWindow = services.GetRequiredService<MainWindow>();
        mainWindow.Activate();
    }
}
