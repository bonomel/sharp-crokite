using SharpCrokite.DataAccess.DatabaseContexts;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<SharpCrokiteDbContext>(ServiceLifetime.Singleton);

            services.AddSingleton<HarvestableRepository>();
            services.AddSingleton<MaterialRepository>();

            services.AddSingleton<MainWindowView>();
        }

        private void Startup_App(object sender, StartupEventArgs e)
        {
            SharpCrokiteDbContext dbContext = new SharpCrokiteDbContext();

            var mainWindow = serviceProvider.GetService<MainWindowView>();

            mainWindow.Show();
        }
    }
}
