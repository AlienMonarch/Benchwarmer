using Benchwarmer.Resources.Code;

namespace Benchwarmer.Resources.Pages;

public partial class SignUp : ContentPage
{
	public SignUp()
	{
		InitializeComponent();
	}

    private void LoginButton_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new LoginPage());
    }

    private async void SignUpButton_Clicked(object sender, EventArgs e)
    {
        string username = UsernameField.Text;
        string password = PasswordField.Text;
        CSVmanager csvmanager = new CSVmanager();
        List<string> users = csvmanager.readCsv(@"\Resources\Users\Users.csv");
        users.Sort();
        if (users.BinarySearch(username + "," + password) >= 0)
        {
            await DisplayAlert("User Already Exists", "THIS IS NOT FINAL. User already exists", "Back");
        }
        else
        {
            csvmanager.editFile("\\Resources\\Memory\\Login.csv", "UserIsLoggedIn", "1", 1);
            csvmanager.writeCsv(@"\Resources\Users\Users.csv", new string[] { username + "," + password });
            App.Current.MainPage = new NavigationPage(new AppShell());
        }
    }
}