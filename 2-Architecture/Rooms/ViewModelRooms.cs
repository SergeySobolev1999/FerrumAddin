using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Microsoft.Win32;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.IO;
using System.Xml.Serialization;
using SSDK;
using System.Collections.ObjectModel;

using ClosedXML.Excel;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;


namespace WPFApplication.Rooms
{

    public class MyDockablePaneViewModel : INotifyPropertyChanged
    {
        private readonly ExternalEvent _externalEvent;
        private readonly RoomParameterWriter _handler;
        private string _searchText;
        private string _excelPath;
        private string _pluginPathRoom;
        private string _pluginPath;
        private RoomsPosition _selectedRoom;
        private UIApplication _uiapp;
        public ObservableCollection<RoomsPosition> AllRooms { get; } = new ObservableCollection<RoomsPosition>();
        public ICollectionView FilteredRooms { get; private set; }
        public ICommand LoadExcelCommand { get; }
        public ICommand ReloadExcelCommand { get; }
        public ObservableCollection<RoomsPosition> SelectedRooms { get; } = new ObservableCollection<RoomsPosition>();

        public ObservableCollection<string> SelectedFunctionalPurposes { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<string> FunctionalPurposeVariants { get; set; } = new ObservableCollection<string>();

        public RoomsPosition SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                _selectedRoom = value;
                OnPropertyChanged();
            }
        }
        public ICommand StartRoomSelect => new RelayCommand(() =>
        {
            _handler.SetData(SelectedRooms.ToList());
            _externalEvent.Raise();
        });
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    FilteredRooms.Refresh();
                }
            }
        }
        public List<Element> Filtered_Elements(List<Element> elements, Document document)
        {
            List<Element> collections = new List<Element>();
            foreach (Element element in elements)
            {
                var nameAndCat = new Dictionary<string, BuiltInCategory>
                {
                    { "Помещения", BuiltInCategory.OST_Rooms },
                };
                if (element.Category == null) { }
                if (nameAndCat.Values.Contains((BuiltInCategory)element.Category.Id.Value))
                {
                    collections.Add(element);
                }
            }
            return collections;
        }
        public void ReloadMatrix()
        {
            try
            {
                using (var workbook = new XLWorkbook(_pluginPath))
                {
                    var worksheet = workbook.Worksheet("Матрица_помещений"); // или любой нужный лист
                    var rows = worksheet.RangeUsed().RowsUsed().Skip(2); // пропусти заголовок

                    AllRooms.Clear();
                
                    foreach (var row in rows)
                    {
                        if (row.Cells().All(c => string.IsNullOrWhiteSpace(c.GetString())))
                            continue;
                        var name = row.Cell(1).GetString();
                        var typeRoom = row.Cell(3).GetString();
                        var useful = ParsePlusMinus(row.Cell(6).GetString());
                        var estimated = ParsePlusMinus(row.Cell(7).GetString());
                        var func = row.Cell(8).GetString();

                        AllRooms.Add(new RoomsPosition(name, useful, estimated, func, typeRoom));
                    }
               
                }
                FilteredRooms.Refresh();
                RefreshFunctionalPurposeVariants();
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }
        private bool ParsePlusMinus(string value)
        {
            if (value == "+") return true;
            if (value == "-") return false;

            throw new FormatException($"Недопустимое значение '{value}' — ожидается '+' или '-'.");
        }

        private bool FilterRooms(object obj)
        {
            if (obj is RoomsPosition room)
            {
                bool matchesSearch = string.IsNullOrWhiteSpace(SearchText) ||
                                     room.Name?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;

                bool matchesFilter = SelectedFunctionalPurposes.Count == 0 ||
                                     SelectedFunctionalPurposes.Contains(room.FunctionalPurpose);

                return matchesSearch && matchesFilter;
            }
            return false;
        }
        public void RefreshFunctionalPurposeVariants()
        {
            var unique = AllRooms.Select(r => r.FunctionalPurpose)
                                 .Where(s => !string.IsNullOrWhiteSpace(s))
                                 .Distinct()
                                 .OrderBy(x => x)
                                 .ToList();

            FunctionalPurposeVariants.Clear();

            foreach (var item in unique)
                FunctionalPurposeVariants.Add(item);

            OnPropertyChanged(nameof(FunctionalPurposeVariants)); // <--- вот это критично
        }
        public void Initialize(UIApplication uiapp)
        {
            _uiapp = uiapp ?? throw new ArgumentNullException(nameof(uiapp));
            _handler.Initialize(uiapp); // <--- ЭТО ОБЯЗАТЕЛЬНО
        }
        public MyDockablePaneViewModel()
        {
            _handler = new RoomParameterWriter();
            _externalEvent = ExternalEvent.Create(_handler);
            FilteredRooms = CollectionViewSource.GetDefaultView(AllRooms);
            FilteredRooms.Filter = FilterRooms;
            _pluginPathRoom = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Autodesk", "Revit", "Addins", "2024", "ZHELEZNO_PLUGIN", "Presets", "RoomMatrix.xml");
            _pluginPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Autodesk", "Revit", "Addins", "2024", "ZHELEZNO_PLUGIN", "Presets", "RoomExcel.xlsx");
            LoadExcelCommand = new RelayCommand(LoadExcel);
            ReloadExcelCommand = new RelayCommand(ReloaExcel);
        }
        private void StartSelect()
        {
            UIDocument uidoc = _uiapp.ActiveUIDocument;
            ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();
            List<Element> selectedElements = selectedIds.Select(id => uidoc.Document.GetElement(id)).Where(e => e != null).ToList();
            List<Element> collections = new List<Element>();
            int iteration = 0;
            if (selectedIds.Count > 0)
            {
                collections = Filtered_Elements(selectedElements, uidoc.Document);
                if (collections.Count is 0)
                {
                    S_Mistake_String s_Mistake_String = new S_Mistake_String($"Ошибка. Среди предвариельно выделенных элементов нет подходящих для обработки");
                    s_Mistake_String.ShowDialog();
                }
            }
            else
            {
                List<Element> all_Elements = (List<Element>)new FilteredElementCollector(uidoc.Document).WhereElementIsNotElementType().ToElements();
                collections = Filtered_Elements(all_Elements, uidoc.Document);
                if (collections.Count is 0)
                {
                    S_Mistake_String s_Mistake_String = new S_Mistake_String($"Ошибка. Среди элементов модели нет подходящих для обработки");
                    s_Mistake_String.ShowDialog();
                }
            }
            if (collections.Count != 0)
            {
                using (TransactionGroup transactionGroup = new TransactionGroup(uidoc.Document, "Значений в параметры помещений"))
                {
                    transactionGroup.Start();
                    
                    foreach (Element element in collections)
                    {
                        Room r = element as Room;
                        if (r == null) continue;

                        foreach (var roomData in SelectedRooms)
                        {
                            try
                            {
                                using (Transaction t = new Transaction(uidoc.Document, "Запись данных"))
                            {
                                t.Start();
                                int UsefulAreaEstimatedUseful = 0;
                                if(roomData.UsefulArea && roomData.EstimatedUseful) { UsefulAreaEstimatedUseful = 3; }
                                else if (roomData.UsefulArea ) { UsefulAreaEstimatedUseful = 1; }
                                else if (roomData.EstimatedUseful) { UsefulAreaEstimatedUseful = 2; }
                                r.LookupParameter("ZH_Назначение_Помещения")?.Set(UsefulAreaEstimatedUseful);
                                r.LookupParameter("Имя")?.Set(roomData.Name);
                                r.LookupParameter("ПО_Функц.назначение")?.Set(roomData.FunctionalPurpose);
                                t.Commit();
                                iteration++;
                            }
                            }
                            catch (Exception ex)
                            {
                                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                                s_Mistake_String.ShowDialog();
                            }
                        }
                    }
                    transactionGroup.Assimilate();
                    
                }
            }
            if (iteration > 0)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String($"Запись завершена. Успешно обработаных элементов: {iteration.ToString()}");
                s_Mistake_String.ShowDialog();
            }
        }
        private void ReloaExcel()
        {
            string path;
            if (!File.Exists(_pluginPathRoom)) 
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Не найдено сохранение пути матрицы. Укажите путь к матрице заново");
                s_Mistake_String.ShowDialog();
                return;
            }
            var serializer = new XmlSerializer(typeof(XmlPath));
            using (var reader = new StreamReader(_pluginPathRoom))
            {
                XmlPath xmlPath = (XmlPath)serializer.Deserialize(reader);
                path = xmlPath.ExcelPath;
            }
            if (path == null)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Не найден путь к матрице. Укажите путь матрице заново");
                s_Mistake_String.ShowDialog();
                return;
            }
            if (!File.Exists(path))
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Не найдена матрица по сохраненному пути. Укажите путь к матрице заново");
                s_Mistake_String.ShowDialog();
                return;
            }
            _excelPath = path;
            File.Copy(path, _pluginPath, overwrite: true);
            ReloadMatrix();
        }
        private void LoadExcel()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Excel Files|*.xlsx;*.xls",
                Title = "Выберите файл Excel"
            };
           
            if (dialog.ShowDialog() == true)
            {
                _excelPath = dialog.FileName;
                XmlPath xmlPath = new XmlPath(_excelPath);
                var serializer = new XmlSerializer(typeof(XmlPath));
                File.Copy(_excelPath, _pluginPath, overwrite: true);
                using (var writer = new StreamWriter(_pluginPathRoom))
                {
                    serializer.Serialize(writer, xmlPath);
                }
                // Здесь ты грузишь Excel и проклинаешь жизнь
                ReloadMatrix();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    public class XmlPath
    {
        public string ExcelPath { get; set; }
        public XmlPath(string path)
        {
            ExcelPath = path;
        }
        public XmlPath() { }
    }

    public class MyDockablePane : IDockablePaneProvider
    {
        private readonly WPFMainRooms _control;

        public MyDockablePane(WPFMainRooms control)
        {
            _control = control;
        }
        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = _control;

            data.InitialState = new DockablePaneState
            {
                DockPosition = DockPosition.Left
            };
        }
    }


    public class RoomsPosition
    {
        public string Name { get; set; }
        public bool UsefulArea { get; set; }
        public bool EstimatedUseful { get; set; }
        public string FunctionalPurpose { get; set; }
        public string TypeRoom { get; set; }
        public RoomsPosition(string name ,bool usefulArea, bool estimatedUsefu, string functionalPurpose, string typeRoom)
        {
            Name = name;
            UsefulArea = usefulArea;
            EstimatedUseful = estimatedUsefu;
            FunctionalPurpose = functionalPurpose;
            TypeRoom = typeRoom;
        }
    }
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();
        public void Execute(object parameter) => _execute();

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
    public class BoolToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return b ? "Да" : "Нет";
            return "Нет";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as string)?.ToLower() == "да";
        }
    }
    public class RoomParameterWriter : IExternalEventHandler
    {
        private List<RoomsPosition> _selectedRooms;
        private UIApplication _uiapp;

        public void Initialize(UIApplication app)
        {
            _uiapp = app;
        }

        public void SetData(List<RoomsPosition> selectedRooms)
        {
            _selectedRooms = selectedRooms;
        }

        public void Execute(UIApplication app)
        {
            UIDocument uidoc = _uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();
            List<Element> selectedElements = selectedIds.Select(id => doc.GetElement(id)).Where(e => e != null).ToList();

            List<Element> collections = new List<Element>();
            int iteration = 0;

            if (selectedIds.Count > 0)
            {
                collections = selectedElements.Where(e => e is Room).ToList();
            }
            else
            {
                collections = new FilteredElementCollector(doc)
                    .OfClass(typeof(SpatialElement))
                    .WhereElementIsNotElementType()
                    .Where(e => e is Room)
                    .ToList();
            }

            if (collections.Count == 0)
            {
                TaskDialog.Show("Ошибка", "Нет подходящих помещений.");
                return;
            }

            using (TransactionGroup tg = new TransactionGroup(doc, "Значения в параметры помещений"))
            {
                tg.Start();

                foreach (Room r in collections)
                {
                    foreach (var roomData in _selectedRooms)
                    {
                        
                        using (Transaction t = new Transaction(doc, "Запись данных"))
                        {
                            t.Start();
                            int value = 0;
                            if (roomData.UsefulArea && roomData.EstimatedUseful) value = 3;
                            else if (roomData.UsefulArea) value = 1;
                            else if (roomData.EstimatedUseful) value = 2;

                            r.LookupParameter("ZH_Назначение_Помещения")?.Set(value);
                            r.LookupParameter("Имя")?.Set(roomData.Name);
                            r.LookupParameter("ПО_Функц. назначение")?.Set(roomData.FunctionalPurpose);

                            t.Commit();
                            iteration++;
                        }
                        
                    }
                }

                tg.Assimilate();
            }

            TaskDialog.Show("Готово", $"Успешно обработано: {iteration}");
        }

        public string GetName() => nameof(RoomParameterWriter);
    }
}
