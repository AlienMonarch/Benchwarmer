using Benchwarmer.Resources.Code;
using Microsoft.Maui.Controls.Shapes;
using System.Net.Http.Metrics;

namespace Benchwarmer.Resources.Pages;

/* Create Team
 * Finn Tweedie 26/8/24
 * This page is where a user can create a new team
 */
public partial class CreateTeam : ContentPage
{
    private List<playerCard> playerCards = new List<playerCard>();
    private Team team;
	public CreateTeam()
	{
		InitializeComponent();
	}

    private void NewPlayerButton_Clicked(object sender, EventArgs e)
    {
		
        playerCard player = new playerCard();
        PlayerCardArea.Children.Add(player.getGrid());
        playerCards.Add(player);
    }

    private async void FinishCreateTeam_Clicked(object sender, EventArgs e)
    {
        try
        {
            FileNameChecker fileNameChecker = new FileNameChecker();
            if (!fileNameChecker.Check(TeamNameEntry.Text))
            {
                await DisplayAlert("Invalid Team Name", "The Name you have chosen is invalid", "OK");
                return;
            }
            string teamName = TeamNameEntry.Text;
            team = new Team(teamName);
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

            bool confirm = await DisplayAlert("Confirm", "Create New Team? You can edit it later.", "Yes", "No");
            if (!confirm) return;

            CSVmanager csv = new CSVmanager();
            string username = csv.EncryptedRead("\\Memory\\Memory.csv")[1].Split(',')[1]; //Gets the current users username
            csv.CreateFile("\\Users" + "\\" + username + "\\", teamName + ".csv"); //creates the team file
            csv.EncryptWrite("\\Users" + "\\" + username + "\\" + teamName + ".csv", teamName); //Writes the team name to the team file
            foreach (Player player in team.GetPlayers())
            {
                csv.EncryptWrite("\\Users\\" + username + "\\" + teamName + ".csv", player.GetName() + "," + player.GetPosition() + "," + player.GetNumber() + "," + player.GetSkill() + "," + "0" + "," + "0" + "," + "0"); //Save each player to the team file
            }
            csv.EncryptWrite("\\Users" + "\\" + username + "\\" + username + ".csv", teamName); //Write the team name to the user file.

            csv.Replace("\\Memory\\Memory.csv", "CurrentTeam", teamName, 1); //Changes the current team to the newly created one

            csv.Replace("\\Memory\\Memory.csv", "UserIsLoggedIn", "1", 1); //Makes sure the current user will stay logged in
            App.Current.MainPage = new NavigationPage(new AppShell()); //Takes the user back to the home page and reloads all pages to have the new team available.
        }
        catch (Exception)
        {
            await DisplayAlert("Error", "Error", "Go Back");
            return;
        }
    }
}

public class playerCard
{
	private Grid grid;
	private ColumnDefinition[] columns;
	private RowDefinition[] rows;
	private Label text;
	private Entry playerName;
	private Entry playerSkill;
	private Picker playerPosition;
	private Entry playerNumber;
    private Border border;
	public playerCard()
	{
        border = new Border()
        {
            Stroke = Colors.Black,
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(0, 0, 0, 0)
            },
            Padding = new Thickness(10, 10, 10, 10),
        };
        grid = new Grid();
        border.Content = grid;
        grid.ColumnDefinitions.Clear();
        grid.RowDefinitions.Clear();

        ColumnDefinition column = new ColumnDefinition(width: 250);
        grid.ColumnDefinitions.Add(column);

        ColumnDefinition column2 = new ColumnDefinition(width: 750);
        grid.ColumnDefinitions.Add(column2);

        for (int i = 0; i < 4; i++)
        {
            RowDefinition row = new RowDefinition(height: 75);
            grid.RowDefinitions.Add(row);
        }

        int margin = 20;
        Label NameLabel = new Label()
        {
            Margin = margin,
            TextColor = Colors.Black,
            Text = "Name:"
        };
        grid.Children.Add(NameLabel);
        grid.SetRow(NameLabel, 0);
        grid.SetColumn(NameLabel, 0);

        Label SkillLabel = new Label()
        {
            Margin = margin,
            TextColor = Colors.Black,
            Text = "Skill (1-5):"
        };
        grid.Children.Add(SkillLabel);
        grid.SetRow(SkillLabel, 1);
        grid.SetColumn(SkillLabel, 0);

        Label PositionLabel = new Label()
        {
            Margin = margin,
            TextColor = Colors.Black,
            Text = "Position:"
        };
        grid.Children.Add(PositionLabel);
        grid.SetRow(PositionLabel, 2);
        grid.SetColumn(PositionLabel, 0);

        Label NumberLabel = new Label()
        {
            Margin = margin,
            TextColor = Colors.Black,
            Text = "Number (1-100):"
        };
        grid.Children.Add(NumberLabel);
        grid.SetRow(NumberLabel, 3);
        grid.SetColumn(NumberLabel, 0);

        playerName = new Entry()
        {
            TextColor = Colors.Black,
            BackgroundColor = new Color(241, 227, 228),
        };
        grid.Children.Add(playerName);
        grid.SetRow(playerName, 0);
        grid.SetColumn(playerName, 1);

        playerSkill = new Entry()
        {
            TextColor = Colors.Black,
            BackgroundColor = new Color(241, 227, 228),
        };
        grid.Children.Add(playerSkill);
        grid.SetRow(playerSkill, 1);
        grid.SetColumn(playerSkill, 1);

        playerPosition = new Picker()
        {
            TextColor = Colors.Black,
            BackgroundColor = new Color(241, 227, 228),
            ItemsSource = new string[] {
            "RW", "LW", "CF",
            "RI", "LI",
            "RHB", "LHB", "CHB",
            "CB",
            "G" }
        };
        grid.Children.Add(playerPosition);
        grid.SetRow(playerPosition, 2);
        grid.SetColumn(playerPosition, 1);

        playerNumber = new Entry()
        {
            TextColor = Colors.Black,
            BackgroundColor = new Color(241, 227, 228)
        };
        grid.Children.Add(playerNumber);
        grid.SetRow(playerNumber, 3);
        grid.SetColumn(playerNumber, 1);
        grid.RowDefinitions.Add(new RowDefinition(height:10));
        grid.SetRow(new Label()
        {
            Text = " "
        }, 4);
    }



    public Grid getGrid() { return grid; }
    public Label getText() { return text; }
    public Entry getPlayerName() { return playerName; }
    public Entry getPlayerSkill() { return playerSkill; }
    public Picker getPlayerPosition() { return playerPosition; }
    public Entry getPlayerNumber() { return playerNumber; }
}