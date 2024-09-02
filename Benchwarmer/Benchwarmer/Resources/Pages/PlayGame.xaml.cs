using Benchwarmer.Resources.Code;
using Microsoft.Maui.Animations;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Benchwarmer.Resources.Pages;

/* Play Game
 * Finn Tweedie
 * This is the page where users will play the game and where the playtime calculations are made.
 */
public partial class PlayGame : ContentPage
{
    bool isGamePlaying = false;
    IDispatcherTimer ticker;
    Stopwatch gameTime;
    string[][] positions = new string[5][] {
            new string[] { "RW", "LW", "CF" },
            new string[] { "RI", "LI" },
            new string[] { "RHB", "LHB", "CHB"},
            new string[] { "CB" },
            new string[] { "G" }
    };
    string[] players;
    Dictionary<GamePlayer, string> playerNames = new Dictionary<GamePlayer, string>();
    List<GamePlayer> playerList = new List<GamePlayer>();
    List<GamePlayer> Forwards = new List<GamePlayer>();
    List<GamePlayer> Mids = new List<GamePlayer>();
    List<GamePlayer> HalfBacks = new List<GamePlayer>();
    List<GamePlayer> Backs = new List<GamePlayer>();
    List<GamePlayer> Golies = new List<GamePlayer>();
    List<GamePlayer> OnField = new List<GamePlayer>();
    List<GamePlayer> OnBench = new List<GamePlayer>();
    List<string> names = new List<string>();
    public PlayGame()
    {
        InitializeComponent();
        CSVmanager csv = new CSVmanager();

        string currentTeam = csv.EncryptedRead("\\Memory\\Memory.csv")[2].Split(',')[1]; //Gets the current team from memory
        if (currentTeam.Contains("NoTeam"))
        {
            NoTeam();
        }
        try
        {
            players = csv.EncryptedRead("\\Users\\" //Reading from a user file
           + csv.EncryptedRead("\\Memory\\Memory.csv")[1].Split(',')[1] + "\\" //Username, got from CurrentUser
           + csv.EncryptedRead("\\Memory\\Memory.csv")[2].Split(',')[1] //Teamname, got from CurrentTeam
           + ".csv").ToArray(); //Convert to an array for memory

            Team team = new Team(currentTeam);
            for (int i = 1; i < players.Length; i++)
            {
                string[] arr = players[i].Split(',');
                Player player = new Player(arr[0], arr[1], Convert.ToInt32(arr[2]), Convert.ToInt32(arr[3]));
                team.addPlayer(player);
                GamePlayer gamePlayer = new GamePlayer(PlayersGrid, player, i - 1);
                gamePlayer.GetButton().Clicked += async (sender, e) =>
                {
                    await sub(playerList[gamePlayer.place()], playerList[names.IndexOf(gamePlayer.getPicker().SelectedItem.ToString())]);
                };
                names.Add(arr[0]);
                playerList.Add(gamePlayer);
            }

            for (int i = 0; i < playerList.Count; i++)
            {
                if (i < 11)
                {
                    OnField.Add(playerList[i]);
                }
                else if (i >= 11)
                {
                    OnBench.Add(playerList[i]);
                }
                playerList[i].getPicker().ItemsSource = names;
                playerList[i].getPicker().SelectedIndex = i;
            }
        }
        catch (Exception)
        {
            error();
        }
        calculatePositionGroups();
    }

    private async void error()
    {
        await DisplayAlert("Error", "Something went wrong", "OK");
        CSVmanager csv = new CSVmanager();
        csv.Replace("\\Memory\\Memory.csv", "UserIsLoggedIn", "1", 1);
        App.Current.MainPage = new NavigationPage(new AppShell());
    }
    private async void NoTeam()
    {
        await DisplayAlert("No team selected", "Select a team first", "OK");
        CSVmanager csv = new CSVmanager();
        csv.Replace("\\Memory\\Memory.csv", "UserIsLoggedIn", "1", 1);
        App.Current.MainPage = new NavigationPage(new AppShell());
    }

    private async Task sub(GamePlayer player1, GamePlayer player2)
    {
        if (await DisplayAlert("Sub?", "Do you want to sub this player? " + player1.getName().Text + " will be subbed for " + player2.getName().Text, "Yes", "No"))
        {
            int temp = player1.place();
            names[player1.place()] = names[player2.place()];
            names[player2.place()] = names[temp];
            playerList[player1.place()] = playerList[player2.place()];
            playerList[player2.place()] = playerList[temp];
            if (OnField.Contains(player1) && OnField.Contains(player2))
            {
                player1.manSub(player2.place());
                player2.manSub(temp);
            }
            else if (OnField.Contains(player1) && !OnField.Contains(player2))
            {
                player1.getTime().Stop();
                player2.getTime().Start();
                OnField.Remove(player1);
                OnField.Add(player2);
                OnBench.Remove(player2);
                OnBench.Add(player1);
                player1.sub(player2.place());
                player2.sub(temp);
            }
            else if (!OnField.Contains(player1) && OnField.Contains(player2))
            {
                player2.getTime().Stop();
                player1.getTime().Start();
                OnField.Remove(player2);
                OnField.Add(player1);
                OnBench.Remove(player1);
                OnBench.Add(player2);
                player1.sub(player2.place());
                player2.sub(temp);
            }
            else if (!OnField.Contains(player1) && !OnField.Contains(player2))
            {
                player1.manSub(player2.place());
                player2.manSub(temp);
            }
            string[] temp1 = players;
            int i = 1;
            foreach (string str in temp1)
            {
                temp1[i] = str.Split(',')[0];
                i++;
            }
            for (i = 0; i < playerList.Count; i++)
            {
                playerList[i].getPicker().ItemsSource = temp1;
            }
        }
    }

    private async void suggestsub(GamePlayer player, GamePlayer player2)
    {
        player.getPicker().SelectedItem = player2.getName().Text;
        player2.getPicker().SelectedItem = player.getName().Text;
    }
    private async void calculatePositionGroups()
    {
        foreach (var player in playerList)
        {
            if (positions[0].Contains(player.getPosition().Text))
            {
                Forwards.Add(player);
            }
            else if (positions[1].Contains(player.getPosition().Text))
            {
                Mids.Add(player);
            }
            else if (positions[2].Contains(player.getPosition().Text))
            {
                HalfBacks.Add(player);
            }
            else if (positions[3].Contains(player.getPosition().Text))
            {
                Backs.Add(player);
            }
            else if (positions[4].Contains(player.getPosition().Text))
            {
                Golies.Add(player);
            }
            else
            {
                await DisplayAlert("Player has an ineligible position", "Player at position: " + player.getPosition().Text + " Has an ineligible position", "OK");
                return;
            }
        }
        if (playerList.Count < 11 || Forwards.Count < 3 || Mids.Count < 2 || HalfBacks.Count < 3 || Backs.Count < 3 || Golies.Count == 0)
        {
            await DisplayAlert("Not enough Players","Not enough players in one or more positions","Go Back");
            CSVmanager csv = new CSVmanager();
            csv.Replace("\\Memory\\Memory.csv", "UserIsLoggedIn", "1", 1);
            App.Current.MainPage = new NavigationPage(new AppShell());
        }
    }

    private void DoAllPendingSubs()
    {
        foreach (var player in playerList)
        {
            sub(player, playerList[names.IndexOf(player.getPicker().SelectedItem.ToString())]);
        }
    }

	private async void Update()
    {
        if (!isGamePlaying)
            return;

	    GameTimer.Text = gameTime.Elapsed.ToString().Split('.')[0];
        foreach (var player in playerList)
        { 
            player.getTimerLabel().Text = player.getTime().Elapsed.ToString().Split('.')[0];
        }

        if (gameTime.Elapsed.TotalSeconds == 5 * 60 || gameTime.Elapsed.TotalSeconds == 10 * 60 || gameTime.Elapsed.TotalSeconds == 15 * 60 || gameTime.Elapsed.TotalSeconds == 25 * 60 || gameTime.Elapsed.TotalSeconds == 30 * 60 || gameTime.Elapsed.TotalSeconds == 35 * 60 )
        {
            string allSubs = "Confirm Subs?";
            foreach (var player in Forwards)
            {
                if (OnBench.Contains(player))
                {
                    List<GamePlayer> possibleSubs = new List<GamePlayer>();
                    foreach (var player2 in OnField)
                    {
                        if (Forwards.Contains(player2))
                        {
                            possibleSubs.Add(player2);
                        }
                    }
                    int min_idx = 0;
                    bool allTimerEqual = true;
                    for (int j = 0 + 1; j < possibleSubs.Count; j++)
                    {
                        if (possibleSubs[j].getTime().ElapsedMilliseconds != possibleSubs[min_idx].getTime().ElapsedMilliseconds)
                            allTimerEqual = false;
                        if (possibleSubs[j].getTime().ElapsedMilliseconds < possibleSubs[min_idx].getTime().ElapsedMilliseconds)
                            min_idx = j;
                    }
                    if (!allTimerEqual)
                    {
                        suggestsub(player, possibleSubs[min_idx]);
                        allSubs += "\n" + player + "will be subbed for " + possibleSubs[min_idx];
                    }

                    min_idx = 0;
                    bool allSkillEqual = true;
                    for (int j = 0 + 1; j < possibleSubs.Count; j++)
                    {
                        if (possibleSubs[j].getSkill() != possibleSubs[min_idx].getSkill())
                            allSkillEqual = false;
                        if (possibleSubs[j].getSkill() < possibleSubs[min_idx].getSkill())
                            min_idx = j;
                    }
                    if (!allSkillEqual)
                    {
                        suggestsub(player, possibleSubs[min_idx]);
                        allSubs += "\n" + player + "will be subbed for " + possibleSubs[min_idx];
                    }
                }
            }

            foreach (var player in Mids)
            {
                if (OnField.Contains(player))
                {
                    List<GamePlayer> possibleSubs = new List<GamePlayer>();
                    foreach (var player2 in OnField)
                    {
                        if (Mids.Contains(player2))
                        {
                            possibleSubs.Add(player2);
                        }
                    }
                    int min_idx = 0;
                    bool allTimerEqual = true;
                    for (int j = 0 + 1; j < possibleSubs.Count; j++)
                    {
                        if (possibleSubs[j].getTime().ElapsedMilliseconds != possibleSubs[min_idx].getTime().ElapsedMilliseconds)
                            allTimerEqual = false;
                        if (possibleSubs[j].getTime().ElapsedMilliseconds < possibleSubs[min_idx].getTime().ElapsedMilliseconds)
                            min_idx = j;
                    }
                    if (!allTimerEqual)
                    {
                        suggestsub(player, possibleSubs[min_idx]);
                        allSubs += "\n" + player + "will be subbed for " + possibleSubs[min_idx];
                    }


                    min_idx = 0;
                    bool allSkillEqual = true;
                    for (int j = 0 + 1; j < possibleSubs.Count; j++)
                    {
                        if (possibleSubs[j].getSkill() != possibleSubs[min_idx].getSkill())
                            allSkillEqual = false;
                        if (possibleSubs[j].getSkill() < possibleSubs[min_idx].getSkill())
                            min_idx = j;
                    }
                    if (!allSkillEqual)
                    {
                        suggestsub(player, possibleSubs[min_idx]);
                        allSubs += "\n" + player + "will be subbed for " + possibleSubs[min_idx];
                    }
                }
            }
            foreach (var player in Backs)
            {
                if (OnField.Contains(player))
                {
                    List<GamePlayer> possibleSubs = new List<GamePlayer>();
                    foreach (var player2 in OnField)
                    {
                        if (Backs.Contains(player2))
                        {
                            possibleSubs.Add(player2);
                        }
                    }
                    int min_idx = 0;
                    bool allTimerEqual = true;
                    for (int j = 0 + 1; j < possibleSubs.Count; j++)
                    {
                        if (possibleSubs[j].getTime().ElapsedMilliseconds != possibleSubs[min_idx].getTime().ElapsedMilliseconds)
                            allTimerEqual = false;
                        if (possibleSubs[j].getTime().ElapsedMilliseconds < possibleSubs[min_idx].getTime().ElapsedMilliseconds)
                            min_idx = j;
                    }
                    if (!allTimerEqual)
                    {
                        suggestsub(player, possibleSubs[min_idx]);
                        allSubs += "\n" + player + "will be subbed for " + possibleSubs[min_idx];
                    }


                    min_idx = 0;
                    bool allSkillEqual = true;
                    for (int j = 0 + 1; j < possibleSubs.Count; j++)
                    {
                        if (possibleSubs[j].getSkill() != possibleSubs[min_idx].getSkill())
                            allSkillEqual = false;
                        if (possibleSubs[j].getSkill() < possibleSubs[min_idx].getSkill())
                            min_idx = j;
                    }
                    if (!allSkillEqual)
                    {
                        suggestsub(player, possibleSubs[min_idx]);
                        allSubs += "\n" + player + "will be subbed for " + possibleSubs[min_idx];
                    }
                }
            }
            if (await DisplayAlert("Do AutoSubs?", allSubs, "OK", "Cancel"))
            {

            }
        }

        else if (gameTime.Elapsed.TotalSeconds == 20 * 60)
        {
            string allSubs = "Confirm Subs?";
            Halftime();
            foreach (var player in Forwards)
            {
                if (OnBench.Contains(player))
                {
                    List<GamePlayer> possibleSubs = new List<GamePlayer>();
                    foreach (var player2 in OnField)
                    {
                        if (Forwards.Contains(player2))
                        {
                            possibleSubs.Add(player2);
                        }
                    }
                    int min_idx = 0;
                    bool allTimerEqual = true;
                    for (int j = 0 + 1; j < possibleSubs.Count; j++)
                    {
                        if (possibleSubs[j].getTime().ElapsedMilliseconds != possibleSubs[min_idx].getTime().ElapsedMilliseconds)
                            allTimerEqual = false;
                        if (possibleSubs[j].getTime().ElapsedMilliseconds < possibleSubs[min_idx].getTime().ElapsedMilliseconds)
                            min_idx = j;
                    }
                    if (!allTimerEqual)
                    {
                        suggestsub(player, possibleSubs[min_idx]);
                        allSubs += "\n" + player + "will be subbed for " + possibleSubs[min_idx];
                    }
                    min_idx = 0;
                    bool allSkillEqual = true;
                    for (int j = 0 + 1; j < possibleSubs.Count; j++)
                    {
                        if (possibleSubs[j].getSkill() != possibleSubs[min_idx].getSkill())
                            allSkillEqual = false;
                        if (possibleSubs[j].getSkill() < possibleSubs[min_idx].getSkill())
                            min_idx = j;
                    }
                    if (!allSkillEqual)
                    {
                        suggestsub(player, possibleSubs[min_idx]);
                        allSubs += "\n" + player + "will be subbed for " + possibleSubs[min_idx];
                    }
                }
            }

            foreach (var player in Mids)
            {
                if (OnField.Contains(player))
                {
                    List<GamePlayer> possibleSubs = new List<GamePlayer>();
                    foreach (var player2 in OnField)
                    {
                        if (Mids.Contains(player2))
                        {
                            possibleSubs.Add(player2);
                        }
                    }
                    int min_idx = 0;
                    bool allTimerEqual = true;
                    for (int j = 0 + 1; j < possibleSubs.Count; j++)
                    {
                        if (possibleSubs[j].getTime().ElapsedMilliseconds != possibleSubs[min_idx].getTime().ElapsedMilliseconds)
                            allTimerEqual = false;
                        if (possibleSubs[j].getTime().ElapsedMilliseconds < possibleSubs[min_idx].getTime().ElapsedMilliseconds)
                            min_idx = j;
                    }
                    if (!allTimerEqual)
                    {
                        suggestsub(player, possibleSubs[min_idx]);
                        allSubs += "\n" + player + "will be subbed for " + possibleSubs[min_idx];
                    }


                    min_idx = 0;
                    bool allSkillEqual = true;
                    for (int j = 0 + 1; j < possibleSubs.Count; j++)
                    {
                        if (possibleSubs[j].getSkill() != possibleSubs[min_idx].getSkill())
                            allSkillEqual = false;
                        if (possibleSubs[j].getSkill() < possibleSubs[min_idx].getSkill())
                            min_idx = j;
                    }
                    if (!allSkillEqual)
                    {
                        suggestsub(player, possibleSubs[min_idx]);
                        allSubs += "\n" + player + "will be subbed for " + possibleSubs[min_idx];
                    }
                }
            }

            foreach (var player in Backs)
            {
                if (OnField.Contains(player))
                {
                    List<GamePlayer> possibleSubs = new List<GamePlayer>();
                    foreach (var player2 in OnField)
                    {
                        if (Backs.Contains(player2))
                        {
                            possibleSubs.Add(player2);
                        }
                    }
                    int min_idx = 0;
                    bool allTimerEqual = true;
                    for (int j = 0 + 1; j < possibleSubs.Count; j++)
                    {
                        if (possibleSubs[j].getTime().ElapsedMilliseconds != possibleSubs[min_idx].getTime().ElapsedMilliseconds)
                            allTimerEqual = false;
                        if (possibleSubs[j].getTime().ElapsedMilliseconds < possibleSubs[min_idx].getTime().ElapsedMilliseconds)
                            min_idx = j;
                    }
                    if (!allTimerEqual)
                        suggestsub(player, possibleSubs[min_idx]);


                    min_idx = 0;
                    bool allSkillEqual = true;
                    for (int j = 0 + 1; j < possibleSubs.Count; j++)
                    {
                        if (possibleSubs[j].getSkill() != possibleSubs[min_idx].getSkill())
                            allSkillEqual = false;
                        if (possibleSubs[j].getSkill() < possibleSubs[min_idx].getSkill())
                            min_idx = j;
                    }
                    if (!allSkillEqual)
                    {
                        suggestsub(player, possibleSubs[min_idx]);
                        allSubs += "\n" + player + "will be subbed for " + possibleSubs[min_idx];
                    }
                }
            }
            if (await DisplayAlert("Do AutoSubs?", allSubs, "OK", "Cancel"))
            {

            }
        }
    }
    private async Task Halftime()
    {
        gameTime.Stop();
        ticker.Stop();
        foreach (var player  in playerList)
        {
            player.getTime().Stop();
        }
        await DisplayAlert("Halftime", "The Game Has reached Halftime. Close this message box to continue the game", "Close");
        gameTime.Start();
        ticker.Start();
        foreach (var player in playerList)
        {
            player.getTime().Start();
        }
    }
    private void StartGame_Clicked(object sender, EventArgs e)
    {
        if (isGamePlaying) return;
        isGamePlaying = true;
        gameTime = Stopwatch.StartNew();
        ticker = Application.Current.Dispatcher.CreateTimer();
        ticker.Interval = TimeSpan.FromSeconds(1);
        ticker.Tick += (s, e) => Update();
        ticker.Start();
        foreach (var player in OnField)
        {
            player.getTime().Start();
        }
    }
}

class GamePlayer
{
	private Label name;
	private Label position;
	private Label number;
    private Button subButton;
	private Label timerLabel;
	private Stopwatch time;
	private bool isOnBench;
    private Grid PlayersGrid;
    private int gridRow;
    private Picker toSub;
    private int skill;
    public GamePlayer(Grid grid, Player player, int i)
	{
        gridRow = i;
        skill = player.GetSkill();
        PlayersGrid = grid;
        time = new Stopwatch();
        RowDefinition row = new RowDefinition(height: 75);
        PlayersGrid.RowDefinitions.Add(row);

        name = new Label()
        {
            TextColor = Colors.Green,
            FontSize = 20,
            Text = player.GetName()
        };
        PlayersGrid.Children.Add(name);
        PlayersGrid.SetRow(name, gridRow);
        PlayersGrid.SetColumn(name, 0);

        number = new Label()
        {
            TextColor = Colors.Green,
            FontSize = 20,
            Text = player.GetNumber().ToString()
        };
        PlayersGrid.Children.Add(number);
        PlayersGrid.SetRow(number, gridRow);
        PlayersGrid.SetColumn(number, 1);

        position = new Label()
        {
            TextColor = Colors.Green,
            FontSize = 20,
            Text = player.GetPosition()
        };
        PlayersGrid.Children.Add(position);
        PlayersGrid.SetRow(position, gridRow);
        PlayersGrid.SetColumn(position, 2);

        timerLabel = new Label()
        {
            TextColor = Colors.Green,
            FontSize = 20,
            Text = ""
        };
        PlayersGrid.Children.Add(timerLabel);
        PlayersGrid.SetRow(timerLabel, gridRow);
        PlayersGrid.SetColumn(timerLabel, 3);

        subButton = new Button()
        {
            BackgroundColor = new Color(241, 227, 228),
            Text = "Sub",
            TextColor = Colors.Black,
            Margin = new Thickness(5,0,5,5),
        };
        PlayersGrid.Children.Add(subButton);
        PlayersGrid.SetRow(subButton, gridRow);
        PlayersGrid.SetColumn(subButton, 4);

        toSub = new Picker()
        {
            Margin = new Thickness(10,0,5,0),
            BackgroundColor = new Color(241, 227, 228),
            TextColor = Colors.Black,
        };
        PlayersGrid.Children.Add(toSub);
        PlayersGrid.SetRow(toSub, gridRow);
        PlayersGrid.SetColumn(toSub, 5);
    }
    public Label getName() => name;
    public Label getPosition() => position;
    public Label getTimerLabel() => timerLabel;
    public void numberColor(Color color) => timerLabel.BackgroundColor = color;
    public Label getNumber() => number;
    public Stopwatch getTime() => time;
    public Picker getPicker() => toSub;
    public bool getBenchStatis() => isOnBench;
    public void setBenchStatis(bool statis) => isOnBench = statis; 
    public Button GetButton() => subButton;
    public int place() => gridRow;
    public int getSkill() => skill;

    public void manSub(int newPlace)
    {
        gridRow = newPlace;
        PlayersGrid.SetRow(name, gridRow);
        PlayersGrid.SetRow(number, gridRow);
        PlayersGrid.SetRow(position, gridRow);
        PlayersGrid.SetRow(timerLabel, gridRow);
        PlayersGrid.SetRow(subButton, gridRow);
    }
    public void sub(int newPlace)
    {
        gridRow = newPlace;
        if (isOnBench)
        {
            isOnBench = false;
            timerLabel.BackgroundColor = Colors.Green;
            time.Start();
        }
        else
        {
            isOnBench = true;
            timerLabel.TextColor = Colors.Red;
            time.Stop();
        }
        PlayersGrid.SetRow(name, gridRow);
        PlayersGrid.SetRow(number, gridRow);
        PlayersGrid.SetRow(position, gridRow);
        PlayersGrid.SetRow(timerLabel, gridRow);
        PlayersGrid.SetRow(subButton, gridRow);
        PlayersGrid.SetRow(toSub, gridRow);
    }
}