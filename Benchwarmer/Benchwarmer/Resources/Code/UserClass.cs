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
        private string username;
        private List<Team> teams;
        public UserClass(string tempname)
        {
            CSVmanager csvmanager = new CSVmanager();
            List<string> savedUsers = csvmanager.EncryptedRead("\\Memory\\Users.csv");
            bool userExists = false;
            foreach (string user in savedUsers)
            {
                if (user.Split(',')[0] == username)
                {
                    userExists = true;
                    break;
                    //User already exists, just creating a user class for temporary use
                } 
            }
            if (!userExists)
            {
                //User does not exist, create a new user
                csvmanager.EncryptWrite("\\Users\\" + tempname + "\\" + tempname + ".csv", "Username," + username);
                csvmanager.Replace("\\Memory\\Memory.csv", "CurrentUser", username, 1);
                csvmanager.Replace("\\Memory\\Memory.csv","CurrentTeam","NoTeam", 1);
                teams = new List<Team>();
            }   
            else
            {
                List<string> content = csvmanager.EncryptedRead("\\Users\\" + tempname);
                username = content[0].Split(',')[1];
                for (int i = 2; i < content.Count; i++)
                {
                    
                }
            }
        }
        public void AddTeam(string teamName, Team team)
        {
            teams.Add(team);
            CSVmanager csvmanager = new CSVmanager();
            List<string> content = new List<string>();
            bool found = false;
            for (int i = 2; i < content.Count; i++)
            {
                if (content[i].Split(',')[1] == teamName)
                {
                    found = true;
                }
            }
            if (!found)
            {
                csvmanager.Replace("\\Users\\" + username, "SavedTeams", "," + teamName, csvmanager.EncryptedRead(username).Count);
            }
        }
        public void RemoveTeam(string teamName)
        {

        }
        public List<Team> GetTeams() => teams;
        public string GetName() => username;
    }
}
