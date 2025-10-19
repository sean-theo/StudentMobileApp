using StudentMobileApp.Models;
using StudentMobileApp.Data;

namespace StudentMobileApp.Views;

public partial class TermsListPage : ContentPage
{
    public TermsListPage()
    {
        InitializeComponent();
        TermsCollection.ItemsSource = AppData.Terms;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        TermsCollection.ItemsSource = null;
        TermsCollection.ItemsSource = AppData.Terms;
        System.Diagnostics.Debug.WriteLine($"Terms loaded: {AppData.Terms.Count}");
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
