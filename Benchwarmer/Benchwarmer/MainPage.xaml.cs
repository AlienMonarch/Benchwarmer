using Benchwarmer.Resources.Pages;
//Why are you here? This file is never used. Weirdo.

//Leave please.
namespace Benchwarmer
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }
        private void LoginButton_Clicked(object sender, EventArgs e)
        {
                App.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }

}
