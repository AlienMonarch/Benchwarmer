using Benchwarmer.Resources.Code;
using Microsoft.Maui;
using static System.Net.Mime.MediaTypeNames;

namespace Benchwarmer.Resources.Pages;

public partial class Stats : ContentPage
{
	public Stats()
	{
		InitializeComponent();
		CSVmanager csv = new CSVmanager();
		string[] team = csv.EncryptedRead("\\" // \\
			+ csv.EncryptedRead("\\Memory\\Memory")[1].Split(',')[1] // \\TestUser
			+ "\\" // \\TestUser\\
			+ csv.EncryptedRead("\\Memory\\Memory")[2].Split(',')[1] // \\TestUser\\TestTeam
			+ ".csv").ToArray(); // \\TestUser\\TestTeam.csv

		for (int i = 1; i < team.Length; i++)
		{
            Label label = new Label()
            {
                Text = team[i].Split(',')[0] 
                + team[i].Split(',')[1]
                + team[i].Split(',')[2]
                + team[i].Split(',')[3],
                TextColor = Colors.Black,
                FontSize = 15,
				
            };
			StatsArea.Children.Add(label);
        }
		
	}
}