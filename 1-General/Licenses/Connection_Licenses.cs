using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;
using SSDK;

namespace WPFApplication.Licenses
{
    public class Licenses_True_
    {
        
        Serialize deserialize = new Serialize();
        public  Licenses_True_()
        {
            Folder folder = new Folder();
            Save save = new Save();
            save.presets_Save();
            // Строка подключения
            string connectionString = "Server=192.168.1.5;Port=8899;Database=bim_users;User ID=sergey_sobolev;Password=ercy352y32a;Connection Timeout=2;";
            // Создание соединения
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    // Открытие соединения
                    connection.Open();
                    // Выполнение SQL-запроса
                    string query = "SELECT * FROM users";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    User_license user_License = deserialize.DeserializeXML();
                    // Чтение данных
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["name"].ToString() == user_License.name && reader["surname"].ToString() == user_License.surname && reader["password"].ToString() == user_License.password)
                            {
                                Get_User_Name get_User_Name_Value = new Get_User_Name();
                                if (get_User_Name_Value.Get_User_Name_Value(user_License.id) == Data.username)
                                {
                                    Set_User_Name set_User_Name = new Set_User_Name(user_License.id);
                                    SSDK_Data.licenses_Connection = true;
                                }
                                else
                                {
                                    SSDK_Data.licenses_Connection = false;
                                }
                            }
                            else
                            {
                                SSDK_Data.licenses_Connection = false;
                            }
                        }
                        
                    }
                }
                catch (MySqlException ex)
                {
                    SSDK_Data.licenses_Connection = false;
                    S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                    s_Mistake_String.ShowDialog();
                }
            }
        }
    }
    public class Set_User_Name
    {
        public Set_User_Name(int i)
        {
            // Строка подключения
            string connectionString = "Server=192.168.1.5;Port=8899;Database=bim_users;User ID=sergey_sobolev;Password=ercy352y32a;Connection Timeout=2;";
            string use_name = Data.username;
            // Создание соединения
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    // Открытие подключения
                    connection.Open();

                    // SQL-запрос для обновления user_name
                    string query = "UPDATE zhelezno_plugin_licenses SET user_name = @user_name WHERE id_persons = @id_persons";

                    // Создание команды
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Добавление параметров
                        command.Parameters.AddWithValue("@user_name", use_name); // Передаем новое имя пользователя
                        command.Parameters.AddWithValue("@id_persons", i); // Указываем идентификатор строки

                        // Выполнение команды
                        int rowsAffected = command.ExecuteNonQuery();

                        // Вывод результата
                        Console.WriteLine($"Обновлено строк: {rowsAffected}");
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }
    }
    public class Get_User_Name
    {
        public string Get_User_Name_Value(int i)
        {
            // Строка подключения
            string connectionString = "Server=192.168.1.5;Port=8899;Database=bim_users;User ID=sergey_sobolev;Password=ercy352y32a;Connection Timeout=2;";
            string use_name = Data.username;
            // Создание соединения
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    // Открытие соединения
                    connection.Open();
                    // Выполнение SQL-запроса
                    string query = "SELECT * FROM zhelezno_plugin_licenses";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    // Чтение данных
                    bool iterarion = false;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (Int32.Parse(reader["id_persons"].ToString()) == i)
                            {
                                return reader["user_name"].ToString();
                                iterarion = true;
                            }
                        }
                    }
                    return "";
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                    return "";
                }
            }
        }
    }
    public class Serialize
    {
        XmlSerializer xml_Set = new XmlSerializer(typeof(User_license));
        User_license user_License = new User_license();
        public void SerializeXML2(User_license user_License)
        {
            using (FileStream fs2 = new FileStream(Data.Folder_Presets + @"\Serialize_Set_Creating_Specifications.xml", FileMode.Create))
            {
                xml_Set.Serialize(fs2, user_License);
            }
        }
        public User_license DeserializeXML()
        {
            using (FileStream fs = new FileStream((Data.Folder_Presets + @"\Serialize_Set_Creating_Specifications.xml"), FileMode.OpenOrCreate))
            {
                return (User_license)xml_Set.Deserialize(fs);
            }
        }
    }
    public class Save
    {
        public void presets_Save()
        {
            string path = System.IO.Path.Combine(Data.Folder_Base_Way, "Presets");

            string path_File_Name = "Serialize_Set_Creating_Specifications.xml";
            string filePath = System.IO.Path.Combine(path, path_File_Name);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath)) ;
                User_license user_License = new User_license(0, "0", "0", "0");
                Serialize serialize = new Serialize();
                serialize.SerializeXML2(user_License);
            }

        }
    }
    public class Folder
    {
        public Folder()
        {
            Data.Folder_Base_Way = Folder_Base_Way();
            Data.Folder_Presets = Folder_Presets();
        }
        public string Folder_Base_Way()
        {

            string basic_Path = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "Autodesk",
            "Revit",
            "Addins",
            "2024"
        );
            //string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            //string[] userName_Split = userName.Split(new[] { "\\" }, StringSplitOptions.None);
            //string str = userName_Split[userName_Split.Count() - 1];
            //return basic_Path.Replace("User_Name", str);
            return basic_Path;
        }
        public string Folder_Presets()
        {
            string path = Data.Folder_Base_Way + @"\\Presets";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
    [Serializable]
    public class User_license
    {
        public User_license() { }
        public int id { get; set; } = 0;
        public string name { get; set; } = "";
        public string surname { get; set; } = "";
        public string password { get; set; } = "";
        public User_license(int id, string name, string surname, string password)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.password = password;
        }
    }

}


