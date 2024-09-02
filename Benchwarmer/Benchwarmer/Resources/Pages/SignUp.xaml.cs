using Benchwarmer.Resources.Code;
using Benchwarmer;
namespace Benchwarmer.Resources.Pages;

public partial class SignUp : ContentPage
{
	public SignUp()
	{
		InitializeComponent();
        CSVmanager cSVmanager = new CSVmanager();
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
        FileNameChecker fn = new FileNameChecker();

        if (savedUsers.Count != 0)
        {
            if (username == null) { await DisplayAlert("Username is null", "Username is Null", "OK"); return; }
            if (password == null) { await DisplayAlert("Password is null", "password is Null", "OK"); return; }
            if (!fn.Check(username)) { await DisplayAlert("Username is Invalid", "Username is Invalid", "OK"); return; }
            if (!fn.Check(username)) { await DisplayAlert("Password is Invalid", "Password is Invalid", "OK"); return; }
            foreach (string user in savedUsers)
            {
                if (user == null) await DisplayAlert("User is null", "Username is Null", "OK");
                if (user == username)
                {
                    await DisplayAlert("User Already Exists", "User Already Exists. Use a Different Username", "OK");
                    founduser = true;
                }
            }
        }
        else
        {
            username = "Admin";
            password = "Admin";
            await DisplayAlert("Problem", "There was a problem with the saved users file", "OK");
        }

        if (founduser == false)
        {
            csvmanager.CreateDirectory("\\Users\\" + username);
            csvmanager.EncryptWrite("\\Memory\\Users.csv", username + "," + password);
            csvmanager.Replace("\\Memory\\Memory.csv", "CurrentUser", username, 1);
            csvmanager.CreateFile("\\Users", username + "\\" + username + ".csv");
            csvmanager.EncryptWrite("\\Users\\" + username + "\\" + username + ".csv", username + "," + password);
            csvmanager.Replace("\\Memory\\Memory.csv", "UserIsLoggedIn", "1", 1);
            App.Current.MainPage = new NavigationPage(new AppShell());
        }
    }
}