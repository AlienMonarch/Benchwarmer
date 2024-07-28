using Benchwarmer.Resources.Code;
using Benchwarmer.Resources.Pages;

namespace Benchwarmer
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            Main main = new Main();
        }
        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            LoginButton.BackgroundColor = Colors.Blue;
            if (UsernameField.Text.Contains("/"))
            {
                UsernameField.Text = "Fuck you";
            }
            else if (PasswordField.Text.Contains("/"))
            {
                PasswordField.Text = "fuck you";
            }
            else 
            {
                App.Current.MainPage = new NavigationPage(new Home());   
            }
        }
    }

}
