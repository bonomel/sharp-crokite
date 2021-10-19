using SharpCrokite.DataAccess.DatabaseContexts;
using System.Windows;

namespace SharpCrokite.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Startup_App(object sender, StartupEventArgs e)
        {
            SharpCrokiteDbContext dbContext = new SharpCrokiteDbContext();
            MainWindowView sharpCrokiteMainWindow = new MainWindowView(dbContext);

            sharpCrokiteMainWindow.Show();
        }
    }
}
