using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApplication.Mark_On_Group_Stained_Glass_Windows;

namespace WPFApplication.Property_Copy
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Main_Class_Property_Copy : IExternalCommand
    {

        private static ExternalEvent _externalEvent;
        private static PropertyCopyHandler _handler = new PropertyCopyHandler();

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            SSDK_Data.username = Environment.UserName;
            if (1==2)
            {
                UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Создаем ExternalEvent (если не создан)
            if (_externalEvent == null)
            {
                _externalEvent = ExternalEvent.Create(_handler);
            }

            WPF_Main_Property_Copy wPF_Main_Property_Copy = new WPF_Main_Property_Copy(commandData);

            wPF_Main_Property_Copy.Closed += (sender, e) =>
            {
                // Передаем данные в обработчик
                List<Parameter_Identification> convertedParameters =
                Data_Class_Property_Copy.parameters.Cast<Parameter_Identification>().ToList();

                _handler.SetData(Data_Class_Property_Copy.elements_Target_Elements, convertedParameters);
                _externalEvent.Raise(); // Запускаем обновление параметров
            };

            wPF_Main_Property_Copy.Show(); // Немодальное окно
            }
            else
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Плагин в разработке");
                s_Mistake_String.ShowDialog();
            }
            return Result.Succeeded;
        }
    }
    public class PropertyCopyHandler : IExternalEventHandler
    {
        private List<Element> elements;
        private List<Parameter_Identification> parameters;

        public void Execute(UIApplication uiApp)
        {
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;

            using (Transaction trans = new Transaction(doc, "Обновление параметров"))
            {
                trans.Start();

                foreach (Element element in elements)
                {
                    foreach (Parameter_Identification parameter_Position in parameters)
                    {
                        Element elementType = doc.GetElement(element.GetTypeId());

                        if (parameter_Position.element_Type_On_Ex == "Тип")
                        {
                            Parameter param = elementType.LookupParameter(parameter_Position.parameter.Definition.Name);
                            if (param != null && !param.IsReadOnly)
                            {
                                if (param.StorageType == StorageType.Integer)
                                    param.Set(parameter_Position.bool_Value == 1 ? 1 : 0);
                                else if (param.StorageType == StorageType.Double)
                                    param.Set(parameter_Position.double_Value);
                                else if (param.StorageType == StorageType.ElementId)
                                {
                                    Material material = new FilteredElementCollector(doc).OfClass(typeof(Material)).Cast<Material>().FirstOrDefault(m => m.Name == parameter_Position.material_Value);

                                    param.Set(material.Id);
                                }
                            }
                        }
                        else
                        {
                            Parameter param_ex = element.LookupParameter(parameter_Position.parameter.Definition.Name);
                            if (param_ex != null && !param_ex.IsReadOnly)
                            {

                                if (param_ex.StorageType == StorageType.Integer)
                                    param_ex.Set(parameter_Position.bool_Value == 1 ? 1 : 0);
                                else if (param_ex.StorageType == StorageType.Double)
                                    param_ex.Set(parameter_Position.double_Value);
                                else if (param_ex.StorageType == StorageType.ElementId)
                                {
                                    Material material_ex = new FilteredElementCollector(doc).OfClass(typeof(Material)).Cast<Material>().FirstOrDefault(m => m.Name == parameter_Position.material_Value);
                                    param_ex.Set(material_ex.Id);
                                }
                            }
                        }
                    }
                }

                doc.Regenerate();
                uidoc.RefreshActiveView();

                trans.Commit();
            }
        }

        public string GetName() => "PropertyCopyHandler";

        public void SetData(List<Element> elements, List<Parameter_Identification> parameters)
        {
            this.elements = elements;
            this.parameters = parameters;
        }
    }
}