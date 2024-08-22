using Microsoft.Maui.Controls.Xaml.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchwarmer.Resources.Code
{
    class Player
    {
        private string name;
        private string position;
        private int number;
        private int skill;
        private int subgroup;
        public Player(string tempname, string tempposition, int tempnumber, int tempskill) 
        {
            name = tempname;
            position = tempposition;
            number = tempnumber;
            if (tempskill >= 0 && tempskill <= 5)
            {
                skill = tempskill;
            }
            else
            {
                skill = 0;
            }
        }
        public string GetName() => name;
        public string GetPosition() => position;
        public int GetNumber() => number;
        public int GetSkill() => skill;
        public int GetSubgroup() => subgroup;
            
    }
}
