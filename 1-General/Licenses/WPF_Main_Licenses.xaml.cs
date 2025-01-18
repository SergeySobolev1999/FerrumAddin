using Google.Protobuf.Compiler;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace WPFApplication.Licenses
{
    /// <summary>
    /// Логика взаимодействия для WPF_Main_Licenses.xaml
    /// </summary>
    public partial class WPF_Main_Licenses : Window
    {
        Serialize deserialize = new Serialize();
        public WPF_Main_Licenses()
        {
            InitializeComponent();
            Folder folder = new Folder();
            Save save = new Save();
            save.presets_Save();
            Licenses_True();
        }
        public void Mysql_TEST()
        {
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
                    // Чтение данных
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        bool iterarion = false;
                        while (reader.Read())
                        {
                            if (reader["name"].ToString() == TextBox_Name.Text && reader["surname"].ToString() == TextBox_Surname.Text && reader["password"].ToString() == TextBox_Password.Text)
                            {
                                Folder folder = new Folder();
                                Save save = new Save();
                                save.presets_Save();
                                // Считываем только необходимые данные
                                Data.id_persons = Int32.Parse(reader["id_persons"].ToString());
                                Data.surname = reader["surname"].ToString();
                                Data.name = reader["name"].ToString();
                                Data.patronymic = reader["patronymic"].ToString();
                                Data.status = reader["status"].ToString();
                                Data.post = reader["post"].ToString();
                                Data.password = reader["password"].ToString();
                                Set_User_Name set_User_Name = new Set_User_Name(Data.id_persons);
                                iterarion = true;

                            }
                            else
                            {
                                status.Text = "Статус подключения: введены некорректные данные";
                                Data.licenses_Value = false;
                                User_license user_License = new User_license(0, "0", "0", "0");
                                Serialize serialize = new Serialize();
                                serialize.SerializeXML2(user_License);
                            }
                        }
                        if (iterarion)
                        {
                            status.Text = "Статус подключения: подключено";
                            Data.licenses_Value = true;
                            Set_User_Name set_User_Name = new Set_User_Name(Int32.Parse(reader["id_persons"].ToString()));
                            Data.user_License = new User_license(Data.id_persons, Data.name, Data.surname, Data.password);
                            Serialize serialize = new Serialize();
                            serialize.SerializeXML2(Data.user_License);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    if (ex.Number == 0) // Код ошибки для недоступного сервера
                    {
                        status.Text = "Статус подключения: сервер недоступен";
                        Data.licenses_Value = false;
                    }
                    else
                    {
                        status.Text = "Статус подключения: сервер недоступен";
                        Data.licenses_Value = false;
                    }
                    User_license user_License = new User_license(0, "0", "0", "0");
                    Serialize serialize = new Serialize();
                    serialize.SerializeXML2(user_License);
                }
            }

        }
        public void Licenses_True()
        {
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
                        bool iterarion = false;
                        while (reader.Read())
                        {
                            if (reader["name"].ToString() == user_License.name && reader["surname"].ToString() == user_License.surname && reader["password"].ToString() == user_License.password)
                            {
                                Get_User_Name get_User_Name_Value = new Get_User_Name();
                                if (get_User_Name_Value.Get_User_Name_Value(user_License.id) == Data.username)
                                {
                                    iterarion = true;
                                    Data.licenses_Value = true;
                                    status.Text = "Статус подключения: подключено";
                                    TextBox_Name.Text = user_License.name;
                                    TextBox_Surname.Text = user_License.surname;
                                    TextBox_Password.Text = user_License.password;
                                    Set_User_Name set_User_Name = new Set_User_Name(user_License.id);
                                }
                                else
                                {
                                    status.Text = "Статус подключения: лицензия занята";
                                }
                            }
                            else
                            {
                                status.Text = "Статус подключения: лицензия недоступна";
                            }
                        }
                        if (iterarion)
                        {
                            status.Text = "Статус подключения: подключено";
                            Data.licenses_Value = true;
                        }
                        if (!iterarion)
                        {
                            status.Text = "Статус подключения: переподключитесь к серверу лицензий";
                            Data.licenses_Value = true;
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    if (ex.Number == 0) // Код ошибки для недоступного сервера
                    {
                        status.Text = "Статус подключения: сервер недоступен";
                        Data.licenses_Value = false;
                    }
                    else
                    {
                        status.Text = "Статус подключения: сервер недоступен";
                        Data.licenses_Value = false;
                    }
                }
            }
        }
        public void Mysql_Connection()
        {
            // Строка подключения
            string connectionString = "Server=192.168.1.5;Port=8899;Database=bim_users;User ID=sergey_sobolev;Password=ercy352y32a;Connection Timeout=5;";
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
                    // Чтение данных
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        bool iterarion = false;
                        while (reader.Read())
                        {
                            if (reader["name"].ToString() != "" && reader["name"].ToString() != null)
                            {
                                status.Text = "Статус подключения: сервер доступен";
                                iterarion = true;
                            }
                            else
                            {
                                status.Text = "Статус подключения: сервер недоступен";
                            }
                        }
                        if (iterarion)
                        {
                            status.Text = "Статус подключения: сервер доступен";
                        }
                    }

                }
                catch (MySqlException ex)
                {
                    if (ex.Number == 0) // Код ошибки для недоступного сервера
                    {
                        status.Text = "Статус подключения: сервер недоступен";
                    }
                    else
                    {
                        status.Text = "Статус подключения: сервер недоступен";
                    }
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Mysql_Connection();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Mysql_TEST();
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
    public class USERS_IDENTIFICATONS
    {
        public int id_persons { get; set; } = 0;
        public string surname { get; set; } = "";
        public string name { get; set; } = "";
        public string patronymic { get; set; } = "";
        public string status { get; set; } = "";
        public string post { get; set; } = "";

        public USERS_IDENTIFICATONS(int id_persons, string surname, string name, string patronymic, string status, string post)
        {
            this.id_persons = id_persons;
            this.surname = surname;
            this.name = name;
            this.patronymic = patronymic;
            this.status = status;
            this.post = post;
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
    [Serializable]
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
}

