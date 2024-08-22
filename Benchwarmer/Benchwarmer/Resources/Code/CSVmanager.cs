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
        public List<string> Read(string csvPath)
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
                    lineContent.Add(encryption.Decrypt(reader.ReadLine(), "Benchwarmer"));
                }
                return lineContent;
            }
        }
        public async void Write(string path, string content)
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
            File.AppendAllText(finalPath, "\n" + encryption.Encrypt(content, "Benchwarmer"));
        }

        public void EditFile(string path, string name, string newContent, int place)
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

                while ((line = encryption.Decrypt(reader.ReadLine(), "Benchwarmer")) != null)
                {
                    string[] split = line.Split(',');

                    if (split[0].Contains(name))
                    {
                        split[place] = newContent;
                        line = string.Join(",", split);
                    }
                    lines.Add(encryption.Encrypt(line, "Benchwarmer"));
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
        private string Encrypt(string plaintext, int shift)
        {
            string cyphertext = "";
            foreach (char c in plaintext)
            {
                if (char.IsLetter(c))
                {
                    char lowerc = char.ToLower(c);
                    char shifted = (char)(((lowerc + shift - 'a') % 26) + 'a');
                    cyphertext += shifted;
                }
                else
                {
                    cyphertext += c;
                }
            }
            return cyphertext;
        }
        private string Decrypt(string cyphertext, int shift)
        {
            string plaintext = "";
            foreach (char c in cyphertext)
            {
                if (char.IsLetter(c))
                {
                    char unshifted = (char)(((c - shift - 'a') % 26) + 'a');
                    plaintext += unshifted;
                }
                else
                {
                    plaintext += c;
                }
            }
            return plaintext;
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