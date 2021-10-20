using SharpCrokite.Core.ViewModels;
using System.Windows.Controls;

namespace SharpCrokite.UI.Views
{
    /// <summary>
    /// Interaction logic for AllHarvestablesView.xaml
    /// </summary>
    public partial class HarvestablesView : UserControl
    {
        public HarvestablesView()
        {
            DataContext = new HarvestablesViewModel();
            InitializeComponent();
        }
    }
}
