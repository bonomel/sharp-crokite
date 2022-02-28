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

            services.AddSingleton<NavigatorViewModel>();

            services.AddSingleton<IskPerHourViewModel>();
            services.AddSingleton<AsteroidIskPerHourGridViewModel>();
            services.AddSingleton<MoonOreIskPerHourGridViewModel>();
            services.AddSingleton<IceIskPerHourGridViewModel>();

            services.AddSingleton<SurveyCalculatorViewModel>();

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
