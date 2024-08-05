using Benchwarmer.Resources.Code;
using Benchwarmer.Resources.Pages;

namespace Benchwarmer
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            CSVmanager csvmanager = new CSVmanager();
            string isLoggedinValue = csvmanager.readCsv("\\Resources\\Memory\\Memory.csv")[0].Split(',')[1];

            if (isLoggedinValue != "1")
            {
                //LoginPage
                App.Current.MainPage = new NavigationPage(new LoginPage());
                //csvmanager.editFile(@"\Resources\Memory\Login.csv", "UserIsLoggedIn", "Oh bad words everything broke :(", 1);
            }
            csvmanager.editFile(@"\Resources\Memory\Memory.csv", "UserIsLoggedIn", "0", 1);
        }
    }

}
