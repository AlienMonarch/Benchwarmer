using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchwarmer.Resources.Code
{
    internal class Formation
    {
        public Dictionary<string, string> formationDictionary;
        public Formation(int formation)
        {
            switch (formation)
            {
                case 1:
                    formationDictionary = new Dictionary<string, string>();
                    formationDictionary.Add("LW", "Forwards");
                    formationDictionary.Add("CF", "Forwards");
                    formationDictionary.Add("RW", "Forwards");
                    formationDictionary.Add("LI", "Inners");
                    formationDictionary.Add("RI", "Inners");
                    formationDictionary.Add("CHB", "Centre Half Back");
                    formationDictionary.Add("LHB", "Back");
                    formationDictionary.Add("RHB", "Back");
                    formationDictionary.Add("CB", "Back");
                    formationDictionary.Add("G", "Golie");
                    break;
                case 2:
                    formationDictionary = new Dictionary<string, string>();
                    formationDictionary.Add("LW", "Forwards");
                    formationDictionary.Add("CF", "Forwards");
                    formationDictionary.Add("RW", "Forwards");
                    formationDictionary.Add("LM", "Mids");
                    formationDictionary.Add("CM", "Mids");
                    formationDictionary.Add("RM", "Mids");
                    formationDictionary.Add("LB", "Back");
                    formationDictionary.Add("RB", "Back");
                    formationDictionary.Add("CB", "Back");
                    formationDictionary.Add("SW", "Back");
                    formationDictionary.Add("G", "Golie");
                    break;
                default:
                    break;
            }
        }
    }
}
