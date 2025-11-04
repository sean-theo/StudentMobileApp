using StudentMobileApp.Data;
using StudentMobileApp.Models;

namespace StudentMobileApp.Views
{
    public partial class AddEditTermPage : ContentPage
    {
        private Term _term;
        private bool _isEditing;

        public AddEditTermPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _isEditing = _term != null && _term.Id != 0;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string title = TitleEntry.Text?.Trim();
            DateTime start = StartDatePicker.Date;
            DateTime end = EndDatePicker.Date;

            //VALIDATION
            if (!ValidationHelper.IsNotEmpty(title))
            {
                await DisplayAlert("Validation Error", "Please enter a term title.", "OK");
                return;
            }

            if (!ValidationHelper.IsDateRangeValid(start, end))
            {
                await DisplayAlert("Validation Error", "End date must be on or after the start date.", "OK");
                return;
            }

            //CREATE OR UPDATE
            if (_term == null)
                _term = new Term();

            _term.TermTitle = title;
            _term.StartDate = start;
            _term.EndDate = end;

            if (_isEditing)
                await Database.UpdateTermAsync(_term);
            else
                await Database.AddTermAsync(_term);

            await DisplayAlert("Success", $"Term \"{_term.TermTitle}\" saved successfully.", "OK");
            await Shell.Current.GoToAsync("..");
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (_term == null) return;

            bool confirm = await DisplayAlert("Confirm", "Are you sure you want to delete this term?", "Yes", "No");
            if (confirm)
            {
                await Database.DeleteTermAsync(_term);
                await DisplayAlert("Deleted", "The term was successfully deleted.", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
