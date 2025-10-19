using StudentMobileApp.Data;
using StudentMobileApp.Models;
using Plugin.LocalNotification;

namespace StudentMobileApp.Views;

[QueryProperty(nameof(CourseId), "courseId")]
[QueryProperty(nameof(AssessmentId), "assessmentId")]
public partial class AddEditAssessmentPage : ContentPage
{
    public int CourseId { get; set; }
    public int AssessmentId { get; set; }

    public AddEditAssessmentPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (AssessmentId > 0)
        {
            var assessment = AppData.GetAssessmentById(AssessmentId);
            if (assessment != null)
            {
                TitleEntry.Text = assessment.Title;
                TypePicker.SelectedItem = assessment.Type;
                StartDatePicker.Date = assessment.StartDate;
                EndDatePicker.Date = assessment.EndDate;
            }
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(TitleEntry.Text) || TypePicker.SelectedItem == null)
        {
            await DisplayAlert("Error", "Please fill all required fields.", "OK");
            return;
        }

        if (StartDatePicker.Date > EndDatePicker.Date)
        {
            await DisplayAlert("Error", "End date cannot be before start date", "OK");
            return;
        }

        Assessment newAssessment = new()
        {
            CourseId = CourseId,
            Title = TitleEntry.Text,
            Type = TypePicker.SelectedItem.ToString(),
            StartDate = StartDatePicker.Date,
            EndDate = EndDatePicker.Date
        };

        // --- Limit: one Performance & one Objective per course ---
        var existing = AppData.GetAssessmentsForCourse(CourseId);
        if (existing.Any(a => a.Type == newAssessment.Type && a.Id != AssessmentId))
        {
            await DisplayAlert("Limit Reached",
                $"This course already has a {newAssessment.Type} assessment.",
                "OK");
            return;
        }

        if (AssessmentId == 0)
            AppData.AddAssessment(newAssessment);
        else
            AppData.UpdateAssessment(newAssessment);

        // --- Notifications ---
        if (newAssessment.StartDate == DateTime.Today)
        {
            await LocalNotificationCenter.Current.Show(new NotificationRequest
            {
                NotificationId = new Random().Next(3000, 3999),
                Title = "Assessment Starts Today",
                Description = $"{newAssessment.Title} ({newAssessment.Type}) begins today."
            });
        }
        else
        {
            await LocalNotificationCenter.Current.Show(new NotificationRequest
            {
                NotificationId = new Random().Next(3000, 3999),
                Title = "Upcoming Assessment",
                Description = $"{newAssessment.Title} starts soon.",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = newAssessment.StartDate.AddHours(9)
                }
            });
        }

        if (newAssessment.EndDate == DateTime.Today)
        {
            await LocalNotificationCenter.Current.Show(new NotificationRequest
            {
                NotificationId = new Random().Next(4000, 4999),
                Title = "Assessment Ends Today",
                Description = $"{newAssessment.Title} ({newAssessment.Type}) ends today."
            });
        }
        else
        {
            await LocalNotificationCenter.Current.Show(new NotificationRequest
            {
                NotificationId = new Random().Next(4000, 4999),
                Title = "Assessment Ending Soon",
                Description = $"{newAssessment.Title} ends soon.",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = newAssessment.EndDate.AddHours(9)
                }
            });
        }

        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
