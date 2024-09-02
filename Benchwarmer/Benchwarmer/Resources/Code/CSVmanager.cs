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
        Encryption encryption = new Encryption();
        private string projectDirectory;
        public CSVmanager()
        {
            if (projectDirectory == null)
            {
                projectDirectory = AppDomain.CurrentDomain.BaseDirectory.Split("\\bin")[0] + "\\Resources";
            }
        }
        public void Replace(string path, string nameValue, string replace, int place)
        {
            string finalpath = projectDirectory + path;
            List<string> tempdata = EncryptedRead(path);
            List<string> data = new List<string>();
            foreach (var item in tempdata)
            {
                string[] split = item.Split(',');
                List<string> temp = new List<string>();
                if (split[0] == nameValue)
                {
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (i == place)
                        {
                            temp.Add(replace);
                        }
                        else
                        {
                            temp.Add(split[i]);
                        }
                    }
                    data.Add(string.Join(",", temp));
                }
                else
                {
                    data.Add(item);
                }
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
                            foreach (string line in data)
                            {
                                encryptWriter.WriteLine(line);
                            }
                        }
                    }
                    Console.WriteLine("The file was encrypted.");
                }
            }
        }

        public void EncryptWrite(string path, string content)
        {
            string finalpath = projectDirectory + path;
            List<string> data = EncryptedRead(path);
            data.Add(content);


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
                            foreach (string line in data)
                            {
                                encryptWriter.WriteLine(line);
                            }
                        }
                    }
                    Console.WriteLine("The file was encrypted.");
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
            if(path.Contains("\\Resorces\\Resorces"))
            {
                throw new Exception("Somehow there are two \\resorces here?");
            }
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
            File.Create(projectDirectory + "\\" + folder + "\\" + filename).Dispose();
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(projectDirectory + path);
        }
        public void DeleteFile(string path)
        {
            File.Delete(projectDirectory + path);
        }

        public void ChangeName(string path, string newPath)
        {
            File.Move(projectDirectory + path, projectDirectory + newPath);
        }
    }
}