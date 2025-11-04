using StudentMobileApp.Data;
using StudentMobileApp.Models;

namespace StudentMobileApp.Views
{
    [QueryProperty(nameof(TermId), "termId")]
    public partial class AddEditCoursePage : ContentPage
    {
        public int TermId { get; set; }
        private Course _course;
        private bool _isEditing;

        public AddEditCoursePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _isEditing = _course != null && _course.Id != 0;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string title = CourseTitleEntry.Text?.Trim();
            string instructorName = InstructorNameEntry.Text?.Trim();
            string instructorPhone = InstructorPhoneEntry.Text?.Trim();
            string instructorEmail = InstructorEmailEntry.Text?.Trim();
            DateTime startDate = StartDatePicker.Date;
            DateTime endDate = EndDatePicker.Date;
            string status = StatusPicker.SelectedItem?.ToString();

            // ===== VALIDATION =====
            if (!ValidationHelper.IsNotEmpty(title))
            {
                await DisplayAlert("Validation Error", "Course title is required.", "OK");
                return;
            }

            if (!ValidationHelper.IsDateRangeValid(startDate, endDate))
            {
                await DisplayAlert("Validation Error", "End date must be on or after the start date.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(status))
            {
                await DisplayAlert("Validation Error", "Please select a course status.", "OK");
                return;
            }

            if (!ValidationHelper.IsNotEmpty(instructorName))
            {
                await DisplayAlert("Validation Error", "Instructor name is required.", "OK");
                return;
            }

            if (!ValidationHelper.IsValidPhone(instructorPhone))
            {
                await DisplayAlert("Validation Error", "Please enter a valid phone number (e.g., 555-123-4567).", "OK");
                return;
            }

            if (!ValidationHelper.IsValidEmail(instructorEmail))
            {
                await DisplayAlert("Validation Error", "Please enter a valid instructor email address.", "OK");
                return;
            }

            // ===== CREATE OR UPDATE =====
            if (_course == null)
                _course = new Course();

            _course.CourseTitle = title;
            _course.StartDate = startDate;
            _course.EndDate = endDate;
            _course.Status = status;
            _course.InstructorName = instructorName;
            _course.InstructorPhone = instructorPhone;
            _course.InstructorEmail = instructorEmail;
            _course.Notes = NotesEditor.Text?.Trim();
            _course.TermId = TermId;

            if (_isEditing)
                await Database.UpdateCourseAsync(_course);
            else
                await Database.AddCourseAsync(_course);

            await DisplayAlert("Success", $"Course \"{_course.CourseTitle}\" saved successfully.", "OK");
            await Shell.Current.GoToAsync("..");
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (_course == null) return;

            bool confirm = await DisplayAlert("Confirm", "Are you sure you want to delete this course?", "Yes", "No");
            if (confirm)
            {
                await Database.DeleteCourseAsync(_course);
                await DisplayAlert("Deleted", "The course was successfully deleted.", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
