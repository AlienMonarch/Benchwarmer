using Benchwarmer.Resources.Code;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Benchwarmer.Resources.Pages;

public partial class Profile : ContentPage
{
	public Profile()
	{
        InitializeComponent();
        CSVmanager csv = new CSVmanager();
		Username.Text = csv.EncryptedRead("\\Users\\" + csv.EncryptedRead("\\Memory\\Memory.csv")[1].Split(',')[1] + "\\" + csv.EncryptedRead("\\Memory\\Memory.csv")[1].Split(',')[1] + ".csv")[0].Split(',')[0];
		//Reads what the currently logged in user is from the memory file and then finds that file in the users folder, opens the specific user file and reads the username, then sets the username as the text of the Username label object
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

    private async void DeleteAccountButton_Clicked(object sender, EventArgs e)
    {
        bool DeleteAccount = await DisplayAlert("Delete Account?", "Are You sure you want to delete your account and all associated data?", "Yes", "No");
        if (DeleteAccount)
		{
            CSVmanager csv = new CSVmanager();
            
            csv.Replace("\\Memory\\Users.csv", csv.EncryptedRead("\\Memory\\Memory.csv")[1].Split(',')[1], "", 1);
            csv.Replace("\\Memory\\Users.csv", csv.EncryptedRead("\\Memory\\Memory.csv")[1].Split(',')[1], "", 0);
            csv.Replace("\\Memory\\Memory.csv", "UserIsLoggedIn", "0", 1);
            csv.Replace("\\Memory\\Memory.csv", "CurrentUser", "Null", 1);
            csv.Replace("\\Memory\\Memory.csv", "CurrentTeam", "NoTeam", 1);

            

            App.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }

    private async void ChangeUsernameButton_Clicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("New Username", "What would you like your new Username to be?");
        string confirm = await DisplayPromptAsync("Confirm Username", "Please Confirm Your new Username");
        CSVmanager csv = new CSVmanager();
        if (result == confirm)
        {
            string username = result;
            csv.DeleteFile("\\Users" + csv.EncryptedRead("\\Memory\\Memory.csv")[1].Split(',')[0]);
            csv.CreateDirectory("\\Users\\" + username);
            csv.CreateFile("\\Users", username + "\\" + username + ".csv");
            csv.EncryptWrite("\\Users\\" + username + "\\" + username + ".csv", username + "," + csv.EncryptedRead("\\Memory\\Memory.csv")[1].Split(',')[1]);
            csv.Replace("\\Memory\\Users.csv", csv.EncryptedRead("\\Memory\\Memory.csv")[1].Split(',')[1], result, 0);
            csv.Replace("\\Memory\\Memory.csv", "CurrentUser", result, 1);
            csv.Replace("\\Memory\\Memory.csv", "UserIsLoggedIn", "1", 1);
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }
        else
        {
            await DisplayAlert("Usernames do not match", "Usernames do not match, try again", "ok");
        }

    }

    private async void ChangePasswordButton_Clicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("New Password", "What would you like your new Password to be?");
        string confirm = await DisplayPromptAsync("Confirm Password", "Please Confirm Your new Password");
        CSVmanager csv = new CSVmanager();
        if (result == confirm)
        {
            csv.Replace("\\Memory\\Users.csv", csv.EncryptedRead("\\Memory\\Memory.csv")[1].Split(',')[1], result, 0);
            csv.Replace("\\Memory\\Memory.csv", "UserIsLoggedIn", "1", 1);
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }
        else
        {
            await DisplayAlert("Passwords do not match", "Passwords do not match, try again", "ok");
        }
    }
}