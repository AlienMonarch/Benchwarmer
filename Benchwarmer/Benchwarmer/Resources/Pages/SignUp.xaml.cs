using Benchwarmer.Resources.Code;
using Benchwarmer;
namespace Benchwarmer.Resources.Pages;

public partial class SignUp : ContentPage
{
	public SignUp()
	{
		InitializeComponent();
        CSVmanager cSVmanager = new CSVmanager();
        cSVmanager.Write("\\Memory\\Users.csv","Admin,Admin");
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
        List<string> savedUsers = csvmanager.Read("\\Memory\\Users.csv");
        bool founduser = false;


        if (savedUsers.Count != 0)
        {
            if (username == null) throw new Exception("User is null");
            foreach (string user in savedUsers)
            {
                if (user == null) throw new Exception("User is null");
                if (user == username)
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
            Directory.CreateDirectory("\\Benchwarmer\\Resorces\\Users\\" + username);
            csvmanager.Write("\\Memory\\Users.csv", username + "," + password);
            csvmanager.EditFile("\\Memory\\Memory.csv", "UserIsLoggedIn", "1", 1);
            csvmanager.EditFile("\\Memory\\Memory.csv", "User", username, 1);
            csvmanager.Write("\\Users\\" + username, username + "," + password);
            App.Current.MainPage = new NavigationPage(new AppShell());
        }
    }
}