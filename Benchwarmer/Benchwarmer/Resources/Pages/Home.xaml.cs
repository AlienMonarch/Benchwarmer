using Benchwarmer.Resources.Code;

namespace Benchwarmer.Resources.Pages;

public partial class Home : ContentPage
{
    int count = 0;
	public Home()
	{
		InitializeComponent();
    }
    private void Csvtest_Clicked(object sender, EventArgs e)
    {
        CSVmanager csv = new CSVmanager();
        CsvText.Text = csv.readCsv("\\SavedTeams\\Sample.csv")[0];
    }
}