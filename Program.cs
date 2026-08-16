using Avalonia;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using back.Data;
using Gestion_avion.ViewModels;

namespace Gestion_avion;

sealed class Program
{
    public static IServiceProvider? ServiceProvider { get; private set; }

    [STAThread]
    public static void Main(string[] args)
    {
        ConfigureServices();
        TestAndInitializeDatabase();

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    private static void ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddDbContextFactory<Contextedb>(options =>
            options.UseNpgsql("Host=localhost;Port=5432;Database=gestion_vol04;Username=root;Password=Hasambarana36"));

        services.AddTransient<DashboardViewModel>();

        ServiceProvider = services.BuildServiceProvider();
    }

    private static void TestAndInitializeDatabase()
    {
        using var scope = ServiceProvider?.CreateScope();
        if (scope == null) return;

        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<Contextedb>>();
        using var dbContext = factory.CreateDbContext();

        try
        {
            if (dbContext.Database.CanConnect())
            {
                Console.WriteLine("Connexion success");
                
                bool created = dbContext.Database.EnsureCreated();
                if (created)
                {
                    Console.WriteLine("Database create succesfully");
                }
                else
                {
                    Console.WriteLine("The database already exist.");
                }
            }
            else
            {
                Console.WriteLine("Connexion not success");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error access : {ex.Message}");
        }
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
#if DEBUG
            .WithDeveloperTools()
#endif
            .WithInterFont()
            .LogToTrace();
}