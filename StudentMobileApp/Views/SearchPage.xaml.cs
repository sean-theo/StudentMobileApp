using StudentMobileApp.Data;
using StudentMobileApp.Models;

namespace StudentMobileApp.Views
{
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private async void OnSearchClicked(object sender, EventArgs e)
        {
            string query = SearchBar.Text?.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(query))
            {
                await DisplayAlert("Empty Search", "Please enter a keyword.", "OK");
                return;
            }

            // Fetch all data from the database
            var terms = await Database.GetTermsAsync();
            var courses = await Database.GetCoursesAsync();
            var assessments = await Database.GetAssessmentsAsync();

            // Search through them using LINQ
            var termResults = terms
                .Where(t => t.TermTitle.ToLower().Contains(query))
                .Select(t => new SearchResult
                {
                    Type = "Term",
                    DisplayText = $"Term: {t.TermTitle} ({t.StartDate:MM/dd/yyyy} - {t.EndDate:MM/dd/yyyy})"
                });

            var courseResults = courses
                .Where(c => c.CourseTitle.ToLower().Contains(query) || c.InstructorName.ToLower().Contains(query))
                .Select(c => new SearchResult
                {
                    Type = "Course",
                    DisplayText = $"Course: {c.CourseTitle} - Instructor: {c.InstructorName}"
                });

            var assessmentResults = assessments
                .Where(a => a.Title.ToLower().Contains(query))
                .Select(a => new SearchResult
                {
                    Type = "Assessment",
                    DisplayText = $"Assessment: {a.Title} ({a.Type})"
                });

            // Combine results
            var results = termResults.Concat(courseResults).Concat(assessmentResults).ToList();

            if (results.Count == 0)
            {
                await DisplayAlert("No Results", "No matching records found.", "OK");
                ResultsCollectionView.ItemsSource = null;
            }
            else
            {
                ResultsCollectionView.ItemsSource = results;
            }
        }

        private class SearchResult
        {
            public string Type { get; set; }
            public string DisplayText { get; set; }
        }
    }
}
