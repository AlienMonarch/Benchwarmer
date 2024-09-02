using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
/* File name checker
 * Checks if a file name is permissable to be used.
 * 
 */
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
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                if (fileName.Contains(c))
                {
                    return false;
                }
            }
            string[] reservedFileNames =
            {
                "USERS", "MEMORY", "CREATETEAM", "EDITTEAM", "HOME", "LOGINPAGE", "PLAYGAME",
                "PROFILE", "SETTINGS", "SIGNUP", "STATS", "TEAMPAGE", "APP", "APPSHELL", "MAINPAGE",
                "MAUIPROGRAM", "NoName", "Admin"
            };
            string fileNameUpper = fileName.ToUpper();
            foreach (string reserved in reservedFileNames)
            {
                if (fileNameUpper == reserved)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
