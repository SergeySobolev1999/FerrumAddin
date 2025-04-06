using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Controls;
using SSDK;
using Autodesk.Revit.DB.Architecture;

namespace WPFApplication.The_Floor_Is_Numeric_Constructions
{
    public class Record_The_Floor_Is_Numeric
    {
        public void Record_The_Floor_Is_Numeric_Position()
        {
            using (Transaction newT2 = new Transaction(Revit_Document_The_Floor_Is_Numeric.Document, "Запись значения параметра"))
            {
                newT2.Start();
                if (Data_The_Floor_Is_Numeric.supporting_Frame)
                {
                    Record_Door_Window_Wall_Model_Group_The_Floor_Is_Numeric(BuiltInCategory.OST_StructuralFraming);
                }
                if (Data_The_Floor_Is_Numeric.load_Bearing_Columns)
                {
                    Record_Door_Window_Wall_Model_Group_The_Floor_Is_Numeric(BuiltInCategory.OST_StructuralColumns);
                }
                if (Data_The_Floor_Is_Numeric.supporting_Fittings)
                {
                    Record_Door_Window_Wall_Model_Group_The_Floor_Is_Numeric(BuiltInCategory.OST_Rebar);
                }
                if (Data_The_Floor_Is_Numeric.generalized_Models)
                {
                    Record_Door_Window_Wall_Model_Group_The_Floor_Is_Numeric(BuiltInCategory.OST_GenericModel);
                }
                if (Data_The_Floor_Is_Numeric.the_Foundation_Of_The_Supporting_Structures)
                {
                    Record_Door_Window_Wall_Model_Group_The_Floor_Is_Numeric(BuiltInCategory.OST_StructuralFoundation);
                }
                newT2.Commit();
            }
        }
        public void Record_Door_Window_Wall_Model_Group_The_Floor_Is_Numeric(BuiltInCategory category)
        {
            try
            {
                
                FilteredElementCollector window = new FilteredElementCollector(Revit_Document_The_Floor_Is_Numeric.Document);
                ICollection<Element> all_Elements = window.OfCategory(category).WhereElementIsNotElementType().ToElements();
                ICollection<Element> all_Elements_Not_Context = new List<Element>();
                foreach (Element element_Not_Context in all_Elements)
                {
                    if(element_Not_Context.get_Parameter(BuiltInParameter.ELEM_CATEGORY_PARAM).AsValueString() == "Стены")
                    {  
                        if(element_Not_Context.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString() == "Базовая стена"
                        || element_Not_Context.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString() == "Витраж")
                        {
                            all_Elements_Not_Context.Add(element_Not_Context);
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {
                        
                            all_Elements_Not_Context.Add(element_Not_Context);
                       
                    }
                }
                all_Elements = all_Elements_Not_Context;
                foreach (Element element in all_Elements)
                {
                    bool element_true = false;
                    bool set_true = false;
                    int iterationWarning = 0;
                    Element elementHost = element;
                    foreach (string str in Data_The_Floor_Is_Numeric.work_Set_Igonre_Collection.Items)
                    {
                        string parameter_Value = element.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM).AsValueString();
                        if (parameter_Value == str)
                        {
                            element_true = true;
                        }
                    }
                    if(!element_true && !element.get_Parameter(Data_The_Floor_Is_Numeric.parameten_Guid).IsReadOnly)
                    {
                        while (!set_true && iterationWarning < 15 && elementHost != null)
                        {
                            if (elementHost.LevelId != null
                                && Revit_Document_The_Floor_Is_Numeric.Document.GetElement(elementHost.LevelId) != null
                                && Revit_Document_The_Floor_Is_Numeric.Document.GetElement(elementHost.LevelId).get_Parameter(Data_The_Floor_Is_Numeric.parameten_Guid) != null
                                && Revit_Document_The_Floor_Is_Numeric.Document.GetElement(elementHost.LevelId).get_Parameter(Data_The_Floor_Is_Numeric.parameten_Guid).AsDouble() != 0)
                            {
                                Element level_ID = Revit_Document_The_Floor_Is_Numeric.Document.GetElement(elementHost.LevelId);
                                Parameter parameter = level_ID.get_Parameter(Data_The_Floor_Is_Numeric.parameten_Guid);
                                set_true = SSDK_Parameter.Set_Parameter(element.get_Parameter(Data_The_Floor_Is_Numeric.parameten_Guid), parameter.AsDouble());
                                Data_The_Floor_Is_Numeric.number_True_Element = set_true == true ? ++Data_The_Floor_Is_Numeric.number_True_Element : Data_The_Floor_Is_Numeric.number_True_Element;
                            }
                            if (elementHost.get_Parameter(BuiltInParameter.STAIRS_BASE_LEVEL_PARAM) != null
                                && elementHost.get_Parameter(BuiltInParameter.STAIRS_BASE_LEVEL_PARAM).AsElementId() != null
                                && Revit_Document_The_Floor_Is_Numeric.Document.GetElement(elementHost.get_Parameter(BuiltInParameter.STAIRS_BASE_LEVEL_PARAM).AsElementId()) != null
                                && Revit_Document_The_Floor_Is_Numeric.Document.GetElement(elementHost.get_Parameter(BuiltInParameter.STAIRS_BASE_LEVEL_PARAM).AsElementId()).get_Parameter(Data_The_Floor_Is_Numeric.parameten_Guid) != null
                                && Revit_Document_The_Floor_Is_Numeric.Document.GetElement(elementHost.get_Parameter(BuiltInParameter.STAIRS_BASE_LEVEL_PARAM).AsElementId()).get_Parameter(Data_The_Floor_Is_Numeric.parameten_Guid).AsDouble() != 0)
                            {
                                Element level_ID = Revit_Document_The_Floor_Is_Numeric.Document.GetElement(elementHost.get_Parameter(BuiltInParameter.STAIRS_BASE_LEVEL_PARAM).AsElementId());
                                Parameter parameter = level_ID.get_Parameter(Data_The_Floor_Is_Numeric.parameten_Guid);
                                set_true = SSDK_Parameter.Set_Parameter(element.get_Parameter(Data_The_Floor_Is_Numeric.parameten_Guid), parameter.AsDouble());
                                Data_The_Floor_Is_Numeric.number_True_Element = set_true == true ? ++Data_The_Floor_Is_Numeric.number_True_Element : Data_The_Floor_Is_Numeric.number_True_Element;
                            }
                            else if ((elementHost as FamilyInstance) != null && (elementHost as FamilyInstance).Host != null)
                            {
                                elementHost = (element as FamilyInstance).Host;
                            }
                            else if ((elementHost as Railing) != null && (elementHost as Railing).HostId != null)
                            {
                                elementHost = Revit_Document_The_Floor_Is_Numeric.Document.GetElement((elementHost as Railing).HostId);
                            }
                            iterationWarning++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. 2" + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }
        public void Work_Set_Download_The_Floor_Is_Numeric()
        {
            try
            {
                var worksetTable = new FilteredWorksetCollector(Revit_Document_The_Floor_Is_Numeric.Document).OfKind(WorksetKind.UserWorkset).ToList();
            ListView all_Elements = new ListView();
            foreach (Workset workset in worksetTable)
            {
                all_Elements.Items.Add(workset.Name);
            }
            if (all_Elements.Items.Count > 0)
            {
                Data_The_Floor_Is_Numeric.work_Set_Collection = all_Elements;
            }
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }
       
    }
}
