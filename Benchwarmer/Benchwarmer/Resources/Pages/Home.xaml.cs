using Benchwarmer.Resources.Code;

namespace Benchwarmer.Resources.Pages;

public partial class Home : ContentPage
{
    int count = 0;
	public Home()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new MainPage());
    }

    private void Csvtest_Clicked(object sender, EventArgs e)
    {
        CSVmanager csv = new CSVmanager();
        Csvtest.Text = csv.readCsv(@"\Resources\SavedTeams\Sample.csv")[0];


    }
}