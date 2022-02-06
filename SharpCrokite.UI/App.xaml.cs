using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

using SharpCrokite.Core.ViewModels;
using SharpCrokite.DataAccess.DatabaseContexts;
using SharpCrokite.Infrastructure.Repositories;
using SharpCrokite.UI.Views;

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
            IServiceCollection services = new ServiceCollection();

            services.AddDbContext<SharpCrokiteDbContext>(ServiceLifetime.Singleton);

            services.AddSingleton<HarvestableRepository>();
            services.AddSingleton<MaterialRepository>();

            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();

            services.AddSingleton<AsteroidIskPerHourView>();
            services.AddSingleton<AsteroidIskPerHourViewModel>();
            services.AddSingleton<MoonOreIskPerHourView>();
            services.AddSingleton<MoonOreIskPerHourViewModel>();
            services.AddSingleton<IceIskPerHourView>();
            services.AddSingleton<IceIskPerHourViewModel>();

            services.AddSingleton<HarvestablesView>();
            services.AddSingleton<HarvestablesViewModel>();

            return services.BuildServiceProvider();
        }

        private void Startup_App(object sender, StartupEventArgs e)
        {
            MainWindow window = serviceProvider.GetRequiredService<MainWindow>();
            window.DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>();

            window.Show();
        }
    }
}
