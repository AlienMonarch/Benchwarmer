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
                projectDirectory = AppDomain.CurrentDomain.BaseDirectory.Split("\\bin")[0];
                writeCsv("\\Resources\\Memory\\Memory.txt", new string[]{projectDirectory});
            }
        }
        public List<string> readCsv(string csvPath)
        {
            string finalpath = projectDirectory + csvPath;
            if (!File.Exists(finalpath))
            {
                throw new FileNotFoundException(finalpath);
            }
            using (var reader = new StreamReader(finalpath))
            {
                List<string> lineContent = new List<string>();
                while (reader.EndOfStream == false)
                {
                    lineContent.Add(reader.ReadLine());
                }
                return lineContent;
            }
        }
        public void writeCsv(string path, string[] content)
        {
            string finalPath = projectDirectory + path;
            if (!File.Exists(finalPath))
            {
                throw new FileNotFoundException(finalPath);
            }
            for (int i = 0; i < content.Length; i++)
            {
                File.AppendAllText(finalPath, "\n" + content[i]);
            }
        }

        public void editFile(string path, string name, string newContent, int place)
        {
            List<string> lines = new List<string>();
            string finalpath = projectDirectory + path;

            if (!File.Exists(finalpath))
            {
                throw new FileNotFoundException(finalpath);
            }
            using (StreamReader reader = new StreamReader(finalpath))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    string[] split = line.Split(',');

                    if (split[0].Contains(name))
                    {
                        split[place] = newContent;
                        line = string.Join(",", split);
                    }
                    lines.Add(line);
                }
            }

            using (StreamWriter writer = new StreamWriter(finalpath, false))
            {
                foreach (String line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }

       /* public void editMemory(string path, string varName, string content) 
        {
            string finalPath = projectDirectory + path;
            if (!File.Exists(finalPath))
            {
                throw new FileNotFoundException(finalPath);
            }
            List<string> lineContent = readCsv(finalPath);
            int temp;
            for (int i = 0; i < lineContent.Count; i++) 
            {
                if (lineContent[i].Split(",")[0] == varName)
                {
                    temp = i; break;
                }
                else if (i == lineContent.Count - 1) 
                {
                    Console.WriteLine("Variable not found");
                }
            }
            using (var editor = new StreamWriter(finalPath)) 
            {
                while (true)
                {

                }
            }
        }*/

        public void encrypt()
        {

        }
        public void decrypt()
        {

        }
    }
}
