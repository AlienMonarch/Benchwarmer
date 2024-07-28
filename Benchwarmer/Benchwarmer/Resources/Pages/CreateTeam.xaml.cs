using Benchwarmer.Resources.Code;

namespace Benchwarmer.Resources.Pages;

public partial class CreateTeam : ContentPage
{
	public CreateTeam()
	{
		InitializeComponent();
        
    }

    private void CreateTeamButton_Clicked(object sender, EventArgs e)
    {
        Team team = new Team(TeamNameField.Text);
        CSVmanager cSVmanager = new CSVmanager();
        string[] content = new string[] {"hello","didthiswork?","Whatsup" };
        cSVmanager.writeCsv(@"\Resources\SavedTeams\", content);
        App.Current.MainPage = new NavigationPage(new TeamView());
    }
}