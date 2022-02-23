using JetBrains.Annotations;
using SharpCrokite.Core.Models;

namespace SharpCrokite.Core.ViewModels
{
    [UsedImplicitly]
    public class IskPerHourViewModel : IContentViewModel
    {
        [UsedImplicitly]
        public IskPerHourGridViewModel<AsteroidIskPerHour> AsteroidIskPerHourGridViewModel { get; }

        [UsedImplicitly]
        public IskPerHourGridViewModel<MoonOreIskPerHour> MoonOreIskPerHourGridViewModel { get; }

        [UsedImplicitly]
        public IskPerHourGridViewModel<IceIskPerHour> IceIskPerHourGridViewModel { get; }

        public IskPerHourViewModel(AsteroidIskPerHourGridViewModel asteroidIskPerHourGridViewModel,
            MoonOreIskPerHourGridViewModel moonOreIskPerHourGridViewModel, IceIskPerHourGridViewModel iceIskPerHourGridViewModel)
        {
            AsteroidIskPerHourGridViewModel = asteroidIskPerHourGridViewModel;
            MoonOreIskPerHourGridViewModel = moonOreIskPerHourGridViewModel;
            IceIskPerHourGridViewModel = iceIskPerHourGridViewModel;
        }

        public void UpdatePrices()
        {
            AsteroidIskPerHourGridViewModel.UpdatePrices();
            MoonOreIskPerHourGridViewModel.UpdatePrices();
            IceIskPerHourGridViewModel.UpdatePrices();
        }

        public void ReloadStaticData()
        {
            AsteroidIskPerHourGridViewModel.ReloadStaticData();
            MoonOreIskPerHourGridViewModel.ReloadStaticData();
            IceIskPerHourGridViewModel.ReloadStaticData();
        }
    }

    public interface IContentViewModel
    {
    }
}