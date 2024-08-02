using Benchwarmer.Resources.Code;

namespace Benchwarmer.Resources.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private void LoginButton_Clicked(object sender, EventArgs e)
    {
        CSVmanager csvmanager = new CSVmanager();
        List<string> users = csvmanager.readCsv(@"\Resources\Users\Users.csv");
        users.Sort();
        users.BinarySearch(UsernameField.Text + "," + PasswordField.Text);
        
    }
}