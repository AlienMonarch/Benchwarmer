namespace Benchwarmer.Resources.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private void LoginButton_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new Home());
    }
}