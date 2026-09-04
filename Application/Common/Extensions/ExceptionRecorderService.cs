using System.Collections.ObjectModel;

namespace Application.Extensions
{
    public class ExceptionRecorderService
    {
        public ObservableCollection<Exception> Exceptions { get; set; } = new();
    }
}
