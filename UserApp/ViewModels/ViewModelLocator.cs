using CommunityToolkit.Mvvm.DependencyInjection;

namespace UserApp.ViewModels
{
    internal static class ViewModelLocator
    {
        public static MainViewModel MainViewModel => Ioc.Default.GetRequiredService<MainViewModel>();
    }
}
