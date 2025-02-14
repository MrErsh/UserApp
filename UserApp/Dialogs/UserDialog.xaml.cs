using System.Windows;

namespace UserApp.Dialogs
{
    public partial class UserDialog : Window
    {
        public UserDialog() => InitializeComponent();

        private void OK_Click(object sender, RoutedEventArgs e) => DialogResult = true;

        private void Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
