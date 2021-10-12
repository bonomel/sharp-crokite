using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SharpCrokite.Core.ViewModels;
using SharpCrokite.Core.PriceUpdater;
using SharpCrokite.DataAccess.Queries;
using SharpCrokite.DataAccess;
using SharpCrokite.Core.StaticDataUpdater;

namespace SharpCrokite.UI
{ 
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SharpCrokiteMainWindow : Window
    {
        private readonly SharpCrokiteDbContext dbContext;

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

        public SharpCrokiteMainWindow()
        {
            InitializeComponent();
        }

        public SharpCrokiteMainWindow(SharpCrokiteDbContext dbContext) : this()
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

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            if (row != null)
            {
                row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private void ReloadGrid()
        {
            harvestables = null;
            HarvestablesDataGrid.ItemsSource = Harvestables;
        }
    }
}
