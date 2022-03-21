using System.Threading.Tasks;

namespace SharpCrokite.Core.Commands
{
    public static class TaskExtensions
    {
        public static async void FireAndForgetAsync(this Task task)
        {
            await task;
            //TODO: add error handling, see: https://johnthiriet.com/mvvm-going-async-with-async-command/ & https://johnthiriet.com/removing-async-void/
        }
    }
}
