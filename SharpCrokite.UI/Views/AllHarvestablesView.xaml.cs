using SharpCrokite.Core.ViewModels;
using System.Windows.Controls;

namespace SharpCrokite.UI.Views
{
    /// <summary>
    /// Interaction logic for AllHarvestablesView.xaml
    /// </summary>
    public partial class AllHarvestablesView : UserControl
    {
        public AllHarvestablesView()
        {
            InitializeComponent();
            DataContext = new HarvestableViewModel();
        }
    }
}
