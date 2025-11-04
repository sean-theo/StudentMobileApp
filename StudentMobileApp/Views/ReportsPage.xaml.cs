using StudentMobileApp.Data;
using StudentMobileApp.Models;
using System.Text;

namespace StudentMobileApp.Views
{
    public partial class ReportsPage : ContentPage
    {
        public ReportsPage()
        {
            InitializeComponent();
        }

        private async void OnGenerateReportClicked(object sender, EventArgs e)
        {
            var terms = await Database.GetTermsAsync();
            var courses = await Database.GetCoursesAsync();
            var assessments = await Database.GetAssessmentsAsync();

            var sb = new StringBuilder();
            sb.AppendLine("STUDENT MOBILE APP REPORT");
            sb.AppendLine($"Generated: {DateTime.Now:G}");
            sb.AppendLine();

            //Terms Section
            sb.AppendLine("TERMS");
            sb.AppendLine("Title | Start Date | End Date");
            foreach (var t in terms)
                sb.AppendLine($"{t.TermTitle} | {t.StartDate:MM/dd/yyyy} | {t.EndDate:MM/dd/yyyy}");
            sb.AppendLine();

            //Courses Section
            sb.AppendLine("COURSES");
            sb.AppendLine("Title | Status | Instructor | Term");
            foreach (var c in courses)
            {
                var term = terms.FirstOrDefault(t => t.Id == c.TermId);
                string termName = term != null ? term.TermTitle : "(No Term)";
                sb.AppendLine($"{c.CourseTitle} | {c.Status} | {c.InstructorName} | {termName}");
            }
            sb.AppendLine();

            //Assessments Section 
            sb.AppendLine("ASSESSMENTS");
            sb.AppendLine("Title | Type | Start Date | End Date | Course");
            foreach (var a in assessments)
            {
                var course = courses.FirstOrDefault(c => c.Id == a.CourseId);
                string courseName = course != null ? course.CourseTitle : "(No Course)";
                sb.AppendLine($"{a.Title} | {a.Type} | {a.StartDate:MM/dd/yyyy} | {a.EndDate:MM/dd/yyyy} | {courseName}");
            }
            sb.AppendLine();

            string reportText = sb.ToString();
            ReportViewer.Text = reportText;

        }
    }
}
