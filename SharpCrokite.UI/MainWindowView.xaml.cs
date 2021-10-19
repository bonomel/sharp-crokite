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
    public partial class MainWindowView : Window
    {
        private readonly SharpCrokiteDbContext dbContext;

        public MainWindowView()
        {
            InitializeComponent();
        }

        public MainWindowView(SharpCrokiteDbContext dbContext) : this()
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
