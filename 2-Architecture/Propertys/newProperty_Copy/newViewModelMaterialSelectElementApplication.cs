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


namespace WPFApplication.newProperty_Copy
{
    public class newViewModelProperty_Copy : BaseViewModel
    {
        private readonly ExternalCommandData _commandData;
        private readonly Document _documentDonor;
        private Document _documentTarget;
        private FamilyInstance _selectedElementDonor;
        private ObservableCollection<Element> _selectedElementTarget = new ObservableCollection<Element>();
        private Element _elementDonor;
        private Element _elementDonorType;
        private List<Element> _elementTarget;
        private string _filterText;
        public Action CloseAction { get; set; }
        private readonly ExternalEvent _externalEvent;
        private readonly TargetSetChangeHandler _handler = new TargetSetChangeHandler();
        private ObservableCollection<ParameterCategory> _parameterCategories = new ObservableCollection<ParameterCategory>();
        public ICommand CloseWindowCommand { get; }
        public ICommand PickElementDonorCommand { get; }
        public ICommand PickElementTargetCommand { get; }
        public ICommand StartCommand { get; }
        public ICollectionView FilteredMaterials { get; private set; }
        private static int item_Set { get; set;}  = 0;
        public ObservableCollection<Element> SelectedElementTarget
        {
            get => _selectedElementTarget;
            set
            {
                _selectedElementTarget = value;
                OnPropertyChanged(nameof(SelectedElementTarget));
            }
        }
        private readonly ObservableCollection<Element> _selectedElementsTarget = new ObservableCollection<Element>();

        public ObservableCollection<Element> SelectedElementsTarget
        {
            get => _selectedElementsTarget;
        }
        public ObservableCollection<ParameterCategory> ParameterCategories
        {
            get => _parameterCategories;
            set
            {
                _parameterCategories = value;
                OnPropertyChanged(nameof(ParameterCategories));
            }
        }
        public FamilyInstance SelectedElement
        {
            get => _selectedElementDonor;
            set
            {
                _selectedElementDonor = value;
                OnPropertyChanged(nameof(SelectedElement));
                CommandManager.InvalidateRequerySuggested();
            }
        }
        private void CloseWindow(object obj)
        {
            CloseAction?.Invoke();
        }
        private bool CanExecuteStartCommand(object obj) => SelectedElementTarget != null && SelectedElement != null;
        public newViewModelProperty_Copy(ExternalCommandData commandData)
        {
            _commandData = commandData;
            _documentDonor = commandData.Application.ActiveUIDocument.Document;
            _documentTarget = null;
            ParameterCategories = new ObservableCollection<ParameterCategory>();

            // Проверяем _handler и создаем ExternalEvent
            if (_handler == null)
            {
                _handler = new TargetSetChangeHandler();
            }

            _externalEvent = ExternalEvent.Create(_handler); // 💡 Создание события

            OnPropertyChanged(nameof(StartCommand));
            PickElementDonorCommand = new RelayCommand(ExecutePickElement);
            PickElementTargetCommand = new RelayCommand(ExecutePickElementTarget);
            StartCommand = new RelayCommand(ExecuteStartCommand, CanExecuteStartCommand);
            CloseWindowCommand = new RelayCommand(CloseWindow);
        }

        private void ExecutePickElement(object obj)
        {
            try
            {
                Pick_Element pick_Element = new Pick_Element();
            Element element = pick_Element.Pick_Element_Donor(_commandData);

                if (element is FamilyInstance familyInstance)
                {
                    SelectedElement = familyInstance;
                    _elementDonor = familyInstance;
                    _elementDonorType = _documentDonor.GetElement(familyInstance.GetTypeId());
                    LoadElementParameters(element);

                    // 🚀 Обновляем TreeView
                    OnPropertyChanged(nameof(ParameterCategories));
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void ExecutePickElementTarget(object obj)
        {
            try
            {
                Pick_Element pick_Element = new Pick_Element();
            List<Element> elements = pick_Element.Pick_Element_Target(_commandData);

                if (elements != null && elements.Count > 0)
                {
                    SelectedElementTarget.Clear();
                    foreach (var element in elements)
                    {
                        SelectedElementTarget.Add(element);
                    }
                    _documentTarget = elements.First().Document; // Назначаем правильный документ
                }
                else
                {
                    TaskDialog.Show("Ошибка", "Не выбраны целевые элементы.");
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void LoadElementParameters(Element element)
        {
            ParameterCategories.Clear(); // Очищаем, но не пересоздаём!

            var typeCategory = new ParameterCategory("Тип");
            var instanceCategory = new ParameterCategory("Экземпляр");

            AddParametersToCategory(element, new ParameterGroup("Материалы"), instanceCategory);
            AddParametersToCategory(element, new ParameterGroup("Размеры"), instanceCategory);
            AddParametersToCategory(element, new ParameterGroup("Видимость"), instanceCategory);

            ElementId typeId = element.GetTypeId();
            if (typeId != ElementId.InvalidElementId)
            {
                Element typeElement = _documentDonor.GetElement(typeId);
                AddParametersToCategory(typeElement, new ParameterGroup("Материалы"), typeCategory);
                AddParametersToCategory(typeElement, new ParameterGroup("Размеры"), typeCategory);
                AddParametersToCategory(typeElement, new ParameterGroup("Видимость"), typeCategory);
            }

            // 🚀 Добавляем категории в уже существующую коллекцию
            ParameterCategories.Add(typeCategory);
            ParameterCategories.Add(instanceCategory);
        }
        
        private void AddParametersToCategory(Element element, ParameterGroup parameterGroup, ParameterCategory category)
        {
            ParameterGroup parameterGroupEx = new ParameterGroup(parameterGroup.Name);
            foreach (Parameter param in element.Parameters.Cast<Parameter>())
            {
                string paramName = param.Definition.Name;
                string paramValue = param.AsValueString() ?? param.AsString() ?? "—";

                if (parameterGroup.Name == "Размеры" && param.Definition.ParameterGroup == BuiltInParameterGroup.PG_GEOMETRY
                    && param.Definition.GetDataType().TypeId == "autodesk.spec.aec:length-2.0.0" && !paramName.Contains("ЗТР_Створка") 
                    && paramName != "Примерная_Высота" && paramName != "Примерная_Ширина" && paramName != "Высота" && paramName != "Ширина"
                     && paramName != "Примерная высота" && paramName != "Примерная ширина" && paramName != "Толщина" )
                {
                    parameterGroupEx.Parameters.Add(new ParameterItem($"{paramName}: {paramValue}", paramName));
                }
                if (parameterGroup.Name == "Материалы" && param.Definition.ParameterGroup == BuiltInParameterGroup.PG_MATERIALS
                    && param.Definition.GetDataType().TypeId == "autodesk.spec.aec:material-1.0.0")
                {
                    parameterGroupEx.Parameters.Add(new ParameterItem($"{paramName}: {paramValue}", paramName));
                }
                if (parameterGroup.Name == "Видимость" && param.Definition.ParameterGroup == BuiltInParameterGroup.PG_VISIBILITY && param.Definition.GetDataType().TypeId == "autodesk.spec:spec.bool-1.0.0")
                {
                    parameterGroupEx.Parameters.Add(new ParameterItem($"{paramName}: {paramValue}", paramName));
                }
            }
            category.ParametersGroup.Add(parameterGroupEx);
        }
        private void ExecuteStartCommand(object obj)
        {
            try
            {
                if (_elementDonor == null)
                {
                    TaskDialog.Show("Ошибка", "Не выбран элемент-донор.");
                    return;
                }

                if (SelectedElementTarget == null || SelectedElementTarget.Count == 0)
                {
                    TaskDialog.Show("Ошибка", "Не выбраны целевые элементы.");
                    return;
                }

                if (_documentTarget == null)
                {
                    TaskDialog.Show("Ошибка", "Документ целевых элементов не найден.");
                    return;
                }
                _handler.Initialize(_elementDonorType, _documentTarget, _documentDonor , _elementDonor, SelectedElementTarget.ToList(), ParameterCategories);
                _externalEvent.Raise();
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Ошибка", $"Произошла ошибка: {ex.Message}");
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
        public List<Element> Pick_Element_Target(ExternalCommandData commandData)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document document = uiDoc.Document;

            Selection choices = uiDoc.Selection;
            ISelectionFilter filter = new MassSelectionFilter(document);
            IList<Reference> pickedRefs = choices.PickObjects(ObjectType.Element, filter);

            return pickedRefs != null && pickedRefs.Count > 0
                ? pickedRefs.Select(r => document.GetElement(r)).ToList()
                : new List<Element>();
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

    public class TargetSetChangeHandler : IExternalEventHandler
    {
        private Document _documentTarget;
        private Document _documentDonor;
        private Element _elementDonor;
        private Element _elementDonorType;
        private List<Element> _elementsTargets;
        private ObservableCollection<ParameterCategory> _parameterCategories;
        private int _item_Set = 0;
        public void Initialize(Element elementDonorTypePosition, Document documentTarget, Document documentDonorPosition, Element elementDonor, List<Element> elementsTarget, ObservableCollection<ParameterCategory> parameterCategories)
        {
            _elementDonorType = elementDonorTypePosition;
            _documentTarget = documentTarget;
            _documentDonor = documentDonorPosition;
            _elementDonor = elementDonor;
            _elementsTargets = elementsTarget;
            _parameterCategories = parameterCategories;
        }

        public void Execute(UIApplication app)
        {
            if (_documentTarget == null || _elementDonor == null || _elementDonorType == null || _elementsTargets == null || _parameterCategories == null)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Введены не все данные для копирования");
                s_Mistake_String.ShowDialog();
                return;
            }
            Material_Download_Collection(_elementDonor, _parameterCategories, _documentDonor, _documentTarget);
            using (Transaction transaction = new Transaction(_documentTarget, "Изменение параметров"))
            {
                List<Element> targetCount = new List<Element>();
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
                                    foreach (Element elementtarget in _elementsTargets)
                                    {
                                        //Заполнение параметров по экземпляру
                                        Parameter donorParameter = _elementDonor.LookupParameter(param.Name);
                                        Parameter targetParameter = elementtarget.LookupParameter(param.Name);
                                        if (targetParameter != null 
                                            && !targetParameter.IsReadOnly
                                            && donorParameter.AsValueString() != "<По категории>"
                                            && targetParameter.Definition.Name == donorParameter.Definition.Name
                                            && targetParameter.Definition.GetDataType().TypeId == "autodesk.spec.aec:material-1.0.0"
                                            && new FilteredElementCollector(_documentTarget).OfClass(typeof(Material)).Cast<Material>().FirstOrDefault(m => m.Name ==
                                            _elementDonor.LookupParameter(param.Name).AsValueString()) !=null )
                                        {
                                            Material material = new FilteredElementCollector(_documentTarget).OfClass(typeof(Material)).Cast<Material>().FirstOrDefault(m => m.Name == 
                                            _elementDonor.LookupParameter(targetParameter.Definition.Name).AsValueString());
                                            targetParameter.Set(material.Id);
                                            targetCount.Add(elementtarget);
                                        }
                                        if (targetParameter != null 
                                            && !targetParameter.IsReadOnly
                                            && targetParameter.Definition.Name == donorParameter.Definition.Name
                                            && targetParameter.Definition.GetDataType().TypeId == "autodesk.spec.aec:length-2.0.0")
                                        {
                                            targetParameter.Set(donorParameter.AsDouble());
                                            targetCount.Add(elementtarget);
                                        }
                                        if (targetParameter != null 
                                            && !targetParameter.IsReadOnly
                                            && targetParameter.Definition.Name == donorParameter.Definition.Name
                                            && targetParameter.Definition.GetDataType().TypeId == "autodesk.spec:spec.bool-1.0.0")
                                        {
                                            targetParameter.Set(donorParameter.AsInteger() == 1 ? 1 : 0);
                                            targetCount.Add(elementtarget);
                                        }
                                        //Заполнение параметров по экземпляру
                                        Element elementTargetType = _documentTarget.GetElement(elementtarget.GetTypeId());
                                        if (elementTargetType != null)
                                        {
                                            Parameter targetParameterType = _elementDonorType.LookupParameter(param.Name);
                                            Parameter targetParameterTargetType = elementTargetType.LookupParameter(param.Name);
                                            if (targetParameterTargetType != null 
                                                && !targetParameterTargetType.IsReadOnly
                                                && targetParameterTargetType.Definition.Name == targetParameterType.Definition.Name
                                                && targetParameterType.AsValueString() != "<По категории>"
                                                && targetParameterTargetType.Definition.GetDataType().TypeId == "autodesk.spec.aec:material-1.0.0")
                                            {
                                                Material material = new FilteredElementCollector(_documentTarget).OfClass(typeof(Material)).Cast<Material>().FirstOrDefault(m => m.Name ==
                                                _elementDonorType.LookupParameter(targetParameterType.Definition.Name).AsValueString());
                                                targetParameterTargetType.Set(material.Id);
                                                targetCount.Add(elementtarget);
                                            }
                                            if (targetParameterTargetType != null 
                                                && !targetParameterTargetType.IsReadOnly
                                                && targetParameterTargetType.Definition.Name == targetParameterType.Definition.Name
                                                && targetParameterTargetType.Definition.GetDataType().TypeId == "autodesk.spec.aec:length-2.0.0")
                                            {
                                                targetParameterTargetType.Set(targetParameterType.AsDouble());
                                                targetCount.Add(elementtarget);
                                            }
                                            if (targetParameterTargetType != null 
                                                && !targetParameterTargetType.IsReadOnly
                                                && targetParameterTargetType.Definition.Name == targetParameterType.Definition.Name
                                                && targetParameterTargetType.Definition.GetDataType().TypeId == "autodesk.spec:spec.bool-1.0.0")
                                            {
                                                targetParameterTargetType.Set(targetParameterType.AsInteger() == 1 ? 1 : 0);
                                                targetCount.Add(elementtarget);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                }
                transaction.Commit();
                if (targetCount.Count > 0)
                {
                    S_Mistake_String s_Mistake_String = new S_Mistake_String("Запись завершена. Успешно обработаных параметров: " + targetCount.Distinct().Count().ToString());
                    s_Mistake_String.ShowDialog();
                }
            }
        }

        public string GetName()
        {
            return "Material Change Handler";
        }
        public void Material_Download_Collection(Element elementDonor, ObservableCollection<ParameterCategory> parametersCategory, Document document_Donor, Document document_Target)
        {
            List<ElementId> elementIds = new List<ElementId>();
            foreach (var category in parametersCategory)
            {
                foreach (var group in category.ParametersGroup)
                {
                    foreach (var param in group.Parameters)
                    {
                        if (param.IsChecked
                            && document_Target.GetHashCode() != document_Donor.GetHashCode()
                            && elementDonor.LookupParameter(param.Name) != null
                            && elementDonor.LookupParameter(param.Name).AsValueString() != "<По категории>"
                            && elementDonor.LookupParameter(param.Name).Definition.GetDataType().TypeId == "autodesk.spec.aec:material-1.0.0"
                            && !new FilteredElementCollector(document_Target)
                            .OfClass(typeof(Material))
                            .Cast<Material>()
                            .Any(m => m.Name.Equals(elementDonor.LookupParameter(param.Name).AsValueString(), StringComparison.OrdinalIgnoreCase)))
                        {
                            elementIds.Add(elementDonor.LookupParameter(param.Name).AsElementId());
                        }
                        else if (param.IsChecked
                            && document_Target.GetHashCode() != document_Donor.GetHashCode()
                            && document_Donor.GetElement(elementDonor.GetTypeId()) != null
                            && document_Donor.GetElement(elementDonor.GetTypeId()).LookupParameter(param.Name) != null
                            && document_Donor.GetElement(elementDonor.GetTypeId()).LookupParameter(param.Name).AsValueString() != "<По категории>"
                            && document_Donor.GetElement(elementDonor.GetTypeId()).LookupParameter(param.Name).Definition.GetDataType().TypeId == "autodesk.spec.aec:material-1.0.0"
                            && !new FilteredElementCollector(document_Target)
                            .OfClass(typeof(Material))
                            .Cast<Material>()
                            .Any(m => m.Name.Equals(document_Donor.GetElement(elementDonor.GetTypeId()).LookupParameter(param.Name).AsValueString())))
                        {
                            Element elementType = document_Donor.GetElement(elementDonor.GetTypeId());
                            elementIds.Add(document_Donor.GetElement(elementDonor.GetTypeId()).LookupParameter(param.Name).AsElementId());
                        }
                    }
                }
            }
            if (elementIds.Count > 0)
            {
                using (Transaction trans_Material = new Transaction(document_Target, "Загрузка материалов"))
                {
                    trans_Material.Start();
                    CopyPasteOptions copyPasteOptions = new CopyPasteOptions();
                    ElementTransformUtils.CopyElements(document_Donor, elementIds, document_Target, Transform.Identity, copyPasteOptions);
                    trans_Material.Commit();
                }
            }
        }
    }
}