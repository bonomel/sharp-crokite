using MyEveToolset.Data;
using System.Windows;

namespace MyEveToolset
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Startup_App(object sender, StartupEventArgs e)
        {
            SharpCrokiteDbContext dbContext = new();
            SharpCrokiteMainWindow sharpCrokiteMainWindow = new SharpCrokiteMainWindow(dbContext);

            sharpCrokiteMainWindow.Show();
        }
    }
}
