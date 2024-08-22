using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchwarmer.Resources.Code
{
    internal class MinuteManager
    {
        public MinuteManager() { }

        public void calculations(Team team, int halftime)
        {
            Player[] players = team.GetPlayers().ToArray();
            Player[] OnField;
            Player[] bench;
        }
        public void Sub(Player player, Team team) 
        {
            List<Player> SubTargets = new List<Player>();
            foreach (Player subtarget in team.GetPlayers())
                if(subtarget.GetSubgroup() == player.GetSubgroup()) SubTargets.Add(subtarget);

            
        }
    }
}
