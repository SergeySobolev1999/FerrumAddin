using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using Autodesk.Revit.UI.Selection;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Events;
using SSDK;


namespace WPFApplication.newMaterial_Select_Element_Application
{
    public class newViewModelMaterialSelectElementApplication : BaseViewModel
    {
        private readonly ExternalCommandData _commandData;
        private readonly Document _document;
        private FamilyInstance _selectedElement;
        private Material _selectedMaterial;
        private Element _elementDonor;
        private string _filterText;
        public Action CloseAction { get; set; }
        public static ObservableCollection<ParameterCategory> ParameterCategories { get; set; }
        public ObservableCollection<Material> Materials_Download { get; } = new ObservableCollection<Material>();
        private readonly ExternalEvent _externalEvent;
        private readonly MaterialChangeHandler _handler = new MaterialChangeHandler();
        public ICommand CloseWindowCommand { get; }
        public ICommand PickElementCommand { get; }
        public ICommand StartCommand { get; }
        public ICollectionView FilteredMaterials { get; private set; }
        private static int item_Set { get; set;}  = 0;
        

        public FamilyInstance SelectedElement
        {
            get => _selectedElement;
            set
            {
                _selectedElement = value;
                OnPropertyChanged(nameof(SelectedElement));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public Material SelectedMaterial
        {
            get => _selectedMaterial;
            set
            {
                _selectedMaterial = value;
                OnPropertyChanged(nameof(SelectedMaterial));
                CommandManager.InvalidateRequerySuggested(); // Принудительное обновление команды
            }
        }
        private void CloseWindow(object obj)
        {
            CloseAction?.Invoke();
        }
        public newViewModelMaterialSelectElementApplication(ExternalCommandData commandData)
        {
            _commandData = commandData;
            _document = commandData.Application.ActiveUIDocument.Document;
            ParameterCategories = new ObservableCollection<ParameterCategory>();
            OnPropertyChanged(nameof(StartCommand));
            LoadMaterials();
            FilteredMaterials = CollectionViewSource.GetDefaultView(Materials_Download);
            FilteredMaterials.Filter = FilterMaterials;
            PickElementCommand = new RelayCommand(ExecutePickElement);
            StartCommand = new RelayCommand(ExecuteStartCommand, CanExecuteStartCommand);
            CloseWindowCommand = new RelayCommand(CloseWindow);
            _externalEvent = ExternalEvent.Create(_handler);
        }
        private bool FilterMaterials(object obj)
        {
            if (string.IsNullOrEmpty(FilterText)) return true;

            if (obj is Material material)
            {
                return material.Name.IndexOf(FilterText, System.StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return false;
        }
        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
                FilteredMaterials.Refresh();
            }
        }
        public class MaterialChangeHandler : IExternalEventHandler
        {
            private Document _document;
            private Element _elementDonor;
            private Material _selectedMaterial;
            private ObservableCollection<ParameterCategory> _parameterCategories;

            public void Initialize(Document document, Element elementDonor, Material selectedMaterial, ObservableCollection<ParameterCategory> parameterCategories)
            {
                _document = document;
                _elementDonor = elementDonor;
                _selectedMaterial = selectedMaterial;
                _parameterCategories = parameterCategories;
            }

            public void Execute(UIApplication app)
            {
                if (_elementDonor == null || _selectedMaterial == null)
                {
                    return;
                }

                using (Transaction transaction = new Transaction(_document, "Изменение материалов"))
                {
                    item_Set = 0;
                    if (transaction.Start() == TransactionStatus.Started)
                    {
                        foreach (var category in _parameterCategories)
                        {
                            foreach (var group in category.ParametersGroup)
                            {
                                foreach (var param in group.Parameters)
                                {
                                    if (param.IsChecked)
                                    {
                                        //Заполнение параметров по экземпляру
                                        Parameter targetParameter = _elementDonor.LookupParameter(param.Name);
                                        if (targetParameter != null && !targetParameter.IsReadOnly)
                                        {
                                            targetParameter.Set(_selectedMaterial.Id);
                                            item_Set++;
                                        }
                                        //Заполнение параметров по экземпляру
                                        Element element_Type = _document.GetElement(_elementDonor.GetTypeId());
                                        if (element_Type != null)
                                        {
                                            Parameter targetParameterType = element_Type.LookupParameter(param.Name);
                                            if (targetParameterType != null && !targetParameterType.IsReadOnly)
                                            {
                                                targetParameterType.Set(_selectedMaterial.Id);
                                                item_Set++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        transaction.Commit();
                        if (item_Set > 0)
                        {
                            S_Mistake_String s_Mistake_String = new S_Mistake_String("Запись завершена. Успешно обработаных параметров: " + item_Set.ToString());
                            s_Mistake_String.ShowDialog();
                        }
                    }
                }
            }

            public string GetName()
            {
                return "Material Change Handler";
            }
        }
        private void ExecutePickElement(object obj)
        {
            Pick_Element pick_Element = new Pick_Element();
            Element element = pick_Element.Pick_Element_Donor(_commandData);

            if (element is FamilyInstance familyInstance)
            {
                SelectedElement = familyInstance;
                _elementDonor = familyInstance;

                LoadElementParameters(element);
            }
            else
            {
            }
        }

        private void LoadMaterials()
        {
            var collector = new FilteredElementCollector(_document).OfClass(typeof(Material));
            foreach (Material mat in collector)
            {
                Materials_Download.Add(mat);
            }
        }

        private void LoadElementParameters(Element element)
        {
            ParameterCategories.Clear();

            var typeCategory = new ParameterCategory("Тип");
            var instanceCategory = new ParameterCategory("Экземпляр");

            AddParametersToCategory(element, new ParameterGroup("Материалы"), instanceCategory);

            ElementId typeId = element.GetTypeId();
            if (typeId != ElementId.InvalidElementId)
            {
                Element typeElement = _document.GetElement(typeId);
                AddParametersToCategory(typeElement, new ParameterGroup("Материалы"), typeCategory);
            }

            ParameterCategories.Add(typeCategory);
            ParameterCategories.Add(instanceCategory);

            OnPropertyChanged(nameof(ParameterCategories));
        }

        private void AddParametersToCategory(Element element, ParameterGroup parameterGroup, ParameterCategory category)
        {
            ParameterGroup parameterGroupEx = new ParameterGroup(parameterGroup.Name);
            foreach (Parameter param in element.Parameters.Cast<Parameter>())
            {
                string paramName = param.Definition.Name;
                string paramValue = param.AsValueString() ?? param.AsString() ?? "—";

                if (param.Definition.ParameterGroup == BuiltInParameterGroup.PG_MATERIALS && param.Definition.GetDataType().TypeId == "autodesk.spec.aec:material-1.0.0")
                {
                    parameterGroupEx.Parameters.Add(new ParameterItem($"{paramName}: {paramValue}", paramName));
                }
            }
            category.ParametersGroup.Add(parameterGroupEx);
        }

        private bool CanExecuteStartCommand(object obj) => SelectedMaterial != null && SelectedElement != null;

        private void ExecuteStartCommand(object obj)
        {
            try
            {
                if (_elementDonor == null)
            {
                return;
            }

            _handler.Initialize(_document, _elementDonor, SelectedMaterial, ParameterCategories);
                _externalEvent.Raise();
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }
    }

    public class Pick_Element
    {
        public Element Pick_Element_Donor(ExternalCommandData commandData)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document document = uiDoc.Document;

            Selection choices = uiDoc.Selection;
            ISelectionFilter filter = new MassSelectionFilter(document);
            Reference pickedRef = choices.PickObject(ObjectType.Element, filter);
            return pickedRef != null ? document.GetElement(pickedRef) : null;
        }
    }
    public class MassSelectionFilter : ISelectionFilter
    {
        private HashSet<int> categories_Filtered;
        Document document_Position { get; set; }
        private  Guid zh_Cod { get; set; } = new Guid("631cd69e-065f-4ec2-8894-4359325312c3");
        public MassSelectionFilter(Document document)
        {
            document_Position = document;
            List<BuiltInCategory> categories = new List<BuiltInCategory> {
                BuiltInCategory.OST_Doors,
                BuiltInCategory.OST_Windows};
            categories_Filtered = new HashSet<int>(categories.Select(c => (int)c));
        }
        public bool AllowElement(Element element)
        {
            double zh_Cod_ex = 0;
            if (document_Position.GetElement(element.GetTypeId()) != null)
            {
                Element element_Type = document_Position.GetElement(element.GetTypeId());
                if (element_Type.get_Parameter(zh_Cod) != null)
                {
                    zh_Cod_ex = element_Type.get_Parameter(zh_Cod).AsDouble() * 304.8;
                }
            }
            if (element.Category != null && categories_Filtered != null && 206.999 < zh_Cod_ex && zh_Cod_ex < 210.999)
            {
                return true;
            }
            return false;
        }
        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }
    }

    public class ParameterCategory
    {
        public string Name { get; set; }
        public ObservableCollection<ParameterGroup> ParametersGroup { get; set; }

        public ParameterCategory(string name)
        {
            Name = name;
            ParametersGroup = new ObservableCollection<ParameterGroup>();
        }
    }
    public class ParameterGroup : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public ObservableCollection<ParameterItem> Parameters { get; set; }

        private bool _isCheckedGroup;
        public bool IsCheckedGroup
        {
            get => _isCheckedGroup;
            set
            {
                if (_isCheckedGroup != value)
                {
                    _isCheckedGroup = value;
                    OnPropertyChanged(nameof(IsCheckedGroup));

                    // Устанавливаем галочку у всех параметров внутри группы
                    foreach (var param in Parameters)
                    {
                        param.IsChecked = value;
                    }
                }
            }
        }

        public ParameterGroup(string name)
        {
            Name = name;
            Parameters = new ObservableCollection<ParameterItem>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public class ParameterItem : INotifyPropertyChanged
    {
        public string DisplayText { get; set; }
        public string Name { get; set; }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        public ParameterItem(string text, string name)
        {
            DisplayText = text;
            IsChecked = false;
            Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public class Parameter_Identification
    {
        public string element_Type_On_Ex { get; set; } = "";
        public string type_Parameter = null;
        public Parameter parameter { get; set; } = null;
        public double double_Value = 0;
        public string material_Value = null;
        public string ghost_Value = null;
        public int bool_Value = 0;
        public Parameter_Identification(string type_Value, Parameter parameter_Value, double value_Value, string element_Type_On_Ex_Value)
        {
            type_Parameter = type_Value;
            parameter = parameter_Value;
            double_Value = value_Value;
            element_Type_On_Ex = element_Type_On_Ex_Value;
        }
        public Parameter_Identification(string type_Value, Parameter parameter_Value, string value_Value, string element_Type_On_Ex_Value)
        {
            if (parameter_Value.Definition.GetDataType().TypeId == "autodesk.spec.aec:material-1.0.0")
            {
                type_Parameter = type_Value;
                parameter = parameter_Value;
                material_Value = value_Value;
                element_Type_On_Ex = element_Type_On_Ex_Value;
            }
            if (parameter_Value.Definition.GetDataType().TypeId == "autodesk.revit.category.family:genericAnnotation-1.0.0")
            {
                type_Parameter = type_Value;
                parameter = parameter_Value;
                string[] value_Reduction = value_Value.Split(new[] { ":" }, StringSplitOptions.None);
                ghost_Value = value_Reduction[value_Reduction.Count() - 1];
                element_Type_On_Ex = element_Type_On_Ex_Value;
            }
        }
        public Parameter_Identification(string type_Value, Parameter parameter_Value, int value_Value, string element_Type_On_Ex_Value)
        {
            type_Parameter = type_Value;
            parameter = parameter_Value;
            bool_Value = value_Value;
            element_Type_On_Ex = element_Type_On_Ex_Value;
        }
    }
    
  
    
        public abstract class BaseViewModel : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                Debug.WriteLine($"[PROPERTY CHANGED] {propertyName}"); // Логирование
            }

            protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
            {
                if (EqualityComparer<T>.Default.Equals(field, value))
                    return false;

                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }

        /// <summary>
        /// Запускает команду, если она ещё не выполняется
        /// </summary>
        /// <param name="updatingFlag">Флаг выполнения команды</param>
        /// <param name="action">Действие</param>
        /// <returns></returns>
        protected async Task RunCommandAsync(Func<Task> action, bool updatingFlag)
        {
                if (updatingFlag)
                    return;

                updatingFlag = true;
                try
                {
                    await action();
                }
                finally
                {
                    updatingFlag = false;
                }
            }
        }

        /// <summary>
        /// Реализация ICommand для биндинга команд в WPF
        /// </summary>
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