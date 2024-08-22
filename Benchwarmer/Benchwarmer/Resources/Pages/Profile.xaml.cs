using Benchwarmer.Resources.Code;
using System.Diagnostics;

namespace Benchwarmer.Resources.Pages;

public partial class Profile : ContentPage
{
	public Profile()
	{
		CSVmanager csv = new CSVmanager();
		Username.Text = csv.Read
			("\\Users\\"
			+ csv.Read("\\Memory\\Memory"
			+ ".csv")
			[1].Split(',')[1])[0];
		//Reads what the currently logged in user is from the memory file and then finds that file in the users folder, opens the specific user file and reads the username, then sets the username as the text of the Username label object
		InitializeComponent();
	}

    private void LoggoutButton_Clicked(object sender, EventArgs e)
    {
		CSVmanager csv = new CSVmanager();
		csv.EditFile("\\Memory\\Memory", "UserIsLoggedIn", "0", 1);
        csv.EditFile("\\Memory\\Memory", "CurrentUser", "Null", 1);
        App.Current.MainPage = new NavigationPage(new LoginPage());
    }

    private async void DeleteAccountButton_Clicked(object sender, EventArgs e)
    {
        bool DeleteAccount = await DisplayAlert("Delete Account?", "Are You sure you want to delete your account and all associated data?", "Yes", "No");
        if (DeleteAccount)
		{
			await DisplayAlert("Not Yet Implemented","Hahahahaha do this later", "Shit");
		}
    }

    private async void ChangeUsernameButton_Clicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("New Username", "What would you like your new Username to be?");
        string confirm = await DisplayPromptAsync("Confirm Username", "Please Confirm Your new Username");
        if (result == confirm)
        {
            await DisplayAlert("Not Yet Implemented", "Hahahahaha do this later", "Shit");
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
        if (result == confirm)
        {
            await DisplayAlert("Not Yet Implemented", "Hahahahaha do this later", "Shit");
        }
        else
        {
            await DisplayAlert("Passwords do not match", "Passwords do not match, try again", "ok");
        }
    }
}