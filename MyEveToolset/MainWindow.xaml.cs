using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using MyEveToolset.Data;
using MyEveToolset.Data.Queries;
using MyEveToolset.PriceUpdater;
using MyEveToolset.StaticDataUpdater;
using MyEveToolset.ViewModels;

namespace MyEveToolset
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MyEveToolDbContext dbContext;

        private IEnumerable<HarvestableViewModel> harvestables;

        private IEnumerable<HarvestableViewModel> Harvestables
        {
            get
            {
                if(harvestables == null)
                {
                    var harvestablesQuery = new AllHarvestablesQuery(dbContext);
                    harvestables = harvestablesQuery.Execute();
                }
                return harvestables;
            }
        }


        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(MyEveToolDbContext dbContext) : this()
        {
            this.dbContext = dbContext;

            HarvestablesDataGrid.ItemsSource = Harvestables;
        }

        private void UpdateStaticDataButton_Click(object sender, RoutedEventArgs e)
        {
            var staticDataUpdateController = new StaticDataUpdateController(dbContext, new StaticDataRetriever(), new EsiJSONToDataModelConverter());

            try
            {
                staticDataUpdateController.UpdateData();
                ReloadGrid();
            }
            catch (HttpRequestException ex)
            {
                _ = MessageBox.Show($"Something went wrong while updating the static data!\nMessage:\n{ex.Message}", 
                    "Http Request Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteStaticDataButton_Click(object sender, RoutedEventArgs e)
        {
            var staticDataUpdateController = new StaticDataUpdateController(dbContext, new StaticDataRetriever(), new EsiJSONToDataModelConverter());
            staticDataUpdateController.DeleteData();

            ReloadGrid();
        }

        private void UpdatePriceDataButton_Click(object sender, RoutedEventArgs e)
        {
            var priceUpdateController = new PriceUpdateController(dbContext, new EveMarketerPriceRetriever());
            priceUpdateController.UpdatePrices();

            ReloadGrid();
        }

        private void DeletePriceDataButton_Click(object sender, RoutedEventArgs e)
        {
            var priceUpdateController = new PriceUpdateController(dbContext, new EveMarketerPriceRetriever());
            priceUpdateController.DeleteAllPrices();

            ReloadGrid();
        }

        private void ReloadGrid()
        {
            harvestables = null;
            HarvestablesDataGrid.ItemsSource = Harvestables;
        }
    }
}
