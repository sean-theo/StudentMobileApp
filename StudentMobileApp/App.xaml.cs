using StudentMobileApp.Models;
using StudentMobileApp.Views;
using StudentMobileApp.Data;

using Plugin.LocalNotification;

namespace StudentMobileApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            SeedDemoData(); // ✅ Add this line
        }

        private void SeedDemoData()
        {
            if (AppData.Terms.Count == 0)
            {
                // Create a term
                var term = new Term
                {
                    Id = 1,
                    TermTitle = "Spring 2025",
                    StartDate = new DateTime(2025, 1, 6),
                    EndDate = new DateTime(2025, 4, 26)
                };
                AppData.AddTerm(term);

                // Create a course
                var course = new Course
                {
                    Id = 1,
                    CourseTitle = "Mobile Application Development",
                    TermId = term.Id,
                    StartDate = new DateTime(2025, 1, 6),
                    EndDate = new DateTime(2025, 4, 26),
                    Status = "In Progress",
                    InstructorName = "Anika Patel",
                    InstructorPhone = "555-123-4567",
                    InstructorEmail = "anika.patel@strimeuniversity.edu"
                };
                AppData.AddCourse(course);

                // Add assessments
                var assessment1 = new Assessment
                {
                    Id = 1,
                    CourseId = course.Id,
                    Title = "Objective Assessment",
                    Type = "Objective",
                    StartDate = new DateTime(2025, 3, 10),
                    EndDate = new DateTime(2025, 3, 15)
                };

                var assessment2 = new Assessment
                {
                    Id = 2,
                    CourseId = course.Id,
                    Title = "Performance Assessment",
                    Type = "Performance",
                    StartDate = new DateTime(2025, 4, 1),
                    EndDate = new DateTime(2025, 4, 5)
                };

                AppData.AddAssessment(assessment1);
                AppData.AddAssessment(assessment2);
            }
        }

    }
}
