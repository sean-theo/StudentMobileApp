using StudentMobileApp.Data;
using StudentMobileApp.Models;

namespace StudentMobileApp.Views
{
    [QueryProperty(nameof(CourseId), "courseId")]
    public partial class CourseDetailPage : ContentPage
    {
        public int CourseId { get; set; }
        private Course _course;

        public CourseDetailPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (CourseId == 0)
                return;

            //Load the selected course from the database
            var allCourses = await Database.GetCoursesAsync();
            _course = allCourses.FirstOrDefault(c => c.Id == CourseId);

            if (_course == null)
            {
                await DisplayAlert("Error", "Course not found.", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }

            //Display course details
            CourseTitleLabel.Text = _course.CourseTitle;
            StatusLabel.Text = _course.Status;
            InstructorNameLabel.Text = _course.InstructorName;
            InstructorPhoneLabel.Text = _course.InstructorPhone;
            InstructorEmailLabel.Text = _course.InstructorEmail;
            NotesEditor.Text = _course.Notes;

            //Load assessments for this course
            var assessments = await Database.GetAssessmentsByCourseAsync(_course.Id);
            AssessmentsCollection.ItemsSource = assessments;
        }

        private async void OnEditCourseClicked(object sender, EventArgs e)
        {
            if (_course == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(AddEditCoursePage)}?courseId={_course.Id}");
        }

        private async void OnDeleteCourseClicked(object sender, EventArgs e)
        {
            if (_course == null)
                return;

            bool confirm = await DisplayAlert("Confirm", "Delete this course?", "Yes", "No");
            if (confirm)
            {
                await Database.DeleteCourseAsync(_course);
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnAddAssessmentClicked(object sender, EventArgs e)
        {
            if (_course == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(AddEditAssessmentPage)}?courseId={_course.Id}");
        }

        private async void OnAssessmentTapped(object sender, EventArgs e)
        {
            // Determine which assessment was tapped
            if (sender is Frame frame && frame.BindingContext is Assessment selectedAssessment)
            {
                await Shell.Current.GoToAsync($"{nameof(AddEditAssessmentPage)}?courseId={selectedAssessment.CourseId}");
            }
        }

        private async void OnSaveNotesClicked(object sender, EventArgs e)
        {
            if (_course == null)
                return;

            _course.Notes = NotesEditor.Text?.Trim();
            await Database.UpdateCourseAsync(_course);

            await DisplayAlert("Saved", "Notes updated successfully.", "OK");
        }
        private async void OnShareNotesClicked(object sender, EventArgs e)
        {
            if (_course == null)
                return;

            if (string.IsNullOrWhiteSpace(_course.Notes))
            {
                await DisplayAlert("No Notes", "There are no notes to share.", "OK");
                return;
            }

            // Use the built-in MAUI Share API
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = _course.Notes,
                Title = $"Share Notes for {_course.CourseTitle}"
            });
        }
    }
}
