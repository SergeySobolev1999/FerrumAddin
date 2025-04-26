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
using System.Security.RightsManagement;
using Org.BouncyCastle.Asn1.Ocsp;
using DocumentFormat.OpenXml.Office2016.Excel;

namespace WPFApplication.Licenses
{
    public class Licenses_True_
    {
        
        Serialize deserialize = new Serialize();
        public  Licenses_True_(Dictionary<string,string> request)
        {
            Folder folder = new Folder();
            Save save = new Save();
            save.presets_Save();
            // Строка подключения
            string connectionString = "Server=192.168.5.161;Port=3306;Database=bim_revit_zhelezno_plugin_licenses;User ID=bimadmin;Password=ercy352y32c;Connection Timeout=2;";
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
                        bool iteration = false;
                        while (reader.Read())
                        {
                            if (reader["name"].ToString() == user_License.name && reader["surname"].ToString() == user_License.surname && reader["password"].ToString() == user_License.password)
                            {
                                Get_User_Name get_User_Name_Value = new Get_User_Name();
                                string a = get_User_Name_Value.Get_User_Name_Value(user_License.id);
                                string s = SSDK_Data.licenses_Name;

                                if (get_User_Name_Value.Get_User_Name_Value(user_License.id).ToString() == SSDK_Data.userName)
                                {
                                    Set_User_Name set_User_Name = new Set_User_Name(user_License.id);
                                    iteration = true;
                                }
                                else
                                {
                                    SSDK_Data.licenses_Connection = false;
                                }

                                if(request.ContainsKey("Время открытия") || request.ContainsKey("Дата открытия") || request.ContainsKey("Время синхронизации"))
                                {
                                    string user = $"{reader["name"].ToString()} {reader["surname"].ToString()} {reader["patronymic"].ToString()}";
                                    string post = reader["post"].ToString();
                                    Set_User_Connections_Open set_User_Connections_Open = new Set_User_Connections_Open(user_License.id , user , request , post);
                                }
                                if (request.ContainsKey("Количество элементов модели") )
                                {
                                    string user = $"{reader["name"].ToString()} {reader["surname"].ToString()} {reader["patronymic"].ToString()}";
                                    string post = reader["post"].ToString();
                                    Set_User_Model_Detalisation set_User_Connections_Open = new Set_User_Model_Detalisation(user_License.id, user, request, post);
                                }
                            }
                            else
                            {
                                SSDK_Data.licenses_Connection = false;
                            }
                        }
                        if (iteration) 
                        {
                            SSDK_Data.licenses_Connection = true;
                            SSDK_Data.licenses_Id = user_License.id;
                            SSDK_Data.licenses_Name = user_License.name;
                            SSDK_Data.licenses_Surname = user_License.surname;
                            SSDK_Data.licenses_Patronomic = user_License.patronymic;
                            SSDK_Data.licenses_Post = user_License.post;
                            SSDK_Data.licenses_Password = user_License.password;
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
            string connectionString = "Server=192.168.5.161;Port=3306;Database=bim_revit_zhelezno_plugin_licenses;User ID=bimadmin;Password=ercy352y32c;Connection Timeout=2;";
            string use_name = SSDK_Data.userName;
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
    public class Set_User_Connections_Open
    {
        public Set_User_Connections_Open(int i , string user ,Dictionary<string,string> dic , string post)
        {
            if (SSDK_Data.licenses_Post != "BIM координатор" && SSDK_Data.licenses_Post != "BIM менеджер")
            {
                // Строка подключения
                string connectionString = "Server=192.168.5.161;Port=3306;Database=bim_revit_zhelezno_plugin_licenses;User ID=bimadmin;Password=ercy352y32c;Connection Timeout=2;";
                // Создание соединения
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        // Открытие подключения
                        connection.Open();

                        // SQL-запрос для обновления user_name
                        string query = "INSERT INTO data_base_connection (id_persons, user, date_open, file_name, time_open, time_synchr , post, code_erp, stage, file_path, file_Information) " +
                            "VALUES (@id_persons, @user, @date_open, @file_name, @time_open , @time_synchr,  @post, @code_erp, @stage, @file_path, @file_Information)";
                        // Создание команды
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            string file_name = "0";
                            if (dic.ContainsKey("Имя файла"))
                            {
                                file_name = dic["Имя файла"].ToString();
                            }
                            string timeLimit = "0";
                            if (dic.ContainsKey("Время открытия"))
                            {
                                timeLimit = dic["Время открытия"].ToString();
                            }
                            string timeSync = "0";
                            if (dic.ContainsKey("Время синхронизации"))
                            {
                                timeSync = dic["Время синхронизации"].ToString();
                            }
                            string codeErp = "Failed";
                            if (dic.ContainsKey("ZH_1С"))
                            {
                                codeErp = dic["ZH_1С"];
                            }
                            string filePath = "Failed";
                            if (dic.ContainsKey("Путь к файлу"))
                            {
                                filePath = dic["Путь к файлу"];
                            }
                            string statusProgect = "Failed";
                            if (dic.ContainsKey("Статус проекта"))
                            {
                                statusProgect = dic["Статус проекта"];
                            }
                            string fileInformation = "Failed";
                            if (dic.ContainsKey("Информация о документе"))
                            {
                                fileInformation = dic["Информация о документе"];
                            }
                            // Добавление параметров
                            command.Parameters.AddWithValue("@user", user); // Передаем новое имя пользователя
                            command.Parameters.AddWithValue("@id_persons", i);
                            command.Parameters.AddWithValue("@file_name", file_name);
                            command.Parameters.AddWithValue("@time_open", timeLimit);
                            command.Parameters.AddWithValue("@time_synchr", timeSync);
                            command.Parameters.AddWithValue("@post", post);
                            command.Parameters.AddWithValue("@date_open", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));// Указываем идентификатор строки
                            command.Parameters.AddWithValue("@code_erp", codeErp);
                            command.Parameters.AddWithValue("@stage", statusProgect);
                            command.Parameters.AddWithValue("@file_path", filePath);
                            command.Parameters.AddWithValue("@file_Information", fileInformation);
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
    }
    public class Set_User_Model_Detalisation
    {
        public Set_User_Model_Detalisation(int i, string user, Dictionary<string, string> dic, string post)
        {
            if (SSDK_Data.licenses_Post != "BIM координатор" && SSDK_Data.licenses_Post != "BIM менеджер")
            {
                // Строка подключения
                string connectionString = "Server=192.168.5.161;Port=3306;Database=bim_revit_zhelezno_plugin_licenses;User ID=bimadmin;Password=ercy352y32c;Connection Timeout=2;";
                // Создание соединения
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        // Открытие подключения
                        connection.Open();

                        // SQL-запрос для обновления user_name
                        string query = "INSERT INTO data_base_elements (id_persons, user, date, file_name, post, number_elements, code_erp, stage, file_path, file_Information) " +
                            "VALUES (@id_persons, @user, @date, @file_name,  @post,  @number_elements, @code_erp, @stage, @file_path, @file_Information)";
                        // Создание команды
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            string fileName = "0";
                            if (dic.ContainsKey("Имя файла"))
                            {
                                fileName = dic["Имя файла"].ToString();
                            }
                            int collectionsElements = 0;
                            if (dic.ContainsKey("Количество элементов модели"))
                            {
                                collectionsElements = Int32.Parse(dic["Количество элементов модели"]);
                            }
                            string codeErp = "Failed";
                            if (dic.ContainsKey("ZH_1С"))
                            {
                                codeErp = dic["ZH_1С"];
                            }
                            string filePath = "Failed";
                            if (dic.ContainsKey("Путь к файлу"))
                            {
                                filePath = dic["Путь к файлу"];
                            }
                            string statusProgect = "Failed";
                            if (dic.ContainsKey("Статус проекта"))
                            {
                                statusProgect = dic["Статус проекта"];
                            }
                            string fileInformation = "Failed";
                            if (dic.ContainsKey("Информация о документе"))
                            {
                                fileInformation = dic["Информация о документе"];
                            }
                            // Добавление параметров
                            command.Parameters.AddWithValue("@user", user); // Передаем новое имя пользователя
                            command.Parameters.AddWithValue("@id_persons", i);
                            command.Parameters.AddWithValue("@file_name", fileName);
                            command.Parameters.AddWithValue("@post", post);
                            command.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));// Указываем идентификатор строки
                            command.Parameters.AddWithValue("@number_elements", collectionsElements);
                            command.Parameters.AddWithValue("@code_erp", codeErp);
                            command.Parameters.AddWithValue("@stage", statusProgect);
                            command.Parameters.AddWithValue("@file_path", filePath);
                            command.Parameters.AddWithValue("@file_Information", fileInformation);
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
    }
    public class Get_User_Name
    {
        public string Get_User_Name_Value(int i)
        {
            // Строка подключения
            string connectionStringa = "Server=192.168.5.161;Port=3306;Database=bim_revit_zhelezno_plugin_licenses;User ID=bimadmin;Password=ercy352y32c;Connection Timeout=2;";
            // Создание соединения
            using (MySqlConnection connection = new MySqlConnection(connectionStringa))
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
                User_license user_License = new User_license(0, "0", "0", "0", "0", "0", "0");
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
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Autodesk",
            "Revit",
            "Addins",
            "2024",
            "ZHELEZNO_PLUGIN"
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
        public string patronymic { get; set; } = "";
        public string status { get; set; } = "";
        public string post { get; set; } = "";
        public string password { get; set; } = "";
        public User_license(int Id, string Name, string Surname, string Patronymic, string Status, string Post, string Password)
        {
            this.id = Id;
            this.name = Name;
            this.surname = Surname;
            this.patronymic = Patronymic;
            this.status = Status;
            this.post = Post;
            this.password = Password;
        }
    }

}


