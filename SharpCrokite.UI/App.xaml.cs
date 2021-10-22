using SharpCrokite.DataAccess.DatabaseContexts;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using SharpCrokite.Infrastructure.Repositories;
using System;
using SharpCrokite.Core.ViewModels;
using SharpCrokite.UI.Views;

namespace SharpCrokite.UI
{
    public partial class App : Application
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

            services.AddSingleton<NormalOreIskPerHourView>();
            services.AddSingleton<NormalOreIskPerHourViewModel>();

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
