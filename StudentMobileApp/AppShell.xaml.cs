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
    }
}
