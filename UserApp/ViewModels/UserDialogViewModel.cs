using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections;
using System.ComponentModel;
using UserApp.Model;

namespace UserApp.ViewModels
{
    public class UserDialogViewModel : ObservableObject, INotifyDataErrorInfo
    {
        const string EMPTY_ERROR = "The value must not be empty";
        const string UNIQUE_ERROR = "A user with this login already exists";

        private readonly HashSet<string> _logins;
        private readonly string _originalLogin;
        private string _loginError;
        

        public UserDialogViewModel(BindableUser user, HashSet<string> logins)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            _logins = logins ?? throw new ArgumentNullException(nameof(logins));

            _originalLogin = user.Login;
        }

        public BindableUser User { get; set; }

        public string Login
        {
            get => User.Login;
            set
            {
                ValidateLogin(value);
                User.Login = value;
                OnPropertyChanged(nameof(Login));
            }
        }


        #region Implementation of INotifyDataErrorInfo

        public bool HasErrors => !string.IsNullOrWhiteSpace(_loginError);

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return new[] { _loginError };
        }

        #endregion

        private void ValidateLogin(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                _loginError = EMPTY_ERROR;
            else if (value != _originalLogin && _logins.Contains(value))
                _loginError = UNIQUE_ERROR;
            else
                _loginError = null;

            if(_originalLogin != _loginError)
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Login)));
        }
    }
}
