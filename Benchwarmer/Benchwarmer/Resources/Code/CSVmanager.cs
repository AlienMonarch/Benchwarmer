﻿using System;
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
                projectDirectory = AppDomain.CurrentDomain.BaseDirectory.Split("\\bin")[0] + "\\Resources";
            }
        }
        public List<string> readCsv(string csvPath)
            //outputs a list of strings of full lines of csv files. string.Split(',') later to get individual values
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
    }
}
