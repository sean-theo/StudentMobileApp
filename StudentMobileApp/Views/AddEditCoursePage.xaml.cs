using StudentMobileApp.Data;
using StudentMobileApp.Models;
using Plugin.LocalNotification;

namespace StudentMobileApp.Views;

[QueryProperty(nameof(CourseId), "courseId")]
[QueryProperty(nameof(TermId), "termId")]
public partial class AddEditCoursePage : ContentPage
{
	public int CourseId { get; set; }
	public int TermId { get; set; }

	public AddEditCoursePage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
	{
		base.OnAppearing();

		if (CourseId > 0)
		{
			Course course = AppData.GetCourseById(CourseId);
			if (course != null)
			{
				CourseTitleEntry.Text = course.CourseTitle;
				StartDatePicker.Date = course.StartDate;
				EndDatePicker.Date = course.EndDate;
				StatusPicker.SelectedItem = course.Status;
				InstructorNameEntry.Text = course.InstructorName;
				InstructorPhoneEntry.Text = course.InstructorPhone;
				InstructorEmailEntry.Text = course.InstructorEmail;
			}
		}
	}

	private async void OnSaveClicked(object sender, EventArgs e)
	{
		//Check for empty title
		if (string.IsNullOrEmpty(CourseTitleEntry.Text))
		{
			await DisplayAlert("Error", "Please enter a course title", "OK");
			return;
		}

		//Verify date fields
		if (StartDatePicker.Date > EndDatePicker.Date)
		{
			await DisplayAlert("Error", "End date cannot be before start date", "OK");
			return;
		}

		//Check instructor information
		if (string.IsNullOrEmpty(InstructorNameEntry.Text) || string.IsNullOrEmpty(InstructorPhoneEntry.Text) || string.IsNullOrEmpty(InstructorEmailEntry.Text))
		{
			await DisplayAlert("Error", "Please fill in any missing instructor details", "OK");
			return;
		}

		//Ensure that email is valid
		if (!(InstructorEmailEntry.Text.Contains('@') && InstructorEmailEntry.Text.Contains('.')))
		{
			await DisplayAlert("Error", "Invalid email address", "OK");
			return;
		}

		if (CourseId == 0)
		{
            var coursesForTerm = AppData.GetCoursesForTerm(TermId);

            if (coursesForTerm.Count >= 6)
            {
                await DisplayAlert("Limit Reached", "You can only add up to six courses per term.", "OK");
                return;
            }

            Course newCourse = new()
			{
				CourseTitle = CourseTitleEntry.Text,
				TermId = TermId,
				StartDate = StartDatePicker.Date,
				EndDate = EndDatePicker.Date,
				Status = StatusPicker.SelectedItem?.ToString(),
				InstructorName = InstructorNameEntry.Text,
				InstructorPhone = InstructorPhoneEntry.Text,
				InstructorEmail = InstructorEmailEntry.Text,
			};
			AppData.AddCourse(newCourse);
		}
		else
		{
			Course course = AppData.GetCourseById(CourseId);
			if (course != null)
			{
				course.CourseTitle = CourseTitleEntry.Text;
				course.StartDate = StartDatePicker.Date;
				course.EndDate = EndDatePicker.Date;
				course.Status = StatusPicker.SelectedItem?.ToString();
				course.InstructorName = InstructorNameEntry.Text;
				course.InstructorPhone = InstructorPhoneEntry.Text;
				course.InstructorEmail = InstructorEmailEntry.Text;

				AppData.UpdateCourse(course);
			}
		}

        //time notifications
        DateTime today = DateTime.Today;

        if (StartDatePicker.Date.Date == today)
        {
            var startNotification = new NotificationRequest
            {
                NotificationId = new Random().Next(1000, 9999),
                Title = "Course Starting Today",
                Description = $"Your course {CourseTitleEntry.Text} starts today!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(5)
                }
            };
            await LocalNotificationCenter.Current.Show(startNotification);
        }

        if (EndDatePicker.Date.Date == today)
        {
            var endNotification = new NotificationRequest
            {
                NotificationId = new Random().Next(10000, 19999),
                Title = "Course Ending Today",
                Description = $"Your course {CourseTitleEntry.Text} ends today!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(10)
                }
            };
            await LocalNotificationCenter.Current.Show(endNotification);
        }

        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

}