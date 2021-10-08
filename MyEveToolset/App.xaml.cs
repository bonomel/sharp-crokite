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
            MyEveToolDbContext dbContext = new();
            MainWindow window = new MainWindow(dbContext);

            window.Title = "My EVE Tool";

            window.Show();
        }
    }
}
