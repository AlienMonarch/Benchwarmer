using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//This file is very much still a work in progress. It is not even called anywhere yet
namespace Benchwarmer.Resources.Code
{
    internal class UserClass
    {
        private string name;
        private string username;
        private List<Team> teams;
        public UserClass(string name)
        {
            CSVmanager csvmanager = new CSVmanager();
            List<string> savedUsers = csvmanager.readCsv("\\Users\\Users.csv");
            bool userExists = false;
            foreach (string user in savedUsers)
            {
                if (user.Split(',')[0] == username)
                {
                    userExists = true;
                    //User already exists, just creating a user class for temporary use
                } 
            }
            if (!userExists)
            {
                //User does not exist, create a new user
                string[] content = new string[3];
                content[0] = "Name," + name;
                content[1] = "Username," + username;
                content[2] = "SavedTeams";
                csvmanager.writeCsv("\\Users\\" + name, content);
            }         
        }
        public void AddTeam(string teamName, Team team)
        {
            CSVmanager csvmanager = new CSVmanager();
            csvmanager.editFile("\\Users\\"+name, "SavedTeams", "," + teamName, csvmanager.readCsv(name).Count);
            teams.Add(team);
        }
    }
}
