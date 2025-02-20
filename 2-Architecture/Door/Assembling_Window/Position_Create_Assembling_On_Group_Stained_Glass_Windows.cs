using Autodesk.Revit.DB;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;


namespace WPFApplication.Assembling_Door
{
    public class Position_Create_Assembling_On_Group_Stained_Glass_Windows
    {
        public void Position_Create_Assembling_View_On_Group_Stained_Glass_Windows()
        {
            using (Transaction newT1 = new Transaction(Revit_Document_Assembling_Window.Document, "Создание видов сборок"))
            {
                newT1.Start();
                foreach (ElementId group_ElementID in Data_Assembling_Window.grup_Filtered_Collection)
                {
                    Element element_Group = Revit_Document_Assembling_Window.Document.GetElement(group_ElementID);
                    View_Elements_On_Group_Stained_Glass_Windows view_Elements_On_Group_Stained_Glass_Windows = new View_Elements_On_Group_Stained_Glass_Windows(element_Group);
                }
                newT1.Commit();
            }
        }
        public void MemberIds_On_Group_Stained_Glass_Windows()
        {
            List<Element> collection_Assembly = new List<Element>();
            if (Data_Assembling_Window.grup_Filtered_Collection.Count > 0)
            {
                Data_Assembling_Window.number_Assembly_Elements = 0;
                using (Transaction trans = new Transaction(Revit_Document_Assembling_Window.Document, "Создание сборок"))
                {
                    try
                    {
                        trans.Start();
                        bool iteration = false;
                        for (int i = 0; i < Data_Assembling_Window.grup_Filtered_Collection.Count; i++)
                        {
                            Element element_Group = Revit_Document_Assembling_Window.Document.GetElement(Data_Assembling_Window.grup_Filtered_Collection[i]);
                            Element element_Type = Revit_Document_Assembling_Window.Document.GetElement(Revit_Document_Assembling_Window.Document.GetElement(Data_Assembling_Window.grup_Filtered_Collection[i]).GetTypeId());


                            string mark_Value = element_Type.get_Parameter(Data_Assembling_Window.guid_ADSK_Mark).AsValueString();
                            List<ElementId> elementIds = new List<ElementId>();
                           
                            if (element_Group.get_Parameter(BuiltInParameter.ASSEMBLY_NAME) == null)
                            {
                                elementIds.Add(element_Group.Id);
                                List<ElementId> IDS = (List<ElementId>)element_Group.GetDependentElements(null);
                                foreach (ElementId elementId in IDS)
                                {
                                    Element element = Revit_Document_Assembling_Window.Document.GetElement(elementId);
                                    Element element_Type_Depended = Revit_Document_Assembling_Window.Document.GetElement(Revit_Document_Assembling_Window.Document.GetElement(elementId).GetTypeId());
                                    if (element.Category.Name.ToString() == "Двери" && element_Type_Depended.Name.Contains("Полотно"))
                                    {
                                        elementIds.Add(elementId);
                                    }
                                }
                            }
                            
                            
                                Data_Assembling_Window.number_Assembly_Elements++;
                            
                            CreateAssemblyExample createAssemblyExample = new CreateAssemblyExample();
                            if (elementIds == null || elementIds.Count == 0 && iteration == false)
                            {
                                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Не выбраны элементы для сборки либо все позиции уже созданы.");
                                s_Mistake_String.ShowDialog();
                                iteration = true;
                            }
                            ICollection<ElementId> elementIds1 = elementIds;
                            string mark_Value1 = mark_Value;
                            if (elementIds != null && elementIds.Count != 0)
                            {
                                collection_Assembly.Add(createAssemblyExample.CreateAssembly(Revit_Document_Assembling_Window.Document, elementIds, mark_Value));
                            }
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. "+ ex.Message);
                        s_Mistake_String.ShowDialog();
                    }
                }
            }
            using (Transaction trans = new Transaction(Revit_Document_Assembling_Window.Document, "Заполнение параметров в сбороках"))
            {
                try
                {
                    trans.Start();
                    foreach (Element element in collection_Assembly)
                    {
                        Element element_Type = Revit_Document_Assembling_Window.Document.GetElement(element.GetTypeId());
                        element_Type.get_Parameter(Data_Assembling_Window.guid_Group).Set(207.00 / 304.8);
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                    s_Mistake_String.ShowDialog();
                }
            }
        }
       
    }
    public class CreateAssemblyExample
    {
        public Element CreateAssembly(Document doc, ICollection<ElementId> elementIds, string assemblyName)
        {
            ElementId category_ID = new ElementId(-2000023);
            ElementId assembly_Instance_Id = new ElementId(0);
            AssemblyInstance assembly = AssemblyInstance.Create(doc, elementIds, category_ID);
            assembly_Instance_Id = assembly.Id;
            return assembly;
        }
    }
    public class RenamedAssemblyExample
    {
        public void RenamedAssembly(Element collection_Group)
        {
            List<ElementId> memberIds = new List<ElementId>();
            memberIds.Add(collection_Group.Id);
            FilteredElementCollector window = new FilteredElementCollector(Revit_Document_Assembling_Window.Document);
            string str_ADSK_Mark = Revit_Document_Assembling_Window.Document.GetElement(collection_Group.GetTypeId()).get_Parameter(Data_Assembling_Window.guid_ADSK_Mark).AsValueString();
            bool position_True = false;
            try
            {
                for (int i = 0; i < memberIds.Count && position_True == false; i++)
                {
                    Element element = Revit_Document_Assembling_Window.Document.GetElement(memberIds[i]);
                    if (element.get_Parameter(BuiltInParameter.ASSEMBLY_NAME) != null)
                    {
                        position_True = true;
                        AssemblyInstance assemblyInstance = (AssemblyInstance)window.OfCategory(BuiltInCategory.OST_Assemblies).ToElements().First(symbol => symbol.Name == element.get_Parameter(BuiltInParameter.ASSEMBLY_NAME).AsValueString());
                        Revit_Document_Assembling_Window.Document.GetElement(assemblyInstance.GetTypeId()).Name = str_ADSK_Mark;
                    }
                }
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }
    }
    public class View_Elements_On_Group_Stained_Glass_Windows
    {
        public View_Elements_On_Group_Stained_Glass_Windows(Element collection_Group)
        {
            FilteredElementCollector window_Position = new FilteredElementCollector(Revit_Document_Assembling_Window.Document);
            IList<Element> all_Elements = window_Position.OfCategory(BuiltInCategory.OST_Assemblies).WhereElementIsNotElementType().ToElements();
            string str_ADSK_Mark = Revit_Document_Assembling_Window.Document.GetElement(collection_Group.GetTypeId()).get_Parameter(Data_Assembling_Window.guid_ADSK_Mark).AsValueString();
            bool position_True = false;
            try
            {
                for (int i = 0; i < all_Elements.Count && position_True == false; i++)
                {
                    Element element_Type = Revit_Document_Assembling_Window.Document.GetElement(all_Elements[i].GetTypeId());
                    Parameter param = element_Type.get_Parameter(BuiltInParameter.SYMBOL_NAME_PARAM);
                    string name1 = param.AsValueString();
                    if (param != null && param.AsValueString() == str_ADSK_Mark && Data_Assembling_Window.AssemblyDetailViewOrientation.Count > 0)
                    {
                        List<Autodesk.Revit.DB.View> views = new List<Autodesk.Revit.DB.View>();
                        FilteredElementCollector collector = new FilteredElementCollector(Revit_Document_Assembling_Window.Document)
                            .OfClass(typeof(Autodesk.Revit.DB.View));
                        List<Autodesk.Revit.DB.View> view_Collection = new List<Autodesk.Revit.DB.View>();
                        foreach (Autodesk.Revit.DB.View view in collector)
                        {
                            if (Revit_Document_Assembling_Window.Document.GetElement(view.AssociatedAssemblyInstanceId) != null)
                            {
                                string name = Revit_Document_Assembling_Window.Document.GetElement(view.AssociatedAssemblyInstanceId).Name.ToString();
                                if (name != null && name == param.AsValueString())
                                {
                                    view_Collection.Add(view);
                                }
                            }
                        }
                        if (view_Collection.Count > 0)
                        {
                            foreach (AssemblyDetailViewOrientation assemblyDetailViewOrientation in Data_Assembling_Window.AssemblyDetailViewOrientation)
                            {
                                AssemblyDetailViewOrientation assemblyDetailViewOrientation1 = new AssemblyDetailViewOrientation();
                                bool position_True_Iteration = false;
                                foreach (Autodesk.Revit.DB.View view in view_Collection)
                                {
                                    AssemblyDetailViewOrientation detail_View = new AssemblyDetailViewOrientation();
                                    string view_Name = view.get_Parameter(BuiltInParameter.VIEW_NAME).AsValueString();
                                    if (view_Name == "Отметка сзади")
                                    {
                                        detail_View = AssemblyDetailViewOrientation.ElevationBack;
                                    }
                                    if (view_Name == "Отметка спереди")
                                    {
                                        detail_View = AssemblyDetailViewOrientation.ElevationFront;
                                    }
                                    if (view_Name == "Отметка слева")
                                    {
                                        detail_View = AssemblyDetailViewOrientation.ElevationLeft;
                                    }
                                    if (view_Name == "Отметка справа")
                                    {
                                        detail_View = AssemblyDetailViewOrientation.ElevationRight;
                                    }
                                    if (view_Name == "План с подробностями")
                                    {
                                        detail_View = AssemblyDetailViewOrientation.HorizontalDetail;
                                    }
                                    if (detail_View == assemblyDetailViewOrientation)
                                    {
                                        Parameter param_Blanks = view.get_Parameter(BuiltInParameter.VIEW_DESCRIPTION);
                                        param_Blanks.Set(str_ADSK_Mark);
                                        assemblyDetailViewOrientation1 = assemblyDetailViewOrientation;
                                        position_True_Iteration = true;
                                    }
                                }
                                if (position_True_Iteration == false)
                                {
                                    AssemblyViewUtils.CreateDetailSection(Revit_Document_Assembling_Window.Document, all_Elements[i].Id, assemblyDetailViewOrientation);
                                }
                            }
                        }
                        else
                        {
                            foreach (AssemblyDetailViewOrientation assemblyDetailViewOrientation in Data_Assembling_Window.AssemblyDetailViewOrientation)
                            {
                                Autodesk.Revit.DB.View view = AssemblyViewUtils.CreateDetailSection(Revit_Document_Assembling_Window.Document, all_Elements[i].Id, assemblyDetailViewOrientation);
                                Parameter param_Blanks = view.get_Parameter(BuiltInParameter.VIEW_DESCRIPTION);
                                param_Blanks.Set(str_ADSK_Mark);
                                view.ViewTemplateId = new FilteredElementCollector(Revit_Document_Assembling_Window.Document).OfClass(typeof(View)).Cast<View>().FirstOrDefault(v => v.IsTemplate && v.Name.Equals("3-А_Фсд_Фасад_ЧБ")).Id;
                            }
                        }
                    }
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
