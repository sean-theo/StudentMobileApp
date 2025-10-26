using StudentMobileApp.Data;
using StudentMobileApp.Models;

namespace StudentMobileApp.Views;

[QueryProperty(nameof(TermId), "termId")]
public partial class TermDetailPage : ContentPage
{
	public int TermId { get; set; }
    private Term _term;

	public TermDetailPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (TermId == 0)
            return;

        var allTerms = await Database.GetTermsAsync();
        _term = allTerms.FirstOrDefault(t => t.Id == TermId);

        if (_term == null)
            return;

        var courses = await Database.GetCoursesByTermAsync(_term.Id);
        CoursesCollection.ItemsSource = courses;

        System.Diagnostics.Debug.WriteLine($"Loaded {_term.TermTitle} with {courses.Count} courses.");
    }

	public async void OnAddCourseClicked(object sender, EventArgs e)
	{
        if (_term == null)
            return;

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