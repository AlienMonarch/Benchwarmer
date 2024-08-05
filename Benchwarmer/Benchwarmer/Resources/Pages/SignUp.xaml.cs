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
        Encryption encryption = new Encryption();
        List<string> savedUsers = csvmanager.readCsv(@"\Resources\Users\Users.csv");
        savedUsers.Sort();
        bool founduser = false;
        foreach (string user in savedUsers)
        {
            if (user.Split(',')[0] == encryption.encrypt(username))
            {
                await DisplayAlert("User Already Exists", "User Already Exists. Use a Different Username", "OK");
                founduser = true;
            }
        }
        if (founduser == false)
        {
            csvmanager.editFile("\\Resources\\Memory\\Login.csv", "UserIsLoggedIn", "1", 1);
            csvmanager.writeCsv(@"\Resources\Users\Users.csv", new string[] {username + "," + password});
            App.Current.MainPage = new NavigationPage(new AppShell());
        }
    }
}