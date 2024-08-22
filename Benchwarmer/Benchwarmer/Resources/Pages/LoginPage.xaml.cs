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
        string username = UsernameField.Text;
        string password = PasswordField.Text;
        List<string> users = csvmanager.Read("\\Memory\\Users.csv");
        users.Sort();
        foreach (string user in users)
        {
            if (user.Split(',')[0] == username && user.Split(',')[1] == password)
            {
                csvmanager.EditFile("\\Memory\\Memory.csv", "UserIsLoggedIn", "1", 1);
                UserClass userClass = new UserClass(username);
                App.Current.MainPage = new NavigationPage(new AppShell());
            }
            else if (user.Split(',')[0] == username && user.Split(',')[1] != password)
            {
                await DisplayAlert("Incorrect Password", "The password you have entered is incorrect", "OK");
            }
        }
        await DisplayAlert("Incorrect Username or Password", "The username or password you have entered is incorrect", "OK");

    }

    private void SignUpButton_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new SignUp());
    }
}