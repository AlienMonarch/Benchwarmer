using Benchwarmer.Resources.Code;
using Microsoft.Maui.Animations;
using System.Diagnostics;

namespace Benchwarmer.Resources.Pages;

public partial class PlayGame : ContentPage
{
	bool isGamePlaying = true;
	Stopwatch gameTime;

	public PlayGame()
	{
		gameTime = Stopwatch.StartNew();

		var ticker = Application.Current.Dispatcher.CreateTimer();
		ticker.Interval = TimeSpan.FromSeconds(1);
		ticker.Tick += (s, e) => Update();
		ticker.Start();
		InitializeComponent();
		CSVmanager cSVmanager = new CSVmanager();
		string currentTeam = cSVmanager.Read("\\Memory\\Memory.csv").ToString().Split(',')[0];
        List<string> list = cSVmanager.Read("\\SavedTeams\\testTeam.csv");
		Team team = new Team("TeamName");
		for (int i = 0; i < list.Count; i++)
		{
			string[] arr = list[i].Split(',');
			Player player = new Player(arr[0], arr[1], Convert.ToInt32(arr[2]), Convert.ToInt32(arr[3]));
			team.addPlayer(player);

			Label label = new Label()
			{
				TextColor = Colors.Green,
				FontSize = 15,
				Text = team.GetPlayers()[i].GetPosition() + " " + team.GetPlayers()[i].GetName(),
				Margin = new Thickness(0, i * 30 + 25, 0, 0)
			};
			GridLayout.Children.Add(label);
		}
	}
	void Update()
    {
		if (isGamePlaying)
			GameTimer.Text = gameTime.Elapsed.ToString().Split('.')[0];
		
    }
}