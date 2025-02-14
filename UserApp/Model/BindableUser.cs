using CommunityToolkit.Mvvm.ComponentModel;

namespace UserApp.Model
{
    // Полные геттеры, сеттеры генерируются через Fody.PropertyChanged
    public class BindableUser : ObservableObject
    {
        public Guid Id { get; set; }
        public string Login { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
