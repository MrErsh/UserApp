
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Windows;
using UserApp.Config;
using UserApp.Dal;
using UserApp.Dal.Providers.InMemory;
using UserApp.Dal.Providers.MSSQLServer;
using UserApp.Dal.Providers.Xml;
using UserApp.Services;
using UserApp.ViewModels;

namespace UserApp;

public partial class App : Application
{
    public App()
    {
        InitializeServiceContainer();
    }

    private void InitializeServiceContainer()
    {
        var sc = new ServiceCollection();
        var threadingService = new ThreadingService(this.Dispatcher);
        sc.AddSingleton<IThreadingService>(threadingService);
        sc.AddTransient<MainViewModel>();
        RegisterDataSource(sc);

        var serviceProvider = sc.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = true
        });
        Ioc.Default.ConfigureServices(serviceProvider);
    }

    private void RegisterDataSource(/*Container sc, */ServiceCollection sc)
    {
        Configuration config =
            ConfigurationManager.OpenExeConfiguration(
            ConfigurationUserLevel.None);
      
        if (config.GetSection("dataSource") is DataSourceSection section)
        {
            switch (section.Type?.ToUpper())
            {
                case "INMEMORY":
                    sc.AddTransient<IUserRepository, InMemoryUserRepository>();
                    break;

                case "XML":
                    var path = section.Path;
                    sc.AddTransient<IUserRepository>(_ => 
                        new XmlUserRepository(section.Path, new InMemoryUserRepository()));
                    break;

                case "SQLSERVER":
                    var connectionString = section.ConnectionString;
                    sc.AddDbContext<UserDbContext>(options => options.UseSqlServer(connectionString));
                    sc.AddDbContextFactory<UserDbContext>(options => options.UseSqlServer(connectionString));
                    sc.AddTransient<IUserRepository, MSSQLServerUserRepository>();
                    break;

                default:
                    // show error message
                    break;
                    
            }
        }
    }
}

