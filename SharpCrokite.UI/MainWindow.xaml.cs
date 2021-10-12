using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using SharpCrokite.Core.ViewModels;
using SharpCrokite.Core.PriceUpdater;
using SharpCrokite.DataAccess.Queries;
using SharpCrokite.DataAccess;
using SharpCrokite.Core.StaticDataUpdater;
using SharpCrokite.Infrastructure.Repositories;
using System;

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
                    var harvestablesQuery = new AllHarvestablesQuery(new HarvestableRepository(dbContext), new MaterialRepository(dbContext));
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
            var staticDataUpdateController = new StaticDataUpdateController(new EsiStaticDataRetriever(),
                new HarvestableRepository(dbContext), new MaterialRepository(dbContext));

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
            catch (ArgumentNullException ex)
            {
                _ = MessageBox.Show($"Something went wrong while updating the static data!\nMessage:\n{ex.Message}",
                    "Http Request Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteStaticDataButton_Click(object sender, RoutedEventArgs e)
        {
            var staticDataUpdateController = new StaticDataUpdateController(new EsiStaticDataRetriever(), 
                new HarvestableRepository(dbContext), new MaterialRepository(dbContext));
            staticDataUpdateController.DeleteAllStaticData();

            ReloadGrid();
        }

        private void UpdatePriceDataButton_Click(object sender, RoutedEventArgs e)
        {
            var priceUpdateController = new PriceUpdateController(new EveMarketerPriceRetriever(), 
                new HarvestableRepository(dbContext), new MaterialRepository(dbContext));
            priceUpdateController.UpdatePrices();

            ReloadGrid();
        }

        private void DeletePriceDataButton_Click(object sender, RoutedEventArgs e)
        {
            var priceUpdateController = new PriceUpdateController(new EveMarketerPriceRetriever(), 
                new HarvestableRepository(dbContext), new MaterialRepository(dbContext));
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
