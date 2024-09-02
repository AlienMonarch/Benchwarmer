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
            try
            {
                //csvmanager.EncryptWrite("\\Memory\\Memory.csv", "UserIsLoggedIn,0\nCurrentUser,Admin\nCurrentTeam,NoTeam");
                //csvmanager.Replace("\\Memory\\Memory.csv", "UserIsLoggedIn", "0", 1);
                string isLoggedinValue = csvmanager.EncryptedRead("\\Memory\\Memory.csv")[0].Split(',')[1];
                if (isLoggedinValue != "1")
                {
                    App.Current.MainPage = new NavigationPage(new LoginPage());
                    //csvmanager.Replace(@"\Memory\Login.csv", "UserIsLoggedIn", "1", 1);
                }
                else
                {
                    csvmanager.Replace("\\Memory\\Memory.csv", "UserIsLoggedIn", "0", 1);
                }
            }
            catch (Exception)
            {
                csvmanager.Replace("\\Memory\\Memory.csv", "UserIsLoggedIn", "0", 1);
                csvmanager.Replace("\\Memory\\Memory.csv", "CurrentUser", "Admin", 1);
                csvmanager.Replace("\\Memory\\Memory.csv", "CurrentTeam", "NoTeam", 1);
            }            
        }
    }

}
