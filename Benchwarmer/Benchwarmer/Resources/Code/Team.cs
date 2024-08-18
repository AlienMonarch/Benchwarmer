﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchwarmer.Resources.Code
{
    class Team
    {
        private List<Player> players = new List<Player>();
        private Formation Formation;
        private string name;
        public Team(string tempname)
        {
            name = tempname;
        }
        public List<Player> GetPlayers() => players;

        public void addPlayer(Player player) => players.Add(player);
        public void removePlayer(Player player) => players.Remove(player);
        public void changename(string changename) => name = changename; 
        public string Getname() => name;
    }
}
