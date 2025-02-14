using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Input;
using UserApp.Dal;
using UserApp.Dal.Model;
using UserApp.Dialogs;
using UserApp.Model;
using UserApp.Services;

namespace UserApp.ViewModels
{
    internal class MainViewModel : ObservableObject
    {  
        #region Fields

        private readonly IUserRepository _userRepository;
        private readonly IThreadingService _threadingService;

        private HashSet<string> _logins = [];

        #endregion

        public MainViewModel(IUserRepository userRepository, IThreadingService threadingService)
        {
            _userRepository = userRepository ?? throw new ArgumentException(nameof(_userRepository));
            _threadingService = threadingService ?? throw new ArgumentException(nameof(_threadingService));

            RefreshCommand = new RelayCommand(async () => await RefreshAsync());
            AddUserCommand = new RelayCommand(async () => await AddUserAsync());
            EditUserCommand = new RelayCommand<User>(async user => await EditUserAsync(user));
            DeleteUserCommand = new RelayCommand<User>(async user => await DeleteUserAsync(user));

            Task.Run(RefreshAsync);
        }

        #region Properties

        public ICollection<User> Users { get; set; }

        public ICommand RefreshCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }

        #endregion

        #region Command handlers

        private async Task RefreshAsync()
        {
            try
            {
                var users = await _userRepository.GetAllAsync().ConfigureAwait(false);
                var logins = users.Select(u => u.Login).ToArray();
                var logHashSet = new HashSet<string>(users.Select(u => u.Login).ToArray());
                  
                _logins = [.. users.Select(u => u.Login)];
                _threadingService.OnUiThread(() => { Users = null; Users = users; });
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private async Task AddUserAsync()
        {
            // по хорошему надо тоже вынести в какой-нибудь IDialogService.ShowDialog()
            var addDialog = new UserDialog();
            var newUser = new BindableUser();
            addDialog.DataContext = new UserDialogViewModel(newUser, _logins);
            var result = addDialog.ShowDialog();
            if (result != true)
                return;
            try
            {
                var user = CreateUserModel(newUser);
                await _userRepository.AddAsync(user).ConfigureAwait(false);
                await RefreshAsync().ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private async Task EditUserAsync(User user)
        {
            if (user == null)
                return;

            var editDialog = new UserDialog();
            var bindableUser = CreateBindableUser(user);
            editDialog.DataContext = new UserDialogViewModel(bindableUser, _logins);
            var result = editDialog.ShowDialog();
            if (result != true)
                return;

            try
            {
                await _userRepository.UpdateAsync(CreateUserModel(bindableUser)).ConfigureAwait(false);
                await RefreshAsync().ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private async Task DeleteUserAsync(User user)
        {
            if (user == null)
                return;

            try
            {
                var result = await _userRepository.DeleteAsync(user.Id).ConfigureAwait(false);
                if (result == true)
                    await RefreshAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        #endregion

        #region Private methods

        // Вместо этого можно было бы использовать какой-нибудь маппер
        #region 

        private static User CreateUserModel([NotNull]BindableUser bindableUser)
        {
            return new User
            {
                Id = bindableUser.Id,
                Login = bindableUser.Login,
                FirstName = bindableUser.FirstName,
                LastName = bindableUser.LastName
            };
       }

        private static BindableUser CreateBindableUser(User user)
        {
            return new BindableUser()
            {
                Id  = user.Id,
                Login = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        #endregion

        private static void ShowError(string text)
        {
            // это тоже можно вынести в IDialogService
            // при создании и регистрации такого сервиса в App.xaml
            // можно будет получить значение, для параметра owner 
            MessageBox.Show(text, "Error");
        }

        #endregion
    }
}
