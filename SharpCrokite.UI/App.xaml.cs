using SharpCrokite.DataAccess.DatabaseContexts;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using SharpCrokite.Infrastructure.Repositories;
using System;
using SharpCrokite.Core.ViewModels;
using SharpCrokite.UI.Views;

namespace SharpCrokite.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
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

            services.AddSingleton<MainWindowView>();
            services.AddSingleton<MainWindowViewModel>();

            services.AddSingleton<HarvestablesView>();
            services.AddSingleton<HarvestablesViewModel>();

            return services.BuildServiceProvider();
        }

        private void Startup_App(object sender, StartupEventArgs e)
        {
            MainWindowView window = serviceProvider.GetRequiredService<MainWindowView>();
            window.DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>();

            window.Show();
        }
    }
}
