namespace Benchwarmer.Resources.Pages;

public partial class Home : ContentPage
{
	public Home()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new MainPage());
    }
}