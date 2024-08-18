using Benchwarmer.Resources.Code;
namespace Benchwarmer.Resources.Pages;

public partial class SignUp : ContentPage
{
	public SignUp()
	{
		InitializeComponent();
        CSVmanager cSVmanager = new CSVmanager();
        cSVmanager.EncryptedWrite("\\Memory\\Users.csv","Admin,Admin");
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
        List<string> savedUsers = csvmanager.EncryptedRead("\\Memory\\Users.csv");
        bool founduser = false;
        if (savedUsers.Count != 0)
        {
            if (username == null) throw new Exception("User is null");
            foreach (string user in savedUsers)
            {
                if (user.Split(',')[0] == username)
                {
                    await DisplayAlert("User Already Exists", "User Already Exists. Use a Different Username", "OK");
                    founduser = true;
                }
            }
        }
        else
        {
            await DisplayAlert("Problem", "There was a problem with the saved users file", "OK");
        }
        if (founduser == false)
        {
            csvmanager.EncryptedWrite("\\Memory\\Users.csv", username + "," + password);
            csvmanager.EncryptedEdit("\\Memory\\Memory.csv", "UserIsLoggedIn", "1", 1);
            csvmanager.EncryptedEdit("\\Memory\\Memory.csv", "User", username, 1);
            App.Current.MainPage = new NavigationPage(new AppShell());
        }
    }
}