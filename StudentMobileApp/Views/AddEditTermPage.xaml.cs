using StudentMobileApp.Data;
using StudentMobileApp.Models;

namespace StudentMobileApp.Views;

public partial class AddEditTermPage : ContentPage
{
	public AddEditTermPage()
	{
		InitializeComponent();
	}

	//Save button clicked
	private async void OnSaveClicked(object sender, EventArgs e)
	{
        //Validate term entry
        if (string.IsNullOrEmpty(TitleEntry.Text))
		{
			await DisplayAlert("Error", "Please enter a term title.", "OK");
			return;
		}

		if (EndDatePicker.Date < StartDatePicker.Date)
		{
			await DisplayAlert("Error", "End date cannot be before start date.", "OK");
			return;
		}

		//Create and save new term
        Term newTerm = new()
        {
            TermTitle = TitleEntry.Text,
            StartDate = StartDatePicker.Date,
            EndDate = EndDatePicker.Date
        };

        AppData.AddTerm(newTerm);

        //Navigate back to term list
        await Shell.Current.GoToAsync("..");
    }

	//Cancel button clicked
	private async void OnCancelClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}
}