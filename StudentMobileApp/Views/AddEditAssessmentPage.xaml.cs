using StudentMobileApp.Data;
using StudentMobileApp.Models;

namespace StudentMobileApp.Views
{
    [QueryProperty(nameof(CourseId), "courseId")]
    public partial class AddEditAssessmentPage : ContentPage
    {
        public int CourseId { get; set; }
        private Assessment _assessment;
        private bool _isEditing;

        public AddEditAssessmentPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _isEditing = _assessment != null && _assessment.Id != 0;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string title = TitleEntry.Text?.Trim();
            string type = TypePicker.SelectedItem?.ToString();
            DateTime startDate = StartDatePicker.Date;
            DateTime endDate = EndDatePicker.Date;

            // ===== VALIDATION =====
            if (!ValidationHelper.IsNotEmpty(title))
            {
                await DisplayAlert("Validation Error", "Please enter an assessment title.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(type))
            {
                await DisplayAlert("Validation Error", "Please select an assessment type.", "OK");
                return;
            }

            if (!ValidationHelper.IsDateRangeValid(startDate, endDate))
            {
                await DisplayAlert("Validation Error", "End date must be on or after the start date.", "OK");
                return;
            }

            // ===== CREATE OR UPDATE =====
            if (_assessment == null)
                _assessment = new Assessment();

            _assessment.Title = title;
            _assessment.Type = type;
            _assessment.StartDate = startDate;
            _assessment.EndDate = endDate;
            _assessment.CourseId = CourseId;

            if (_isEditing)
                await Database.UpdateAssessmentAsync(_assessment);
            else
                await Database.AddAssessmentAsync(_assessment);

            await DisplayAlert("Success", $"Assessment \"{_assessment.Title}\" saved successfully.", "OK");
            await Shell.Current.GoToAsync("..");
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (_assessment == null) return;

            bool confirm = await DisplayAlert("Confirm", "Are you sure you want to delete this assessment?", "Yes", "No");
            if (confirm)
            {
                await Database.DeleteAssessmentAsync(_assessment);
                await DisplayAlert("Deleted", "The assessment was successfully deleted.", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
