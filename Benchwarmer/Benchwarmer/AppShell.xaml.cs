using Benchwarmer.Resources.Code;
using Benchwarmer.Resources.Pages;

namespace Benchwarmer
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            CSVmanager csvmanager = new CSVmanager();
            string isLoggedinValue = csvmanager.readCsv("\\Resources\\Memory\\Login.csv")[0].Split(',')[1];
            if (isLoggedinValue != "1")
            {
                App.Current.MainPage = new NavigationPage(new LoginPage());
            }
            csvmanager.editFile(@"\Resources\Memory\Login.csv", "UserIsLoggedIn", "1", 1);
        }
    }
    public class Login(string username, string password)
    {

    }
}
