using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

using SharpCrokite.Core.ViewModels;
using SharpCrokite.DataAccess.DatabaseContexts;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.UI
{
    public partial class App
    {
        private IServiceProvider serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            serviceProvider = CreateServiceProvider();
            base.OnStartup(e);
        }

        private static IServiceProvider CreateServiceProvider()
        {
            return new ServiceCollection()
                .AddDbContext<SharpCrokiteDbContext>(ServiceLifetime.Singleton)

                .AddSingleton<HarvestableRepository>()
                .AddSingleton<MaterialRepository>()

                .AddSingleton<MainWindow>()

                .AddSingleton<MainWindowViewModel>()
                .AddSingleton<NavigatorViewModel>()
                .AddSingleton<IskPerHourViewModel>()
                .AddSingleton<AsteroidIskPerHourGridViewModel>()
                .AddSingleton<MoonOreIskPerHourGridViewModel>()
                .AddSingleton<IceIskPerHourGridViewModel>()
                .AddSingleton<SurveyCalculatorViewModel>()

                .BuildServiceProvider();
        }

        private void Startup_App(object sender, StartupEventArgs e)
        {
            serviceProvider.GetRequiredService<SharpCrokiteDbContext>().RunMigrations();

            MainWindow window = serviceProvider.GetRequiredService<MainWindow>();
            window.DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>();
            window.Show();
        }
    }
}
