using StudentMobileApp.Data;
using StudentMobileApp.Models;

namespace StudentMobileApp.Views;

[QueryProperty(nameof(CourseId), "courseId")]
public partial class CourseDetailPage : ContentPage
{
    public int CourseId { get; set; }

    private Course currentCourse;

    public CourseDetailPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        
        currentCourse = AppData.GetCourseById(CourseId);

        if (currentCourse != null)
        {
            CourseTitleLabel.Text = currentCourse.CourseTitle;
            StatusLabel.Text = $"Status: {currentCourse.Status}";
            StartDateLabel.Text = $"Start: {currentCourse.StartDate:MM/dd/yyyy}";
            EndDateLabel.Text = $"End: {currentCourse.EndDate:MM/dd/yyyy}";

            InstructorNameLabel.Text = $"Name: {currentCourse.InstructorName}";
            InstructorPhoneLabel.Text = $"Phone: {currentCourse.InstructorPhone}";
            InstructorEmailLabel.Text = $"Email: {currentCourse.InstructorEmail}";

            // Load assessments linked to this course
            var assessments = AppData.GetAssessmentsForCourse(CourseId);
            AssessmentsCollection.ItemsSource = assessments;
        }
    }

    private async void OnAddAssessmentClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(AddEditAssessmentPage)}?courseId={CourseId}");
    }

    private async void OnAssessmentTapped(object sender, TappedEventArgs e)
    {
        int assessmentId = (int)e.Parameter;
        await Shell.Current.GoToAsync($"{nameof(AddEditAssessmentPage)}?assessmentId={assessmentId}&courseId={CourseId}");
    }

    private async void OnEditCourseClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(AddEditCoursePage)}?courseId={CourseId}&termId={currentCourse.TermId}");
    }

    private async void OnDeleteCourseClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirm", "Are you sure you want to delete this course?", "Yes", "No");
        if (confirm)
        {
            AppData.DeleteCourse(CourseId);
            await Shell.Current.GoToAsync("..");
        }
    }

    private void OnSaveNotesClicked(object sender, EventArgs e)
    {
        
        currentCourse.Notes = NotesEditor.Text;
        DisplayAlert("Saved", "Notes saved successfully.", "OK");
    }

    private async void OnShareNotesClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(NotesEditor.Text))
        {
            await Share.Default.RequestAsync(new ShareTextRequest
            {
                Text = NotesEditor.Text,
                Title = "Share Course Notes"
            });
        }
        else
        {
            await DisplayAlert("No Notes", "There are no notes to share.", "OK");
        }
    }
}
