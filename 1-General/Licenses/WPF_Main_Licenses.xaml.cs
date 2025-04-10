﻿using Google.Protobuf.Compiler;
using MySql.Data.MySqlClient;
using SSDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.RightsManagement;
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
            Version.Text = SSDK_Data.plugin_Version;
            Folder folder = new Folder();
            Save save = new Save();
            save.presets_Save();
            Licenses_True();
        }
        public void Mysql_TEST()
        {
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
                    // Чтение данных
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        bool iterarion = false;
                        while (reader.Read())
                        {
                            if (reader["name"].ToString() == TextBox_Name.Text && reader["surname"].ToString() == TextBox_Surname.Text && reader["password"].ToString() == TextBox_Password.Password)
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
                                SSDK_Data.licenses_Connection = true;
                               

                            }
                            else
                            {
                                status.Text = "Статус подключения: введены некорректные данные";
                                Data.licenses_Value = false;
                                User_license user_License = new User_license(0, "0", "0", "0", "0", "0", "0");
                                Serialize serialize = new Serialize();
                                serialize.SerializeXML2(user_License);
                                SSDK_Data.licenses_Connection = false;
                            }
                        }
                        if (iterarion)
                        {
                            status.Text = "Статус подключения: подключено";
                            Data.licenses_Value = true;
                            Set_User_Name set_User_Name = new Set_User_Name(Int32.Parse(reader["id_persons"].ToString()));
                            Data.user_License = new User_license(Data.id_persons, Data.name, Data.surname, Data.patronymic, Data.status, Data.post, Data.password);
                            Serialize serialize = new Serialize();
                            serialize.SerializeXML2(Data.user_License);
                            SSDK_Data.licenses_Connection = true;
                            SSDK_Data.licenses_Id = Data.id_persons;
                            SSDK_Data.licenses_Name = Data.name;
                            SSDK_Data.licenses_Surname = Data.surname;
                            SSDK_Data.licenses_Patronomic = Data.patronymic;
                            SSDK_Data.licenses_Status = Data.status;
                            SSDK_Data.licenses_Post = Data.post;
                            SSDK_Data.licenses_Password = Data.password;
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
                    User_license user_License = new User_license(0, "0", "0", "0", "0", "0", "0");
                    Serialize serialize = new Serialize();
                    serialize.SerializeXML2(user_License);
                    SSDK_Data.licenses_Connection = false;
                }
            }

        }
        public void Licenses_True()
        {
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
                        bool iterarion = false;
                        while (reader.Read())
                        {
                            if (reader["name"].ToString() == user_License.name && reader["surname"].ToString() == user_License.surname && reader["password"].ToString() == user_License.password)
                            {
                                Get_User_Name get_User_Name_Value = new Get_User_Name();
                                if (get_User_Name_Value.Get_User_Name_Value(user_License.id) == SSDK_Data.userName)
                                {
                                    iterarion = true;
                                    Data.licenses_Value = true;
                                    TextBox_Name.Text = user_License.name;
                                    TextBox_Surname.Text = user_License.surname;
                                    TextBox_Password.Password = user_License.password;
                                    Set_User_Name set_User_Name = new Set_User_Name(user_License.id);
                                    
                                }
                                else
                                {
                                    SSDK_Data.licenses_Connection = false;
                                    status.Text = "Статус подключения: лицензия занята";
                                    
                                }
                            }
                            else
                            {
                                SSDK_Data.licenses_Connection = false;
                                status.Text = "Статус подключения: лицензия недоступна";
                            }
                        }
                        if (iterarion)
                        {
                            SSDK_Data.licenses_Connection = true;
                            status.Text = "Статус подключения: подключено";
                            Data.licenses_Value = true;
                            SSDK_Data.licenses_Id = user_License.id;
                            SSDK_Data.licenses_Name = user_License.name;
                            SSDK_Data.licenses_Surname = user_License.surname;
                            SSDK_Data.licenses_Patronomic = user_License.patronymic;
                            SSDK_Data.licenses_Status = user_License.status;
                            SSDK_Data.licenses_Post = user_License.post;
                            SSDK_Data.licenses_Password = user_License.password;
                        }
                        if (!iterarion)
                        {
                            SSDK_Data.licenses_Connection = false;
                            status.Text = "Статус подключения: переподключитесь к серверу лицензий";
                            Data.licenses_Value = false;
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    if (ex.Number == 0) // Код ошибки для недоступного сервера
                    {
                        SSDK_Data.licenses_Connection = false;
                        status.Text = "Статус подключения: сервер недоступен";
                        Data.licenses_Value = false;
                    }
                    else
                    {
                        SSDK_Data.licenses_Connection = false;
                        status.Text = "Статус подключения: сервер недоступен";
                        Data.licenses_Value = false;
                    }
                }
            }
        }
        public void Mysql_Connection()
        {
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

        private void Label_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void Button_Click_Licenses(object sender, object e)
        {
            Mysql_TEST();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
    
    
   
    



   
}

