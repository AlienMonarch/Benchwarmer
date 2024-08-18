using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
        public async void writeCsv(string path, string[] content)
        {
            FileNameChecker checker = new FileNameChecker();
            if (!checker.Check(path))
            {
                return;
            }
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

        public void EncryptedEdit(string path, string name, string newContent, int place)
        {
            string finalpath = projectDirectory + path;
            if (!File.Exists(finalpath))
            {
                CreateFile(finalpath.Split('\\')[0], finalpath.Split('\\')[1]);
            }
            List<string> lines = EncryptedRead(path);
            foreach (String line in lines)
            {
                string[] split = line.Split(',');
                if (split[0].Contains(name))
                {
                    split[place] = newContent;
                    lines.Add(string.Join(",", split));
                }
            }
            try
            {
                using (FileStream fileStream = new("TestData.txt", FileMode.OpenOrCreate))
                {
                    using (Aes aes = Aes.Create())
                    {
                        byte[] key =
                        {
                            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                            0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16,
                            0x20, 0x40, 0x80, 0x30, 0x60, 0x90, 0x17, 0x18
                        };
                        aes.Key = key;

                        byte[] iv = aes.IV;
                        fileStream.Write(iv, 0, iv.Length);
                        fileStream.Read(iv, 0, iv.Length);
                        using (CryptoStream cryptoStream = new(
                        fileStream,
                        aes.CreateEncryptor(),
                        CryptoStreamMode.Write))
                        {
                            using (StreamWriter encryptWriter = new(cryptoStream))
                            {
                                foreach (String line in lines)
                                {
                                    encryptWriter.WriteLine(line);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("The file was encrypted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The encryption failed. {ex}");
            }
        }

        public void EncryptedWrite(string path, string data)
        {
            string finalpath = projectDirectory + path;
            List<string> content = EncryptedRead(path);
            if (!File.Exists(finalpath))
            {
                CreateFile(path.Split('\\')[0], path.Split('\\')[1]);
            }
            using (FileStream fileStream = new(finalpath, FileMode.OpenOrCreate))
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] key =
                    {
                        0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                        0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16,
                        0x20, 0x40, 0x80, 0x30, 0x60, 0x90, 0x17, 0x18
                    };
                    aes.Key = key;

                    byte[] iv = aes.IV;
                    fileStream.Write(iv, 0, iv.Length);

                    using (CryptoStream cryptoStream = new(
                        fileStream,
                        aes.CreateEncryptor(),
                        CryptoStreamMode.Write))
                    {
                        using (StreamWriter encryptWriter = new(cryptoStream))
                        {
                            if (content.Count >= 0)
                            {
                                encryptWriter.WriteLine(data);
                            }
                            else
                            {
                                foreach (string line in content)
                                {
                                    encryptWriter.WriteLine(line);
                                }
                                encryptWriter.WriteLine(data);
                            }
                        }
                    }
                    Console.WriteLine("The file was encrypted.");
                }
            }
        }

            public List<string> EncryptedRead(string path)
            //outputs a list of strings of full lines of csv files. encryptedRead(path)[#]string.Split(',') later to get individual values
            //https://learn.microsoft.com/en-us/dotnet/standard/security/encrypting-data
            {
                string finalpath = projectDirectory + path;
                using (FileStream fileStream = new(finalpath, FileMode.Open, FileAccess.Read))
                {
                    using (Aes aes = Aes.Create())
                    {
                        byte[] iv = new byte[aes.IV.Length];
                        int numBytesToRead = aes.IV.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            int n = fileStream.Read(iv, numBytesRead, numBytesToRead);
                            if (n == 0) break;
                            numBytesRead += n;
                            numBytesToRead -= n;
                        }

                        byte[] key =
                        {
                            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                            0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16,
                            0x20, 0x40, 0x80, 0x30, 0x60, 0x90, 0x17, 0x18
                            };

                        using (CryptoStream cryptoStream = new(
                           fileStream,
                           aes.CreateDecryptor(key, iv),
                           CryptoStreamMode.Read))
                        {
                            using (StreamReader decryptReader = new(cryptoStream))
                            {
                                List<string> lineContent = new List<string>();
                                while (decryptReader.EndOfStream == false)
                                {
                                    lineContent.Add(decryptReader.ReadLine());
                                }
                                return lineContent;
                            }
                        }
                    }
                }
                throw new Exception("Something Broke :(");
            }

            public void CreateFile(string folder, string filename)
            {
                string finalpath = projectDirectory + "\\" + folder + "\\" + filename;
                FileNameChecker checker = new FileNameChecker();
                if (!File.Exists(finalpath) && checker.Check(finalpath))
                {
                    File.Create(finalpath).Close();
                }
                else
                {
                    Console.WriteLine("File name already exists or contains inelligible characters or strings");
                }
            }
        
    }
}
