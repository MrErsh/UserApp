using System.Windows.Threading;

namespace UserApp.Services
{
    internal class ThreadingService : IThreadingService
    {
        private readonly Dispatcher _dispatcher;

        public ThreadingService( Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void OnUiThread(Action action)
        {
            if (action == null)
                return;

            if (_dispatcher.CheckAccess())
                action();
            else
                _dispatcher.BeginInvoke(action);
        }

        public async Task<TResult> OnUiThreadAsync<TResult>(Func<TResult> callback)
        {
            if (callback == null)
                return default;

            return _dispatcher.CheckAccess()
                ? callback()
                : await _dispatcher.InvokeAsync(callback);
        }
    }
}
