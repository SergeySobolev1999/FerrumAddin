using Autodesk.Revit.UI;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
using SSDK;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;
using WPFApplication.Licenses;
using static ClosedXML.Excel.XLPredefinedFormat;
using static FerrumAddin.ModelStatistics.StatisticsViewModel;
using System.Diagnostics;
using System.Windows.Threading;


namespace FerrumAddin.ModelStatistics
{
    public class StatisticsViewModel : INotifyPropertyChanged
    {
        public StatisticsViewModel(ExternalCommandData commandData)
        {
            _externalCommandData = commandData;
            FoldersPreset foldersPreset = new FoldersPreset();
            foldersPreset.Folder_Presets();
            _uiDispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher; ;
            // Инициализация коллекций
            CollectionsStatisticsData = new ObservableCollection<StatisticData>();
            CollectionsStatisticsModelElements = new ObservableCollection<StatisticModelElements>();
            // Инициализация CollectionViewSource
            FilteredConnectionsData = new ListCollectionView(CollectionsStatisticsData);
            FilteredConnectionsData.Filter = item =>
            {
                var data = item as StatisticData;
                if (data == null) return false;

                return
                    (string.IsNullOrEmpty(FilterFileNameConnections) || data.File_Name.Contains(FilterFileNameConnections)) &&
                    (string.IsNullOrEmpty(FilterPostConnections) || data.Post.Contains(FilterPostConnections)) &&
                    (string.IsNullOrEmpty(FilterUserConnections) || data.User.Contains(FilterUserConnections)) &&
                    (string.IsNullOrEmpty(FilterDateOpenConnections) || data.Date_Open.Contains(FilterDateOpenConnections)) &&
                    (string.IsNullOrEmpty(FilterTimeOpenConnections) || data.Time_Open.Contains(FilterTimeOpenConnections)) &&
                    (string.IsNullOrEmpty(FilterTimeSynchrConnections) || data.Time_Synchr.Contains(FilterTimeSynchrConnections)) &&
                    (string.IsNullOrEmpty(FilterSecuenceNumberConnections) || data.Secuence_Number.Contains(FilterSecuenceNumberConnections)) &&
                    (string.IsNullOrEmpty(FilterIdPersonsConnections) || data.Id_Persons.Contains(FilterIdPersonsConnections));
            };
            FilteredModelsData = new ListCollectionView(CollectionsStatisticsModelElements);
            FilteredModelsData.Filter = item =>
            {
                var data = item as StatisticModelElements;
                if (data == null) return false;

                return
                    (string.IsNullOrEmpty(FilterFileNameModels) || data.File_Name.Contains(FilterFileNameModels)) &&
                    (string.IsNullOrEmpty(FilterPostModels) || data.Post.Contains(FilterPostModels)) &&
                    (string.IsNullOrEmpty(FilterUserModels) || data.User.Contains(FilterUserModels)) &&
                    (string.IsNullOrEmpty(FilterDateModels) || data.Date.Contains(FilterDateModels)) &&
                    (string.IsNullOrEmpty(FilterNumberElementsModels) || data.Number_Elements.Contains(FilterNumberElementsModels)) &&
                    (string.IsNullOrEmpty(FilterSecuenceNumberModels) || data.Secuence_Number.Contains(FilterSecuenceNumberModels)) &&
                    (string.IsNullOrEmpty(FilterIdPersonsModels) || data.Id_Persons.Contains(FilterIdPersonsModels));
            };

            DownloadStatistics = new RelayCommand(ExecuteDownloadStatistics);
        }
        private readonly ExternalCommandData _externalCommandData;
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<StatisticData> _collectionsStatisticsData ;
        private ObservableCollection<StatisticModelElements> _collectionsStatisticsModelElements ;
        private string _filterFileNameConnections;
        private string _filterPostConnections;
        private string _filterUserConnections;
        private string _filterDateOpenConnections;
        private string _filterTimeOpenConnections;
        private string _filterTimeSynchrConnections;
        private string _filterSecuenceNumberConnections;
        private string _filterIdPersonsConnections;
        // Фильтры для вкладки "Модели"
        private string _filterFileNameModels;
        private string _filterPostModels;
        private string _filterUserModels;
        private string _filterDateModels;
        private string _filterNumberElementsModels;
        private string _filterSecuenceNumberModels;
        private string _filterIdPersonsModels;
        public ICollectionView _filteredConnectionsData {  get; set; }
        public ICollectionView _filteredModelsData { get; set; }
        private readonly Dispatcher _uiDispatcher;


        public ICommand DownloadStatistics { get; }
        // Свойства для фильтров (Connections)

       
        public ICollectionView FilteredConnectionsData
        {
            get => _filteredConnectionsData;
            set
            {
                Debug.WriteLine("Фильтрованная коллекция присвоена");
                _filteredConnectionsData = value;
                OnPropertyChanged();
            }
        }
        public ICollectionView FilteredModelsData
        {
            get => _filteredModelsData;
            set
            {
                _filteredModelsData = value;
                OnPropertyChanged();
            }
        }
        public string FilterFileNameConnections
        {
            get => _filterFileNameConnections;
            set
            {
                _filterFileNameConnections = value;
                OnPropertyChanged();
                FilteredConnectionsData?.Refresh();
            }
        }

        public string FilterPostConnections
        {
            get => _filterPostConnections;
            set
            {
                _filterPostConnections = value;
                OnPropertyChanged();
                _uiDispatcher.InvokeAsync(() =>
                {
                    FilteredConnectionsData?.Refresh();
                });
            }
        }
        public string FilterUserConnections
        {
            get => _filterUserConnections;
            set
            {
                _filterUserConnections = value;
                OnPropertyChanged();
                _uiDispatcher.InvokeAsync(() =>
                {
                    FilteredConnectionsData?.Refresh();
                });
            }
        }
        public string FilterDateOpenConnections
        {
            get => _filterDateOpenConnections;
            set
            {
                _filterDateOpenConnections = value;
                OnPropertyChanged();
                _uiDispatcher.InvokeAsync(() =>
                {
                    FilteredConnectionsData?.Refresh();
                });
            }
        }
       
        public string FilterTimeOpenConnections
        {
            get => _filterTimeOpenConnections;
            set
            {
                _filterTimeOpenConnections = value;
                OnPropertyChanged();
                _uiDispatcher.InvokeAsync(() =>
                {
                    FilteredConnectionsData?.Refresh();
                });
            }
        }
        public string FilterTimeSynchrConnections
        {
            get => _filterTimeSynchrConnections;
            set
            {
                _filterTimeSynchrConnections = value;
                OnPropertyChanged();
                _uiDispatcher.InvokeAsync(() =>
                {
                    FilteredConnectionsData?.Refresh();
                });
            }
        }
        public string FilterSecuenceNumberConnections
        {
            get => _filterSecuenceNumberConnections;
            set
            {
                _filterSecuenceNumberConnections = value;
                OnPropertyChanged();
                _uiDispatcher.InvokeAsync(() =>
                {
                    FilteredConnectionsData?.Refresh();
                });
            }
        }
        public string FilterIdPersonsConnections
        {
            get => _filterIdPersonsConnections;
            set
            {
                _filterIdPersonsConnections = value;
                OnPropertyChanged();
                _uiDispatcher.InvokeAsync(() =>
                {
                    FilteredConnectionsData?.Refresh();
                });
            }
        }
        public string FilterFileNameModels
        {
            get => _filterFileNameModels;
            set
            {
                _filterFileNameModels = value;
                OnPropertyChanged();
                _uiDispatcher.InvokeAsync(() =>
                {
                    FilteredModelsData?.Refresh();
                });
            }
        }

        public string FilterPostModels
        {
            get => _filterPostModels;
            set
            {
                _filterPostModels = value;
                OnPropertyChanged();
                _uiDispatcher.InvokeAsync(() =>
                {
                    FilteredModelsData?.Refresh();
                });
            }
        }
        public string FilterUserModels
        {
            get => _filterUserModels;
            set
            {
                _filterUserModels = value;
                OnPropertyChanged();
                _uiDispatcher.InvokeAsync(() =>
                {
                    FilteredModelsData?.Refresh();
                });
            }
        }
        public string FilterDateModels
        {
            get => _filterDateModels;
            set
            {
                _filterDateModels = value;
                OnPropertyChanged();
                _uiDispatcher.InvokeAsync(() =>
                {
                    FilteredModelsData?.Refresh();
                });
            }
        }

        public string FilterNumberElementsModels
        {
            get => _filterNumberElementsModels;
            set
            {
                _filterNumberElementsModels = value;
                OnPropertyChanged();
                _uiDispatcher.InvokeAsync(() =>
                {
                    FilteredModelsData?.Refresh();
                });
            }
        }
        public string FilterSecuenceNumberModels
        {
            get => _filterSecuenceNumberModels;
            set
            {
                _filterSecuenceNumberModels = value;
                OnPropertyChanged();
                _uiDispatcher.InvokeAsync(() =>
                {
                    FilteredModelsData?.Refresh();
                });
            }
        }
        public string FilterIdPersonsModels
        {
            get => _filterIdPersonsModels;
            set
            {
                _filterIdPersonsModels = value;
                OnPropertyChanged();
                _uiDispatcher.InvokeAsync(() =>
                {
                    FilteredModelsData?.Refresh();
                });
            }
        }
        public ObservableCollection<StatisticData> CollectionsStatisticsData
        {
            get => _collectionsStatisticsData;
            set
            {
                _collectionsStatisticsData = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<StatisticModelElements> CollectionsStatisticsModelElements
        {
            get => _collectionsStatisticsModelElements;
            set
            {
                _collectionsStatisticsModelElements = value;
                OnPropertyChanged();
            }
        }
       
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public void ExecuteDownloadStatistics(object obj)
        {
            try
            {
                Serialize deserialize = new Serialize();
                Save save = new Save();
                save.presets_Save();
                if (SSDK_Data.licenses_Post == "BIM координатор" || SSDK_Data.licenses_Post == "BIM менеджер" || SSDK_Data.licenses_Post == "Конструктор расчетчик")
                {
                    //if (1 != 2)
                    //{
                    // Строка подключения
                    string connectionString = "Server=192.168.5.161;Port=3306;Database=bim_revit_zhelezno_plugin_licenses;User ID=bimadmin;Password=ercy352y32c;Connection Timeout=2;";
                    // Создание соединения
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {

                        // Открытие соединения
                        connection.Open();
                        // Выполнение SQL-запроса
                        string query = "SELECT * FROM users";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        User_license user_License = deserialize.DeserializeXML();
                        if (user_License != null)
                        {
                            bool conections = false;
                            // Чтение данных
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (reader["name"].ToString() == user_License.name && reader["surname"].ToString() == user_License.surname && reader["password"].ToString() == user_License.password && conections == false)
                                    {
                                        Get_User_Name get_User_Name_Value = new Get_User_Name();
                                        conections = true;

                                    }
                                }
                            }
                            if (conections)
                            {
                                Statistics(connection);
                                FilteredConnectionsData.Refresh();
                                StatisticsModelElements(connection);
                                FilteredModelsData.Refresh();
                            }
                        }
                        else
                        {
                            S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Выполните переподключение к серверу лицензий");
                            s_Mistake_String.ShowDialog();
                        }
                    }
                }
                else
                {
                    S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Вы не являетесь BIM координатором или BIM менеджереом");
                    s_Mistake_String.ShowDialog();
                }
            }
            catch (MySqlException ex)
            {
                SSDK_Data.licenses_Connection = false;
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }
        public class Save
        {
            public void presets_Save()
            {
                string path = SSDK_Data.folder_Preset;

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
        public void Statistics(MySqlConnection connection)
        {
            try
            {
                // Выполнение SQL-запроса
                string query = "SELECT * FROM data_base_connection";
            MySqlCommand command = new MySqlCommand(query, connection);
            // Чтение данных
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    ObservableCollection<StatisticData> statisticDatas = new ObservableCollection<StatisticData>();
                    while (reader.Read())
                    {
                        statisticDatas.Add(new StatisticData(reader["sequence number"].ToString()
                        , reader["id_persons"].ToString()
                        , reader["file_name"].ToString()
                        , reader["post"].ToString()
                        , reader["user"].ToString()
                        , reader["date_open"].ToString()
                        , reader["time_open"].ToString()
                        , reader["time_synchr"].ToString()));
                    }
                    CollectionsStatisticsData.Clear();
                    foreach (StatisticData data in statisticDatas)
                    {
                        CollectionsStatisticsData.Add(data);
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
        public void StatisticsModelElements(MySqlConnection connection)
        {
            try
            {
                // Выполнение SQL-запроса
                string query = "SELECT * FROM data_base_elements";
            MySqlCommand command = new MySqlCommand(query, connection);
            // Чтение данных
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                ObservableCollection<StatisticModelElements> statisticModelElements = new ObservableCollection<StatisticModelElements>();
                while (reader.Read())
                {
                    string sequence_Number = reader["num"].ToString();
                    string id_Persons = reader["id_persons"].ToString();
                    string file_Name = reader["file_name"].ToString();
                    string post = reader["post"].ToString();
                    string user = reader["user"].ToString();
                    string date = reader["date"].ToString();
                    string number_Elements = reader["number_elements"].ToString();
                    statisticModelElements.Add(new StatisticModelElements(sequence_Number, id_Persons, file_Name, post, user, date, number_Elements));
                }
                CollectionsStatisticsModelElements.Clear();
                foreach (StatisticModelElements data in statisticModelElements)
                {
                    CollectionsStatisticsModelElements.Add(data);
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
        public class RelayCommand : ICommand
        {
            private readonly Action<object> _execute;
            private readonly Func<object, bool> _canExecute;

            public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }

            public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

            public void Execute(object parameter) => _execute(parameter);
        }
    }
    public class Serialize
    {
   
        XmlSerializer xml_Set = new XmlSerializer(typeof(User_license));
        User_license user_License = new User_license();
        public void SerializeXML2(User_license user_License)
        {
            try
            {
                using (FileStream fs2 = new FileStream(SSDK_Data.folder_Preset + @"\Serialize_Set_Creating_Specifications.xml", FileMode.Create))
            {
                xml_Set.Serialize(fs2, user_License);
            }
            }
            catch (MySqlException ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }
        public User_license DeserializeXML()
        {
            try
            {
                using (FileStream fs = new FileStream((SSDK_Data.folder_Preset + @"\Serialize_Set_Creating_Specifications.xml"), FileMode.OpenOrCreate))
            {
                return (User_license)xml_Set.Deserialize(fs);
            }
            }
            catch (MySqlException ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
            return null;
        }
       
    }
    public class FoldersPreset
    {
        public string Folder_Presets()
        {
            try
            {
                string path = Folder_Base_Way() + @"\\Presets";
            SSDK_Data.folder_Preset = path;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
                return path;
            }
            catch (MySqlException ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
            return "";

        }
        public string Folder_Base_Way()
        {
            try
            {
                string basic_Path = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Autodesk",
            "Revit",
            "Addins",
            "2024",
            "ZHELEZNO_PLUGIN"
            );
            return basic_Path;
            }
            catch (MySqlException ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
            return "";
        }
    }


}
