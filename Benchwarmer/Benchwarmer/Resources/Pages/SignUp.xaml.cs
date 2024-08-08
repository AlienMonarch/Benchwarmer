using Benchwarmer.Resources.Code;
namespace Benchwarmer.Resources.Pages;
using Konscious.Security.Cryptography;
using System.Text;

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
        List<string> savedUsers = csvmanager.readCsv("\\Memory\\Users.csv");
        savedUsers.Sort();
        bool founduser = false;
        foreach (string user in savedUsers)
        {
            var Argon = new Argon2i(Encoding.ASCII.GetBytes(user.Split(',')[0]));
            
            if (user.Split(',')[0] == Argon.ToString())
            {
                await DisplayAlert("User Already Exists", "User Already Exists. Use a Different Username", "OK");
                founduser = true;
            }
        }
        if (founduser == false)
        {
            csvmanager.writeCsv("\\Memory\\Users.csv", new string[] {new Argon2i(Encoding.ASCII.GetBytes(username)).ToString() + "," + new Argon2i(Encoding.ASCII.GetBytes(username)).ToString()});
            csvmanager.editFile("\\Memory\\Memory.csv", "UserIsLoggedIn", "1", 1);
            App.Current.MainPage = new NavigationPage(new AppShell());
        }
    }
}