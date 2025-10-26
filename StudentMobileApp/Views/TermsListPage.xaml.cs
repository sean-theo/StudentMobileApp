using StudentMobileApp.Models;
using StudentMobileApp.Data;


namespace StudentMobileApp.Views;

public partial class TermsListPage : ContentPage
{
    public TermsListPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // load terms from the database
        var terms = await Database.GetTermsAsync();
        TermsCollection.ItemsSource = terms;

        System.Diagnostics.Debug.WriteLine($"Terms loaded: {terms.Count}");
    }

    private async void OnAddTermClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddEditTermPage));
    }

    private async void OnTermTapped(object sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is Term selectedTerm)
        {
            await Shell.Current.GoToAsync(nameof(TermDetailPage),
                new Dictionary<string, object> { { "termId", selectedTerm.Id } });
        }
    }
}
