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
                    foreach (string str in Data_The_Floor_Is_Numeric.work_Set_Igonre_Collection.Items)
                    {
                        string parameter_Value = element.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM).AsValueString();
                        if (parameter_Value == str)
                        {
                            element_true = true;
                        }
                    }
                    if (!element_true && Revit_Document_The_Floor_Is_Numeric.Document.GetElement(element.LevelId) != null)
                    {
                        Element level_ID = Revit_Document_The_Floor_Is_Numeric.Document.GetElement(element.LevelId);
                        element.get_Parameter(Data_The_Floor_Is_Numeric.parameten_Guid).Set(level_ID.get_Parameter(Data_The_Floor_Is_Numeric.parameten_Guid).AsDouble());
                        Data_The_Floor_Is_Numeric.number_True_Element++;
                    }
                    else if (!element_true && element is Railing && (element as Railing).HostId != null && Revit_Document_The_Floor_Is_Numeric.Document.GetElement((element as Railing)?.HostId ?? ElementId.InvalidElementId) != null)
                    {
                        ElementId hostId = (element as Railing)?.HostId ?? ElementId.InvalidElementId;
                        Element host = Revit_Document_The_Floor_Is_Numeric.Document.GetElement(hostId);
                        Element level = Revit_Document_The_Floor_Is_Numeric.Document.GetElement(host.get_Parameter(BuiltInParameter.STAIRS_BASE_LEVEL_PARAM).AsElementId());
                        element.get_Parameter(Data_The_Floor_Is_Numeric.parameten_Guid).Set(level.get_Parameter(Data_The_Floor_Is_Numeric.parameten_Guid).AsDouble());
                        Data_The_Floor_Is_Numeric.number_True_Element++;
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
