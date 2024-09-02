using Benchwarmer.Resources.Code;

namespace Benchwarmer.Resources.Pages;

public partial class Home : ContentPage
{
    int count = 0;
    public Home()
    {
        InitializeComponent();
        TeamPicker.ItemsSource.Clear();
        CSVmanager csv = new CSVmanager();
        List<string> strings = csv.EncryptedRead("\\Users\\" //Directory
            + csv.EncryptedRead("\\Memory\\Memory.csv")[1].Split(',')[1] // + Username
            + "\\" + csv.EncryptedRead("\\Memory\\Memory.csv")[1].Split(',')[1] // + Username
            + ".csv");
        strings.RemoveAt(0);
        if (strings.Count >= 0)
            TeamPicker.ItemsSource = strings;
    }

    private void PlayGameButton_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new PlayGame());
    }

    private async void Picker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        CSVmanager csv = new CSVmanager();
        Picker TeamPicker = (Picker)sender;
        if (TeamPicker.ItemsSource.Contains("@ERROR@TEST@ERROR"))
        {
            TeamPicker.ItemsSource.Clear();
            List<string> strings = csv.EncryptedRead("\\Users\\" //Directory
            + csv.EncryptedRead("\\Memory\\Memory.csv")[1].Split(',')[1] // + Username
            + "\\" + csv.EncryptedRead("\\Memory\\Memory.csv")[1].Split(',')[1] // + Username
            + ".csv");
            strings.RemoveAt(0);
            if (strings.Count >= 0)
                TeamPicker.ItemsSource = strings;
        }
        if (TeamPicker.SelectedItem != null)
            csv.Replace("\\Memory\\Memory.csv", "CurrentTeam", TeamPicker.SelectedItem.ToString(), 1);
    }

    private async void LoggoutButton_Clicked(object sender, EventArgs e)
    {
        bool Loggout = await DisplayAlert("Loggout?", "Are you sure you want to Log out?", "Confirm", "Cancel");
        if (!Loggout) return;
        CSVmanager csv = new CSVmanager();
        csv.Replace("\\Memory\\Memory.csv", "UserIsLoggedIn", "0", 1);
        csv.Replace("\\Memory\\Memory.csv", "CurrentUser", "Null", 1);
        csv.Replace("\\Memory\\Memory.csv", "CurrentTeam", "NoTeam", 1);
        App.Current.MainPage = new NavigationPage(new LoginPage());
    }
}