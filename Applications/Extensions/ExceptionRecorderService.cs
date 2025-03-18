using System.Collections.ObjectModel;

namespace Applications.Extensions
{
    public class ExceptionRecorderService
    {
        public ObservableCollection<Exception> Exceptions { get; set; } = new();
    }
}
