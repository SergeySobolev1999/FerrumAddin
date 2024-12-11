using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace RevitAPI_Basic_Course.Creating_Specifications
{
    public class Folder
    {
        public Folder()
        {
            Data_Creating_Specifications.Folder_Base_Way = Folder_Base_Way();
            Data_Creating_Specifications.Folder_Presets = Folder_Presets();
        }
        public string Folder_Base_Way()
        {
            string basic_Path = @"C:\Users\User_Name\AppData\Roaming\Autodesk\Revit\Addins\2024\Splugin";
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string[] userName_Split = userName.Split(new[] { "\\" }, StringSplitOptions.None);
            string str = userName_Split[userName_Split.Count() - 1];
            return basic_Path.Replace("User_Name", str);
        }
        public string Folder_Presets()
        {
            string path = Data_Creating_Specifications.Folder_Base_Way + @"\\Presets";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}
