using System;
using System.Net.Http;
using System.Windows;
using SharpCrokite.Core.PriceUpdater;
using SharpCrokite.Core.StaticDataUpdater;
using SharpCrokite.DataAccess.DatabaseContexts;
using SharpCrokite.Infrastructure.Repositories;
using SharpCrokite.Core.StaticDataUpdater.Esi;

namespace SharpCrokite.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SharpCrokiteMainWindowView : Window
    {
        private readonly SharpCrokiteDbContext dbContext;

        public SharpCrokiteMainWindowView()
        {
            InitializeComponent();
        }

        public SharpCrokiteMainWindowView(SharpCrokiteDbContext dbContext) : this()
        {
            try
            {
                this.dbContext = dbContext;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

            //ReloadGrid();
        }

        private void UpdatePriceDataButton_Click(object sender, RoutedEventArgs e)
        {
            var priceUpdateController = new PriceUpdateController(new EveMarketerPriceRetriever(), 
                new HarvestableRepository(dbContext), new MaterialRepository(dbContext));
            priceUpdateController.UpdatePrices();

            //ReloadGrid();
        }

        private void DeletePriceDataButton_Click(object sender, RoutedEventArgs e)
        {
            var priceUpdateController = new PriceUpdateController(new EveMarketerPriceRetriever(), 
                new HarvestableRepository(dbContext), new MaterialRepository(dbContext));
            priceUpdateController.DeleteAllPrices();

            //ReloadGrid();
        }

        private void ReloadGrid()
        {
            
        }
    }
}
