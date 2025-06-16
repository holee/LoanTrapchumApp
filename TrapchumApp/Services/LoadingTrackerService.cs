namespace TrapchumApp.Services
{
    
    public sealed class LoadingTrackerService
    {
        public event Action<bool>? OnLoadingChanged;

        private int _activityCount = 0;

        public void Show()
        {
            Interlocked.Increment(ref _activityCount);

            OnLoadingChanged?.Invoke(IsLoading);
        }

        public void Hide()
        {
            Interlocked.Decrement(ref _activityCount);

            OnLoadingChanged?.Invoke(IsLoading);
        }

        public bool IsLoading => _activityCount > 0;

    }

}
