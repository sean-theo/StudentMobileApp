using StudentMobileApp.Data;

namespace StudentMobileApp.Views
{
    public partial class LoginPage : ContentPage
    {
        private const string DefaultUsername = "student";
        private const string DefaultPassword = "password123";

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text?.Trim();
            string password = PasswordEntry.Text;

            if (!ValidationHelper.IsNotEmpty(username) || !ValidationHelper.IsNotEmpty(password))
            {
                ShowError("Username and password are required.");
                return;
            }

            // In a real app, credentials would be verified via a backend API.
            string storedUsername = Preferences.Get("username", DefaultUsername);
            string storedPassword = Preferences.Get("password", DefaultPassword);

            if (username == storedUsername && password == storedPassword)
            {
                Preferences.Set("isLoggedIn", true);
                Application.Current.MainPage = new AppShell();  // go to main app
            }
            else
            {
                ShowError("Invalid username or password.");
            }
        }

        private void ShowError(string message)
        {
            ErrorLabel.Text = message;
            ErrorLabel.IsVisible = true;
        }
    }
}
