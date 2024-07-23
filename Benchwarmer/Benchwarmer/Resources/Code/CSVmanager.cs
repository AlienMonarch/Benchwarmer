using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//If the CSV files break this is the culprit

namespace Benchwarmer.Resources.Code
{
    internal class CSVmanager
    {
        private string projectDirectory;
        public CSVmanager()
        {
            if (projectDirectory == null)
            {
                projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }
        }
        public List<string> readCsv(string csvPath)
        {
            string finalpath = projectDirectory.Split("\\bin")[0] + csvPath;
            using (var reader = new StreamReader(finalpath))
            {
                List<string> lineContent = new List<string>();
                while (reader.EndOfStream == false)
                {
                    lineContent.Add(reader.ReadLine());
                }
                return lineContent;
            }
            return null;
        }
        public void writeCsv(string csvPath, string[] content)
        {
            for (int i = 0; i < content.Length; i++)
            {
                File.AppendAllText(projectDirectory.Split("\\bin")[0] + csvPath, content[i]);
            }
        }
    }
}
