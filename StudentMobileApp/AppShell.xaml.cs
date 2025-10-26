using StudentMobileApp.Views;

namespace StudentMobileApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddEditTermPage), typeof(AddEditTermPage));
            Routing.RegisterRoute(nameof(TermDetailPage), typeof(TermDetailPage));
            Routing.RegisterRoute(nameof(AddEditCoursePage), typeof(AddEditCoursePage));
            Routing.RegisterRoute(nameof(CourseDetailPage), typeof(CourseDetailPage));
            Routing.RegisterRoute(nameof(AddEditAssessmentPage), typeof(AddEditAssessmentPage));
        }

        public async Task LogoutAsync()
        {
            bool confirm = await Shell.Current.DisplayAlert("Logout", "Are you sure you want to log out?", "Yes", "No");
            if (confirm)
            {
                Preferences.Clear();
                Application.Current.MainPage = new NavigationPage(new Views.LoginPage());
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await LogoutAsync();
        }
    }
}
