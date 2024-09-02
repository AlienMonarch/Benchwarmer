using Benchwarmer.Resources.Code;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Benchwarmer.Resources.Pages;

public partial class EditTeam : ContentPage
{
    string originalName;
    private List<playerCard> playerCards =  new List<playerCard>();
    private async Task NoTeam()
    {
        await DisplayAlert("No Team Selected", "You need to select a team or create a new to to edit an existing team.", "Back");
        CSVmanager csv = new CSVmanager();
        csv.Replace("\\Memory\\Memory.csv", "UserIsLoggedIn", "1", 1);
        App.Current.MainPage = new NavigationPage(new AppShell());
    }
    public EditTeam()
	{
		InitializeComponent();
        CSVmanager csv = new CSVmanager();
        if (csv.EncryptedRead("\\Memory\\Memory.csv")[2].Split(',')[1] != "NoTeam")
        {
            string currentUser = csv.EncryptedRead("\\Memory\\Memory.csv")[1].Split(',')[1];
            string currentTeam = csv.EncryptedRead("\\Memory\\Memory.csv")[2].Split(',')[1];
            var team = csv.EncryptedRead("\\Users\\" + currentUser + "\\" + currentTeam + ".csv");
            originalName = team[0];
            
            TeamNameEntry.Text = team[0];
            
            for (int i = 1; i < team.Count; i++)
            {
                playerCard player = new playerCard(); //Create the playercard
                player.getPlayerName().Text = team[i].Split(',')[0]; //Add the name
                player.getPlayerSkill().Text = team[i].Split(',')[3]; //Add the Skill
                player.getPlayerNumber().Text = team[i].Split(',')[2]; //Add the player number
                player.getPlayerPosition().SelectedItem = team[i].Split(',')[1]; //Set the player Position
                playerCards.Add(player); //Add the created player to the playercards array
                PlayerCardArea.Children.Add(player.getGrid()); //Adds the playercard to the UI
            }
        }
        else
        {
            NoTeam();
        }
	}

    private void NewPlayerButton_Clicked(object sender, EventArgs e)
    {
        playerCard player = new playerCard();
        PlayerCardArea.Children.Add(player.getGrid());
        playerCards.Add(player);
    }
    private async void FinishEditTeam_Clicked(object sender, EventArgs e)
    {
        try
        {
            Team team = new Team(TeamNameEntry.Text);
            if (playerCards.Count < 11)
            { await DisplayAlert("Less than 11 players", "Team has less than 11 players", "OK"); return; }

            foreach (var card in playerCards)
            {
                if (card.getPlayerName().Text == null || card.getPlayerPosition().SelectedItem == null || card.getPlayerNumber().Text == null || card.getPlayerSkill().Text == null) //existance checking
                { await DisplayAlert("Null Fields", "One or more required fields are null", "Cancel"); return; }

                if (card.getPlayerSkill().Text != "1" && card.getPlayerSkill().Text != "2" && card.getPlayerSkill().Text != "3" && card.getPlayerSkill().Text != "4" && card.getPlayerSkill().Text != "5") //range and type checking
                { await DisplayAlert("Invalid Skill", "Invalid skill number for one or more players. Skill must be a number between 1 and 5. Current skill: " + card.getPlayerSkill().Text, "Cancel"); return; }

                team.addPlayer(new Player(card.getPlayerName().Text, card.getPlayerPosition().SelectedItem.ToString(), Convert.ToInt32(card.getPlayerNumber().Text), Convert.ToInt32(card.getPlayerSkill().Text)));
            }
            if (!await DisplayAlert("Confirm", "Finished Team? You can edit it later.", "Yes", "No")) return;
            CSVmanager csv = new CSVmanager();
            string username = csv.EncryptedRead("\\Memory\\Memory.csv")[1].Split(',')[1];
            string newTeamName = TeamNameEntry.Text;
            csv.DeleteFile("\\Users" + "\\" + username + "\\" + originalName + ".csv");
            csv.CreateFile("\\Users" + "\\" + username, "\\" + newTeamName + ".csv");
            csv.EncryptWrite("\\Users" + "\\" + username + "\\" + newTeamName + ".csv", newTeamName);
            foreach (Player player in team.GetPlayers())
            {
                csv.EncryptWrite("\\Users\\" + username + "\\" + newTeamName + ".csv", player.GetName() + "," + player.GetPosition() + "," + player.GetNumber() + "," + player.GetSkill() + "," + "0" + "," + "0" + "," + "0");
            }
            csv.Replace("\\Users\\" + username + "\\" +  username + ".csv", originalName, newTeamName, 0);
            csv.Replace("\\Memory\\Memory.csv", "CurrentTeam", newTeamName, 1);

            csv.Replace("\\Memory\\Memory.csv", "UserIsLoggedIn", "1", 1);
            App.Current.MainPage = new NavigationPage(new AppShell());
        }
        catch (Exception)
        {
            await DisplayAlert("Error", "Unknown Error" ,"Go Back");
            return;
        }
    }
}