using System.Windows;

namespace SharpCrokite.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();
        }

        private void UpdatePriceDataButton_Click(object sender, RoutedEventArgs e)
        {
            //var priceUpdateController = new PriceUpdateController(new EveMarketerPriceRetriever(), 
            //    new HarvestableRepository(dbContext), new MaterialRepository(dbContext));
            //priceUpdateController.UpdatePrices();

            //ReloadGrid();
        }

        private void DeletePriceDataButton_Click(object sender, RoutedEventArgs e)
        {
            //var priceUpdateController = new PriceUpdateController(new EveMarketerPriceRetriever(), 
            //    new HarvestableRepository(dbContext), new MaterialRepository(dbContext));
            //priceUpdateController.DeleteAllPrices();

            //ReloadGrid();
        }
    }
}
