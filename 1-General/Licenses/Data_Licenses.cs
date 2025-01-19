using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace WPFApplication.Licenses
{
    public class Data
    {
        public static string Folder_Base_Way { get; set; } = "";
        public static string Folder_Presets { get; set; } = "";
        public static int id_persons { get; set; } = 0;
        public static string surname { get; set; } = "";
        public static string name { get; set; } = "";
        public static string patronymic { get; set; } = "";
        public static string status { get; set; } = "";
        public static string post { get; set; } = "";
        public static string password { get; set; } = "";
        
        public static User_license user_License { get; set; } = null;
        public static bool licenses_Value = false;
    }
}
