using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchwarmer.Resources.SavedTeams
{
    internal class CSVmanager
    {
        public CSVmanager() { }
        public void writeToCsv(string csvPath, string content)
        {
            using (var reader = new StreamReader(csvPath))
            {
                while (reader.EndOfStream == false)
                { 
                    string lineContent = reader.ReadLine();
                }
            }
        }
    }
}
