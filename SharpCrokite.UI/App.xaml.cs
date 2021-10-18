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
            SharpCrokiteMainWindowView sharpCrokiteMainWindow = new SharpCrokiteMainWindowView(dbContext);

            sharpCrokiteMainWindow.Show();
        }
    }
}
