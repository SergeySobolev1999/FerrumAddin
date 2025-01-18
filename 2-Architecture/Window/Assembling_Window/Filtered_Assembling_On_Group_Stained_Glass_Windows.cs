using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;



namespace WPFApplication.Assembling_Window
{
    public class Filtered_Assembling_Window
    {
        public void Filtered_Position_Assembling_On_Group_Stained_Glass_Windows(bool false_Update)
        {
            using (Transaction trans = new Transaction(Revit_Document_Assembling_Window.Document, "Фильтрация"))
            {
                trans.Start();
                try
                {
                    FilteredElementCollector window = new FilteredElementCollector(Revit_Document_Assembling_Window.Document);
                    List<Element> all_Elements_Group_Collection = new List<Element>();
                    List<Element> all_Elements_Group_Collection_Shortened = new List<Element>();
                    List<Element> all_Elements_Group_Collection_Last = new List<Element>();
                    ICollection<Element> all_Elements = window.OfCategory(BuiltInCategory.OST_Windows).WhereElementIsNotElementType().ToElements();
                    foreach (Element element in all_Elements)
                    {
                        if (Revit_Document_Assembling_Window.Document.GetElement(element.GetTypeId()) != null)
                        {
                            Element element_Type = Revit_Document_Assembling_Window.Document.GetElement(element.GetTypeId());
                            if (element_Type.get_Parameter(Data_Assembling_Window.guid_COD)!=null)
                            {
                                double parameter_Value = element_Type.get_Parameter(Data_Assembling_Window.guid_COD).AsDouble() * 304.8;
                                if (208.999 < parameter_Value && parameter_Value < 211 && element_Type.LookupParameter("ЮТС_Dynamo_ID") != null)
                                {
                                    all_Elements_Group_Collection_Last.Add(element);
                                    all_Elements_Group_Collection.Add(element);
                                }
                            }
                        }
                    }
                    all_Elements_Group_Collection_Last=all_Elements_Group_Collection_Last.GroupBy(p => p.GetTypeId()).Select(g => g.First()).ToList();
                    foreach (Element group in all_Elements_Group_Collection_Last)
                    {
                        bool iteration = false;
                        AssemblyInstance assemblyInstance = null;
                        string category = group.get_Parameter(BuiltInParameter.ELEM_CATEGORY_PARAM).AsValueString();
                        string mark_Group = Revit_Document_Assembling_Window.Document.GetElement(group.GetTypeId()).get_Parameter(Data_Assembling_Window.guid_ADSK_Mark).AsValueString();
                        if (category != null)
                            {
                                if (category == "Окна")
                                {
                                    if (Revit_Document_Assembling_Window.Document.GetElement(group.AssemblyInstanceId) != null)
                                    {
                                        if (Revit_Document_Assembling_Window.Document.GetElement(group.AssemblyInstanceId).Name.ToString() == mark_Group)
                                        {
                                            assemblyInstance = (AssemblyInstance)Revit_Document_Assembling_Window.Document.GetElement(group.AssemblyInstanceId);
                                            iteration = true;
                                        }
                                    }
                                }
                            }
                        
                        string group_Name = group.Name;
                        Window_Assembly_Position group_Assembly_Position = new Window_Assembly_Position(group, assemblyInstance);
                        Data_Assembling_Window.filtered_Group.Add(group_Assembly_Position);
                        
                    }
                    Data_Assembling_Window.filtered_Group=Data_Assembling_Window.filtered_Group.GroupBy(p => p.group.GetTypeId()).Select(g => g.First()).ToList();
                    Data_Assembling_Window.DataItems.Clear();
                    List<DataItem> DataItems_Sort = new List<DataItem>();
                    if (Data_Assembling_Window.filtered_Group.Count() > 0)
                    {
                        foreach (Window_Assembly_Position group_Assembly_Position in Data_Assembling_Window.filtered_Group)
                        {
                            Element element_Type = Revit_Document_Assembling_Window.Document.GetElement(group_Assembly_Position.group.GetTypeId());

                            string mark_Value = element_Type.get_Parameter(Data_Assembling_Window.guid_ADSK_Mark).AsValueString();
                            string actual = "Х";
                            if (group_Assembly_Position.assemblyInstance != null && Revit_Document_Assembling_Window
                                .Document.GetElement(group_Assembly_Position.group.GetTypeId()).get_Parameter(Data_Assembling_Window
                                .guid_ADSK_Mark).AsValueString() == group_Assembly_Position.assemblyInstance.Name.ToString())
                            {
                                actual = "✓";
                            }
                            string type_Value = element_Type.Name.ToString();
                            string group_ID_Value = group_Assembly_Position.group.Id.ToString();
                            string assembling_ID_Value = "Х";
                            if (group_Assembly_Position.assemblyInstance != null)
                            {
                                assembling_ID_Value = group_Assembly_Position.assemblyInstance.GetTypeId().ToString();
                            }
                            bool update = false_Update;
                            DataItem dataItem = new DataItem(mark_Value, type_Value, actual, group_ID_Value, assembling_ID_Value, update);
                            DataItems_Sort.Add(dataItem);
                        }
                        List<DataItem> DataItems_Sort_Last = DataItems_Sort.OrderBy(x => x.Mark).ToList();
                        foreach (DataItem dataItem_Position in DataItems_Sort_Last)
                        {
                            Data_Assembling_Window.DataItems.Add(dataItem_Position);
                        }

                    }
                }
                catch (Exception ex)
                {
                    S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                    s_Mistake_String.ShowDialog();
                }
                trans.Commit();
            }
        }
        public void Assembly_Delete()
        {
            try
            {
                using (Transaction trans = new Transaction(Revit_Document_Assembling_Window.Document, "Фильтрация"))
                {
                    trans.Start();
                    FilteredElementCollector window = new FilteredElementCollector(Revit_Document_Assembling_Window.Document);
                    List<Element> all_Elements_Assemblys = (List<Element>)window.OfCategory(BuiltInCategory.OST_Assemblies).WhereElementIsNotElementType().ToElements();
                    foreach (AssemblyInstance element in all_Elements_Assemblys)
                    {
                        if (element.AllowsAssemblyViewCreation() == false)
                        {
                            element.Disassemble();
                        }
                    }
                    FilteredElementCollector window2 = new FilteredElementCollector(Revit_Document_Assembling_Window.Document);
                    List<Element> all_Elements_Group = (List<Element>)window2.OfCategory(BuiltInCategory.OST_Windows).WhereElementIsNotElementType().ToElements();
                    List<Element> all_Elements_Group_Collection = new List<Element>();
                    List<Element> all_Elements_Group_Collection_Shortened = new List<Element>();
                    foreach (Element element in all_Elements_Group)
                    {
                        Element element_Type = Revit_Document_Assembling_Window.Document.GetElement(element.GetTypeId());
                        if (element_Type.get_Parameter(Data_Assembling_Window.guid_COD) !=null)
                        {
                            double parameter_Value = element_Type.get_Parameter(Data_Assembling_Window.guid_COD).AsDouble() * 304.8;
                            if (208.999 <= parameter_Value && parameter_Value < 211 && element_Type.LookupParameter("ЮТС_Dynamo_ID") != null)
                            {
                                all_Elements_Group_Collection.Add(element);
                            }
                        }
                    }
                    all_Elements_Group_Collection_Shortened = all_Elements_Group_Collection.GroupBy(p => p.GetTypeId()).Select(g => g.First()).ToList();
                    List<Element> all_Elements_Group_Collection_Filtered_With_Assembly = new List<Element>();
                    List<Element> all_Elements_Group_Collection_Filtered_Dont_Assembly = new List<Element>();

                    foreach (Element group_Position_Filtered in all_Elements_Group_Collection_Shortened)
                    {
                        AssemblyInstance assembly = null;
                        List<AssemblyInstance> assemblies = new List<AssemblyInstance>();
                        List<Element> all_Elements_Group_Collection_Filtered_Position_With_Assembly = new List<Element>();
                        foreach (Element group_Position_Elements_In_All_Model in all_Elements_Group_Collection)
                        {
                            FamilyInstance parent = (FamilyInstance)group_Position_Elements_In_All_Model;
                            List<FamilyInstance> collection_ElementId_Memberids_All_Grop_In_Model = new List<FamilyInstance>();
                           
                            collection_ElementId_Memberids_All_Grop_In_Model.Add(parent);
                            bool iteration = false;
                            for (int a = 0; a < collection_ElementId_Memberids_All_Grop_In_Model.Count && !iteration; a++)
                            {
                                if (Revit_Document_Assembling_Window.Document.GetElement(collection_ElementId_Memberids_All_Grop_In_Model[a].Id).get_Parameter(BuiltInParameter.ASSEMBLY_NAME) != null)
                                {
                                    AssemblyInstance assemblyInstance = (AssemblyInstance)Revit_Document_Assembling_Window.Document.GetElement(collection_ElementId_Memberids_All_Grop_In_Model[a].AssemblyInstanceId);
                                    Element element_Type = Revit_Document_Assembling_Window.Document.GetElement(assemblyInstance.GetTypeId());
                                    string name1 = group_Position_Filtered.get_Parameter(Data_Assembling_Window.guid_ADSK_Mark).AsValueString();
                                    string name2 = group_Position_Elements_In_All_Model.get_Parameter(Data_Assembling_Window.guid_ADSK_Mark).AsValueString();
                                    string name_Assembly = element_Type.Name.ToString();
                                    if (name1 == name2 && name2 == name_Assembly)
                                    {
                                        assembly = assemblyInstance;
                                    }
                                    if (name1 == name2 && name2 != name_Assembly)
                                    {
                                        assemblies.Add(assemblyInstance);
                                    }
                                    iteration = true;
                                }
                            }
                        }
                        if (assembly != null)
                        {
                            foreach (var assemblyInstance in assemblies)
                            {
                                assemblyInstance.Disassemble();
                            }
                        }
                        
                    }
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message); 
                s_Mistake_String.ShowDialog();
            }
        }
       
    }
  
    public class Window_Assembly_Position
    {
        public Element group { get; set; } = null;
        public AssemblyInstance assemblyInstance { get; set; } = null;
        public Window_Assembly_Position(Element group, AssemblyInstance assemblyInstance)
        {
            this.group = group;
            this.assemblyInstance = assemblyInstance;
        }
    }

}
