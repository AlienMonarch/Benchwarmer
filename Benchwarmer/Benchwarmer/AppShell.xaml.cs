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
            if (csvmanager.readCsv("\\Resources\\Memory\\Memory.csv")[0].Split(',')[1] == "0")
            {
                App.Current.MainPage = new NavigationPage(new LoginPage());
            }
            csvmanager.editFile(@"\Resorces\Memory\Memory.txt", "UserIsLoggedIn", "1", 0);
        }
    }
    public class Login(string username, string password)
    {

    }
}
