using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//Little bit of GPT...
namespace Benchwarmer.Resources.Code
{
    internal class FileNameChecker
    {
        public bool Check(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
            {
                if (fileName.Contains(c))
                {
                    return false;
                }
            }
            string[] reservedFileNames =
            {
            "CON", "PRN", "AUX", "NUL", "NULL",
            "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
            "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9",
            "USERS", "MEMORY", "CREATETEAM", "EDITTEAM", "HOME", "LOGINPAGE", "PLAYGAME",
            "PROFILE", "SETTINGS", "SIGNUP", "STATS", "TEAMPAGE", "APP", "APPSHELL", "MAINPAGE",
            "MAUIPROGRAM"
            };
            string upperFileName = fileName.ToUpper();
            foreach (string reserved in reservedFileNames)
            {
                if (upperFileName == reserved)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
