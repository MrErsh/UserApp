namespace UserApp.Services
{
    internal interface IThreadingService
    {
        public void OnUiThread(Action action);
        Task<TResult> OnUiThreadAsync<TResult>(Func<TResult> callback);
    }
}
