using StudentMobileApp.Data;
using StudentMobileApp.Models;

namespace StudentMobileApp.Views;

[QueryProperty(nameof(TermId), "termId")]
public partial class TermDetailPage : ContentPage
{
	public int TermId { get; set; }

	public TermDetailPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

		Term term =AppData.GetTerm(TermId);

		if (term != null)
		{
			TermTitleLabel.Text = term.TermTitle;
			StartDateLabel.Text = "Start: " + term.StartDate.ToShortDateString();
			EndDateLabel.Text = "End: " + term.EndDate.ToShortDateString();
		}

		var courses = AppData.GetCoursesForTerm(TermId);
		CoursesCollection.ItemsSource = courses;
    }

	public async void OnAddCourseClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync($"{nameof(AddEditCoursePage)}?termId={TermId}");
	}

    private async void OnCourseTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is int courseId)
        {
            await Shell.Current.GoToAsync($"{nameof(CourseDetailPage)}?courseId={courseId}");
        }
    }
}