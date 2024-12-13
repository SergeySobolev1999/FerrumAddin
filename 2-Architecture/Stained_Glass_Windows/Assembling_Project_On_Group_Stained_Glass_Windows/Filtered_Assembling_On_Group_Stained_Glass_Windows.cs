using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace WPFApplication.Assembling_Project_On_Group_Stained_Glass_Windows
{
    public class Filtered_Assembling_On_Group_Stained_Glass_Windows
    {
        public void Filtered_Position_Assembling_On_Group_Stained_Glass_Windows(bool false_Update)
        {
            try
            {
                using (Transaction trans = new Transaction(Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document, "Фильтрация"))
                {
                    trans.Start();
                    Data_Assembling_On_Group_Stained_Glass_Windows.filtered_Group.Clear();
                    FilteredElementCollector window = new FilteredElementCollector(Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document);
                    List<Element> all_Elements_Group = (List<Element>)window.OfCategory(BuiltInCategory.OST_IOSModelGroups).WhereElementIsNotElementType().ToElements();
                    List<Group> all_Elements_Group_Collection = new List<Group>();
                    List<Group> all_Elements_Group_Collection_Shortened = new List<Group>();
                    List<Group> all_Elements_Group_Collection_Last = new List<Group>();
                    foreach (Group element in all_Elements_Group)
                    {
                        Element element_Type = Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(element.GetTypeId());
                        double parameter_Value = element_Type.get_Parameter(Data_Assembling_On_Group_Stained_Glass_Windows.guid_Group).AsDouble() * 304.8;
                        if (211 <= parameter_Value && parameter_Value < 212.999)
                        {
                            all_Elements_Group_Collection.Add(element);
                        }
                    }
                    all_Elements_Group_Collection_Shortened = all_Elements_Group_Collection.GroupBy(p => p.GetTypeId()).Select(g => g.First()).ToList();
                    List<Group> all_Elements_Group_Collection_Filtered_With_Assembly = new List<Group>();
                    List<Group> all_Elements_Group_Collection_Filtered_Dont_Assembly = new List<Group>();
                    foreach (Group group_Position_Filtered in all_Elements_Group_Collection_Shortened)
                    {
                        List<Group> all_Elements_Group_Collection_Filtered_Position_With_Assembly = new List<Group>();
                        foreach (Group group_Position_Elements_In_All_Model in all_Elements_Group_Collection)
                        {
                            List<ElementId> collection_ElementId_All_Grop_In_Model = (List<ElementId>)group_Position_Elements_In_All_Model.GetMemberIds();
                            string id = group_Position_Filtered.Id.ToString();
                            bool iteration = false;
                            for (int a = 0; a < collection_ElementId_All_Grop_In_Model.Count && iteration == false; a++)
                            {

                                string category = Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(collection_ElementId_All_Grop_In_Model[a]).get_Parameter(BuiltInParameter.ELEM_CATEGORY_PARAM).AsValueString();
                                if (category != null)
                                {
                                    if (category == "Импосты витража"
                               || category == "Окна"
                               || category == "Двери"
                               || category == "Обобщенные модели"
                               || category == "Ограждение")
                                    {
                                        Element element_MemberIds = Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(collection_ElementId_All_Grop_In_Model[a]);
                                        string ids = element_MemberIds.Id.ToString();
                                        if (element_MemberIds.get_Parameter(BuiltInParameter.ASSEMBLY_NAME) != null)
                                        {
                                            AssemblyInstance assemblyInstance = (AssemblyInstance)Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(element_MemberIds.AssemblyInstanceId);
                                            Element element_Type = Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(element_MemberIds.GetTypeId());
                                            string name1 = group_Position_Filtered.get_Parameter(Data_Assembling_On_Group_Stained_Glass_Windows.guid_ADSK_Mark).AsValueString();
                                            string name2 = group_Position_Elements_In_All_Model.get_Parameter(Data_Assembling_On_Group_Stained_Glass_Windows.guid_ADSK_Mark).AsValueString();
                                            if (group_Position_Filtered.get_Parameter(Data_Assembling_On_Group_Stained_Glass_Windows.guid_ADSK_Mark).AsValueString() ==
                                                group_Position_Elements_In_All_Model.get_Parameter(Data_Assembling_On_Group_Stained_Glass_Windows.guid_ADSK_Mark).AsValueString())
                                            {
                                                all_Elements_Group_Collection_Filtered_Position_With_Assembly.Add(group_Position_Elements_In_All_Model);
                                                iteration = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        for (int e = 0; e < all_Elements_Group_Collection_Filtered_Position_With_Assembly.Count; e++)
                        {
                            List<ElementId> collection_ElementId_Memberids_All_Grop_In_Model = (List<ElementId>)all_Elements_Group_Collection_Filtered_Position_With_Assembly[e].GetMemberIds();
                            bool iteration = false;
                            for (int a = 0; a < collection_ElementId_Memberids_All_Grop_In_Model.Count && iteration == false; a++)
                            {
                                string category = Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(collection_ElementId_Memberids_All_Grop_In_Model[a]).get_Parameter(BuiltInParameter.ELEM_CATEGORY_PARAM).AsValueString();
                                if (category != null)
                                {
                                    if (category == "Импосты витража"
                               || category == "Окна"
                               || category == "Двери"
                               || category == "Обобщенные модели"
                               || category == "Ограждение")
                                    {
                                        AssemblyInstance assemblyInstance = (AssemblyInstance)Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document
                                            .GetElement(Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(collection_ElementId_Memberids_All_Grop_In_Model[a]).AssemblyInstanceId);
                                        if (assemblyInstance.AllowsAssemblyViewCreation() != false)
                                        {
                                            all_Elements_Group_Collection_Last.Add(all_Elements_Group_Collection_Filtered_Position_With_Assembly[e]);
                                        }
                                        iteration = true;
                                    }
                                }
                            }
                        }
                        if(all_Elements_Group_Collection_Filtered_Position_With_Assembly.Count==0)
                        {
                            all_Elements_Group_Collection_Last.Add(group_Position_Filtered);
                        }
                    }
                    foreach (Group group in all_Elements_Group_Collection_Last)
                    {
                        List<ElementId> collection_ElementId_All_Grop_In_Model = (List<ElementId>)group.GetMemberIds();

                        bool iteration = false;
                        AssemblyInstance assemblyInstance = null;
                        for (int a = 0; a < collection_ElementId_All_Grop_In_Model.Count && iteration == false; a++)
                        {
                            string mark_Group = Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(group.Id).get_Parameter(Data_Assembling_On_Group_Stained_Glass_Windows.guid_ADSK_Mark).AsValueString();
                            string category = Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(collection_ElementId_All_Grop_In_Model[a]).get_Parameter(BuiltInParameter.ELEM_CATEGORY_PARAM).AsValueString();
                            if (category != null)
                            {
                                if (category == "Импосты витража"
                           || category == "Окна"
                           || category == "Двери"
                           || category == "Обобщенные модели"
                           || category == "Ограждение")
                                {
                                    Element element = Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(collection_ElementId_All_Grop_In_Model[a]);
                                    if (Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(element.AssemblyInstanceId) != null)
                                    {
                                        if (Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(element.AssemblyInstanceId).Name.ToString() == mark_Group)
                                        {
                                            assemblyInstance = (AssemblyInstance)Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(element.AssemblyInstanceId);
                                            iteration = true;
                                        }
                                    }
                                }
                            }
                        }
                        string group_Name = group.Name;
                        Group_Assembly_Position group_Assembly_Position = new Group_Assembly_Position(group, assemblyInstance);
                        Data_Assembling_On_Group_Stained_Glass_Windows.filtered_Group.Add(group_Assembly_Position);
                    }
                    Data_Assembling_On_Group_Stained_Glass_Windows.DataItems.Clear();
                    if (Data_Assembling_On_Group_Stained_Glass_Windows.filtered_Group.Count() > 0)
                    {
                        List<DataItem> DataItems_Sort = new List<DataItem>();
                        foreach (Group_Assembly_Position group_Assembly_Position in Data_Assembling_On_Group_Stained_Glass_Windows.filtered_Group)
                        {
                            Element element_Type = Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(group_Assembly_Position.group.GetTypeId());
                            GroupType groupType = group_Assembly_Position.group.GroupType;
                            string mark_Value = group_Assembly_Position.group.get_Parameter(Data_Assembling_On_Group_Stained_Glass_Windows.guid_ADSK_Mark).AsValueString();
                            string actual = "Х";
                            if (group_Assembly_Position.assemblyInstance != null && Revit_Document_Assembling_On_Group_Stained_Glass_Windows
                                .Document.GetElement(group_Assembly_Position.group.Id).get_Parameter(Data_Assembling_On_Group_Stained_Glass_Windows
                                .guid_ADSK_Mark).AsValueString()== group_Assembly_Position.assemblyInstance.Name.ToString())
                                {
                                actual = "✓";
                                }
                            string type_Value = groupType.Name.ToString();
                            string group_ID_Value = group_Assembly_Position.group.Id.ToString();
                            string assembling_ID_Value = "Х";
                            if (group_Assembly_Position.assemblyInstance != null)
                            {
                                assembling_ID_Value = group_Assembly_Position.assemblyInstance.Id.ToString();
                            }
                            bool update = false_Update;
                            DataItem dataItem = new DataItem(mark_Value, type_Value, actual, group_ID_Value, assembling_ID_Value, update);
                            DataItems_Sort.Add(dataItem);
                        }
                        List<DataItem> DataItems_Sort_Last = DataItems_Sort.OrderBy(x => x.Mark).ToList();
                        foreach (DataItem dataItem_Position in DataItems_Sort_Last)
                        {
                            Data_Assembling_On_Group_Stained_Glass_Windows.DataItems.Add(dataItem_Position);
                        }
                    }
                    //foreach(Group group_position in all_Elements_Group_Collection)
                    //{
                    //    List<Group> all_Elements_Group_Collection_Filtered = new List<Group>();
                    //    List<ElementId> collection_ElementId_All_Grop_In_Model = (List<ElementId>)group_position.GetMemberIds();
                    //    bool iteration = false;
                    //    for(int a =0; a< collection_ElementId_All_Grop_In_Model.Count && iteration==false;a++)
                    //    {
                    //        //ICollection<ElementId> collection_ElementId_All_Grop_In_Model1 = collection_ElementId_All_Grop_In_Model;
                    //        Element element_MemberIds = Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(collection_ElementId_All_Grop_In_Model[a]);
                    //        if (element_MemberIds.get_Parameter(BuiltInParameter.ASSEMBLY_NAME) != null)
                    //        {
                    //            AssemblyInstance assemblyInstance = (AssemblyInstance)Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(element_MemberIds.AssemblyInstanceId);
                    //            Element element_Type = Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(element_MemberIds.GetTypeId());
                    //            if (element_MemberIds.get_Parameter(Data_Assembling_On_Group_Stained_Glass_Windows.guid_ADSK_Mark).AsValueString() == assemblyInstance.Name.ToString())
                    //            {
                    //                all_Elements_Group_Collection_Filtered.Add(group_position);
                    //                iteration = true;
                    //            }
                    //        }
                    //    }
                    //    ICollection<ElementId> collection_ElementId_Delete = new List<ElementId>();
                    //    for (int e = 0; e < all_Elements_Group_Collection_Filtered.Count; e++)
                    //    {
                    //        if (e < all_Elements_Group_Collection_Filtered.Count - 1)
                    //        {
                    //            ICollection<ElementId> collection_ElementId_Memberids_All_Grop_In_Model = all_Elements_Group_Collection_Filtered[e].GetMemberIds();
                    //            for (int a = 0; a < collection_ElementId_Memberids_All_Grop_In_Model.Count && group_position.get_Parameter(BuiltInParameter.ASSEMBLY_NAME) != null; a++)
                    //            {
                    //                collection_ElementId_Delete.Add(Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(group_position.Id).Id);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            all_Elements_Group_Collection_Last.Add((Group)all_Elements_Group.Last());
                    //        }
                    //    }
                    //    if(all_Elements_Group_Collection_Filtered.Count==0)
                    //    {
                    //        all_Elements_Group_Collection_Last.Add(group_position);
                    //    }
                    //    Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.Delete(collection_ElementId_Delete);
                    //}
                    //foreach(Group group in all_Elements_Group_Collection_Last)
                    //{
                    //    AssemblyInstance assemblyInstance = (AssemblyInstance)Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(group.AssemblyInstanceId);
                    //    Group_Assembly_Position group_Assembly_Position = new Group_Assembly_Position(group, assemblyInstance);
                    //    Data_Assembling_On_Group_Stained_Glass_Windows.filtered_Group.Add(group_Assembly_Position);
                    //}
                    //TaskDialog.Show("ып", Data_Assembling_On_Group_Stained_Glass_Windows.filtered_Group.Count.ToString()+ "/"+all_Elements_Group_Collection.Count.ToString()+"/"+ all_Elements_Group_Collection_Shortened.Count.ToString());
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }
        public void Assembly_Delete()
        {
            try
            {
                using (Transaction trans = new Transaction(Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document, "Фильтрация"))
                {
                    trans.Start();
                    FilteredElementCollector window = new FilteredElementCollector(Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document);
                    List<Element> all_Elements_Assemblys = (List<Element>)window.OfCategory(BuiltInCategory.OST_Assemblies).WhereElementIsNotElementType().ToElements();
                    foreach (AssemblyInstance element in all_Elements_Assemblys)
                    {
                        if (element.AllowsAssemblyViewCreation() == false)
                        {
                            element.Disassemble();
                        }
                    }
                    FilteredElementCollector window2 = new FilteredElementCollector(Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document);
                    List<Element> all_Elements_Group = (List<Element>)window2.OfCategory(BuiltInCategory.OST_IOSModelGroups).WhereElementIsNotElementType().ToElements();
                    List<Group> all_Elements_Group_Collection = new List<Group>();
                    List<Group> all_Elements_Group_Collection_Shortened = new List<Group>();
                    foreach (Group element in all_Elements_Group)
                    {
                        Element element_Type = Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(element.GetTypeId());
                        double parameter_Value = element_Type.get_Parameter(Data_Assembling_On_Group_Stained_Glass_Windows.guid_Group).AsDouble() * 304.8;
                        if (211 <= parameter_Value && parameter_Value < 212.999)
                        {
                            all_Elements_Group_Collection.Add(element);
                        }
                    }
                    all_Elements_Group_Collection_Shortened = all_Elements_Group_Collection.GroupBy(p => p.GetTypeId()).Select(g => g.First()).ToList();
                    List<Group> all_Elements_Group_Collection_Filtered_With_Assembly = new List<Group>();
                    List<Group> all_Elements_Group_Collection_Filtered_Dont_Assembly = new List<Group>();
                    foreach (Group group_Position_Filtered in all_Elements_Group_Collection_Shortened)
                    {
                        AssemblyInstance assembly = null;
                        List<AssemblyInstance> assemblies = new List<AssemblyInstance>();
                        List<Group> all_Elements_Group_Collection_Filtered_Position_With_Assembly = new List<Group>();
                        foreach (Group group_Position_Elements_In_All_Model in all_Elements_Group_Collection)
                        {
                            List<ElementId> collection_ElementId_Memberids_All_Grop_In_Model = (List<ElementId>)group_Position_Elements_In_All_Model.GetMemberIds();
                            bool iteration = false;
                            for (int a = 0; a < collection_ElementId_Memberids_All_Grop_In_Model.Count && !iteration; a++)
                            {
                                if (Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(collection_ElementId_Memberids_All_Grop_In_Model[a]).get_Parameter(BuiltInParameter.ASSEMBLY_NAME) != null)
                                {
                                    AssemblyInstance assemblyInstance = (AssemblyInstance)Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document
                                        .GetElement(Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(collection_ElementId_Memberids_All_Grop_In_Model[a]).AssemblyInstanceId);
                                    Element element_Type = Revit_Document_Assembling_On_Group_Stained_Glass_Windows.Document.GetElement(assemblyInstance.GetTypeId());
                                    string name1 = group_Position_Filtered.get_Parameter(Data_Assembling_On_Group_Stained_Glass_Windows.guid_ADSK_Mark).AsValueString();
                                    string name2 = group_Position_Elements_In_All_Model.get_Parameter(Data_Assembling_On_Group_Stained_Glass_Windows.guid_ADSK_Mark).AsValueString();
                                    string name_Assembly = element_Type.Name.ToString();
                                    if (name1 == name2&& name2 == name_Assembly)
                                    {
                                        assembly = assemblyInstance;
                                    }
                                    if(name1 == name2 && name2 != name_Assembly)
                                    {
                                        assemblies.Add(assemblyInstance);
                                    }
                                    iteration = true;
                                }
                            }
                        }
                        if(assembly!=null)
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
    public class Group_Assembly_Position
    {
        public Group group { get; set; } = null;
        public AssemblyInstance assemblyInstance { get; set; } = null;
        public Group_Assembly_Position(Group group, AssemblyInstance assemblyInstance)
        {
            this.group = group;
            this.assemblyInstance = assemblyInstance;
        }
    }
}
