using StudentMobileApp.Data;
using StudentMobileApp.Models;

namespace StudentMobileApp.Views;

[QueryProperty(nameof(CourseId), "courseId")]
public partial class AssessmentListPage : ContentPage
{
    public int CourseId { get; set; }

    public AssessmentListPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (CourseId == 0) return;

        var assessments = await Database.GetAssessmentsByCourseAsync(CourseId);
        AssessmentsCollection.ItemsSource = assessments;
    }

    private async void OnAddAssessmentClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(AddEditAssessmentPage)}?courseId={CourseId}");
    }

    private async void OnDeleteAssessmentClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int assessmentId)
        {
            bool confirm = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this assessment?", "Yes", "No");
            if (confirm)
            {
                AppData.DeleteAssessment(assessmentId);
                AssessmentsCollection.ItemsSource = null;
                AssessmentsCollection.ItemsSource = AppData.GetAssessmentsForCourse(CourseId);
            }
        }
    }

}
