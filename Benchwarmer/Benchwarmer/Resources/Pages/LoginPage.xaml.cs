using Benchwarmer.Resources.Code;

namespace Benchwarmer.Resources.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        CSVmanager csvmanager = new CSVmanager();
        List<string> users = csvmanager.readCsv(@"\Resources\Users\Users.csv");
        users.Sort();
        if (users.BinarySearch(UsernameField.Text + "," + PasswordField.Text) >= 0)
        {
            csvmanager.editFile("\\Resources\\Memory\\Login.csv", "UserIsLoggedIn", "1", 1);
            App.Current.MainPage = new NavigationPage(new AppShell());
        }
        else 
        {
            await DisplayAlert("Username/Password Inccorect","You stupid motherfuker you got the username/password wrong", "OK");
        }
        
    }
}