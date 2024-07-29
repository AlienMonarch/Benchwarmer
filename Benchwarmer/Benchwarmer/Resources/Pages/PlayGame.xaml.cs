using Benchwarmer.Resources.Code;

namespace Benchwarmer.Resources.Pages;

public partial class PlayGame : ContentPage
{
	public PlayGame()
	{
		InitializeComponent();
		CSVmanager cSVmanager = new CSVmanager();
		List<string> list = cSVmanager.readCsv(@"\Resources\SavedTeams\testTeam.csv");
		Team team = new Team("TeamName");
		for (int i = 0; i < list.Count-1; i++)
		{
			string[] arr = list[i].Split(',');
			Player player = new Player(arr[0], arr[1], Convert.ToInt32(arr[2]), Convert.ToInt32(arr[3]));
			team.addPlayer(player);
			Label label = new Label();
			label.Text = team.GetPlayers()[i].GetName();
			label.BackgroundColor = Colors.Blue;
		}
	}
}