using Microsoft.Maui.Controls.Xaml.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchwarmer.Resources.Code
{
    class Player
    {
        string name;
        string position;
        int number;
        int skill;
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
    }
}
