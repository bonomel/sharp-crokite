using System.Threading.Tasks;
using System.Windows.Input;

namespace SharpCrokite.Core.Commands
{
    interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();
        bool CanExecute();
    }
}
