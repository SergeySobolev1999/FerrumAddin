using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAPI_Basic_Course.Creating_Specifications;
using SSDK;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Xml.Serialization;
using WPFApplication.SharedParameterManager;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ListViewItem = System.Windows.Forms.ListViewItem;

namespace RevitAPI_Basic_Course.Creating_Specifications
{
    public partial class WPF_Creating_Specifications : System.Windows.Window
    {
        XmlSerializer xml = new XmlSerializer(typeof(Serialize_Data));
        XmlSerializer xml_Set = new XmlSerializer(typeof(Shedule_Save_Elements_List));
        Shedule_Save_Elements_List shedule_Save_Elements_List = new Shedule_Save_Elements_List();
        bool iteration = false;

        public WPF_Creating_Specifications(ExternalCommandData commandData)
        {
           
            Revit_Document_Creating_Specifications.Initialize(commandData);
            InitializeComponent();
            shedules_Elements();
            presets_Save();
            shedule_Set_SelectionChanged_First();
            shedule_Set_SelectionChanged_1(this, null);

        }
        public void Scrol_Viewer1_Creating_View()
        {
            scrollViewer1.Content = Data_Creating_Specifications.list_Filter1;
            scrollViewer2.Content = Data_Creating_Specifications.list_Filter2;
        }
        private void presets_Save()
        {
            string path = Data_Creating_Specifications.Folder_Base_Way + @"\\Presets";
            string path_File_Name = "Serialize_Set_Creating_Specifications.xml";
            string filePath = System.IO.Path.Combine(path, path_File_Name);
            if (File.Exists(filePath))
            {
                Shedule_Set_Download(this, null);
            }
        }
        private void inputTextBoxFilterSheetsValue(object sender, TextChangedEventArgs e)
        {
            if (inputTextBoxFilterSheets.Text != "")
            {
                Data_Creating_Specifications.filter_Text_Sheets = inputTextBoxFilterSheets.Text;
            }
            Data_Creating_Specifications.scrol_Viewer1_Creating_Specifications.Scrol_Viewer1_Creating_Specifications_Main();
            Scrol_Viewer1_Creating_View();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Data_Creating_Specifications.list_Filter1.SelectedItems.Count > 0)
            {
                System.Windows.Controls.ListView listView1 = new System.Windows.Controls.ListView();
                for (int i = 0; i < Data_Creating_Specifications.list_Filter1.Items.Count; i++)
                {
                    int number = 0;
                    for (int j = 0; j < Data_Creating_Specifications.list_Filter1.SelectedItems.Count; j++)
                    {
                        if (Data_Creating_Specifications.list_Filter1.Items[i] == Data_Creating_Specifications.list_Filter1.SelectedItems[j])
                        {
                            number++;
                        }
                    }
                    if (number == 1)
                    {
                        bool filter_Two = true;
                        foreach (string element in Data_Creating_Specifications.list_Filter2.Items)
                        {
                            if (element == Data_Creating_Specifications.list_Filter1.Items[i].ToString() && Data_Creating_Specifications.list_Filter2.Items.Count > 0)
                            {
                                filter_Two = false;
                            }
                        }
                        if (filter_Two == true)
                        {
                            Data_Creating_Specifications.list_Filter2.Items.Add(Data_Creating_Specifications.list_Filter1.Items[i]);
                        }
                    }
                    else
                    {
                        listView1.Items.Add(Data_Creating_Specifications.list_Filter1.Items[i]);
                    }
                }
                Data_Creating_Specifications.list_Filter1 = listView1;
                Scrol_Viewer1_Creating_View();
                Scrol_Viewer1_Creating_Specifications_NUmber();
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Data_Creating_Specifications.list_Filter2.SelectedItems.Count > 0)
            {
                System.Windows.Controls.ListView listView1 = new System.Windows.Controls.ListView();
                for (int i = 0; i < Data_Creating_Specifications.list_Filter2.Items.Count; i++)
                {
                    int number = 0;
                    for (int j = 0; j < Data_Creating_Specifications.list_Filter2.SelectedItems.Count; j++)
                    {
                        if (Data_Creating_Specifications.list_Filter2.Items[i] == Data_Creating_Specifications.list_Filter2.SelectedItems[j])
                        {
                            number++;
                        }
                    }
                    if (number == 1)
                    {
                        Data_Creating_Specifications.list_Filter1.Items.Add(Data_Creating_Specifications.list_Filter2.Items[i]);
                    }
                    else
                    {
                        listView1.Items.Add(Data_Creating_Specifications.list_Filter2.Items[i]);
                    }
                }
                Data_Creating_Specifications.list_Filter2 = listView1;
                Scrol_Viewer1_Creating_View();
                Scrol_Viewer1_Creating_Specifications_NUmber();
            }
        }
        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void scrollViewer2elementdown(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ListView listView2down = Data_Creating_Specifications.list_Filter2;
            for (int i = Data_Creating_Specifications.list_Filter2.Items.Count - 1; i >= 0; i--)
                if (Data_Creating_Specifications.list_Filter2.Items[Data_Creating_Specifications.list_Filter2.Items.Count - 1] != Data_Creating_Specifications.list_Filter2.SelectedItems[Data_Creating_Specifications.list_Filter2.SelectedItems.Count - 1])
                {
                    for (int j = Data_Creating_Specifications.list_Filter2.SelectedItems.Count - 1; j > -1; j--)
                        if (Data_Creating_Specifications.list_Filter2.Items[i] == Data_Creating_Specifications.list_Filter2.SelectedItems[j])
                        {
                            listView2down.Items.Insert(i, listView2down.Items[i + 1]);
                            //listView2down.Items.Insert(i, Data_Creating_Specifications.list_Filter2.Items[i - 1]);
                            listView2down.Items.RemoveAt(i + 2);
                        }
                }
            Data_Creating_Specifications.list_Filter2 = listView2down;
            Scrol_Viewer1_Creating_View();
        }
        private void scrollViewer2elementup(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ListView listView2up = Data_Creating_Specifications.list_Filter2;
            for (int i = 0; i < Data_Creating_Specifications.list_Filter2.Items.Count; i++)
                if (Data_Creating_Specifications.list_Filter2.Items[0] != Data_Creating_Specifications.list_Filter2.SelectedItems[0])
                {
                    for (int j = 0; j < Data_Creating_Specifications.list_Filter2.SelectedItems.Count; j++)
                        if (Data_Creating_Specifications.list_Filter2.Items[i] == Data_Creating_Specifications.list_Filter2.SelectedItems[j])
                        {
                            listView2up.Items.Insert(i + 1, listView2up.Items[i - 1]);
                            //listView2down.Items.Insert(i, Data_Creating_Specifications.list_Filter2.Items[i - 1]);
                            listView2up.Items.RemoveAt(i - 1);
                        }
                }
            Data_Creating_Specifications.list_Filter2 = listView2up;
            Scrol_Viewer1_Creating_View();
        }
        public void Scrol_Viewer1_Creating_Specifications_NUmber()
        {
            //if (shedule_Set.SelectedItem.ToString() == "Без набора")
            //{
            //    outputTextBoxFilter2Sheets.Text = ("Всего: 0");
            //}
            //else
            //{
                outputTextBoxFilter2Sheets.Text = ("Всего: " + Data_Creating_Specifications.list_Filter2.Items.Count.ToString());
            //}
        }
        private void inputNumberSheetsFirstValue(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, e.Text.Length - 1);
        }
        private void sizeSegmentsValue(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, e.Text.Length - 1);
        }
      
        private void sizeCapValue(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, e.Text.Length - 1);
        }
        private void shedules_Elements()
        {
            Shedule_Elements_Creating_Specifications shedule_Elements_Ctreating_Specification = new Shedule_Elements_Creating_Specifications();
            shedule_Elements_Ctreating_Specification.Shedule_Elements_Creating_Specifications_List();
            foreach (Element element in Data_Creating_Specifications.shedule_Elements)
            {
                if (element.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString().Count() > 0)
                {

                }
                else
                {
                    string name_Family_Elements = element.get_Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM).AsValueString();
                    shedule_Elements.Items.Add("Семейство:" + name_Family_Elements + ", Тип:" + element.Name.ToString());
                    Data_Creating_Specifications.shedule_Elements_Result = ("Семейство:" + name_Family_Elements + ", Тип:" + element.Name.ToString());
                         shedule_Elements_Next.Items.Add("Семейство:" + name_Family_Elements + ", Тип:" + element.Name.ToString());
                    Data_Creating_Specifications.shedule_Elements_Result_Next = ("Семейство:" + name_Family_Elements + ", Тип:" + element.Name.ToString());
                }
            }
        }
        private void Serialize_Data_Discharge_Click(object sender, RoutedEventArgs e)
        {
            using (Transaction newT1 = new Transaction(Revit_Document_Creating_Specifications.Document, "Выгрузка данных формата "))
            {
                newT1.Start();
                try
                {
                    if (inputNumberSheetsFirst.Text.ToString() == "")
                    {
                        inputNumberSheetsFirst.Text = "0";
                    }
                    if (inputNameSheets.Text.ToString() == "")
                    {
                        inputNameSheets.Text = "Лист";
                    }
                    if (sizeSegments.Text.ToString() == "")
                    {
                        sizeSegments.Text = "230";
                    }
                    if (sizeSegments_Next.Text.ToString() == "")
                    {
                        sizeSegments_Next.Text = "230";
                    }
                    if (shedule_Elements.Text.ToString() == "")
                    {
                        shedule_Elements.SelectedItem = Data_Creating_Specifications.shedule_Elements_Result.ToString();
                    }
                    if (shedule_Elements_Next.Text.ToString() == "")
                    {
                        shedule_Elements_Next.SelectedItem = Data_Creating_Specifications.shedule_Elements_Result_Next.ToString();
                    }
                    Serialize_Data serialize_Creating_Specifications_Information = new Serialize_Data(shedule_Elements.Text
                        , shedule_Elements_Next.Text,Int32.Parse(inputNumberSheetsFirst.Text), inputNameSheets.Text, chekFormat9.IsChecked.Value, Int32
                        .Parse(sizeSegments.Text), Int32.Parse(sizeSegments_Next.Text), chekFormatLayersOne.IsChecked.Value);

                    SerializeXML(serialize_Creating_Specifications_Information);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                newT1.Commit();
            }
        }
        //        using (FileStream fs = new FileStream(@"%AppData %\Autodesk\Revit\Addins\2024\Splugin\Serialize_Creating_Specifications.xml", FileMode.Create))
        //C:\Users\Сергей Соболев\Downloads\WPFApplication-8a01b768bdfac45b7303112b8479860be1039cb1(1)\WPFApplication-8a01b768bdfac45b7303112b8479860be1039cb1\SharedParameterManager
        private void SerializeXML(Serialize_Data serialize_Creating_Specifications)
        {
            using (FileStream fs = new FileStream((Data_Creating_Specifications.Folder_Presets + @"\Serialize_Creating_Specifications.xml"), FileMode.Create))
            {
                xml.Serialize(fs, serialize_Creating_Specifications);
            }
           
        }
             //using (FileStream fs2 = new FileStream(@"%AppData %\Autodesk\Revit\Addins\2024\Splugin\Serialize_Set_Creating_Specifications.xml", FileMode.Create))
        private void SerializeXML2(Shedule_Save_Elements_List serialize_Set_Creating_Specifications)
        {
            using (FileStream fs2 = new FileStream(Data_Creating_Specifications.Folder_Presets +@"\Serialize_Set_Creating_Specifications.xml", FileMode.Create))
            {
                
                xml_Set.Serialize(fs2, serialize_Set_Creating_Specifications);
            }
        }
                   //using (FileStream fs = new FileStream("Serialize_Creating_Specifications.xml", FileMode.OpenOrCreate))
        private Serialize_Data DeserializeXML()
        {
            using (FileStream fs = new FileStream((Data_Creating_Specifications.Folder_Presets + @"\Serialize_Creating_Specifications.xml"), FileMode.OpenOrCreate))
            {
                return (Serialize_Data)xml.Deserialize(fs);
            }
        }
               //using (FileStream fs = new FileStream("Serialize_Set_Creating_Specifications.xml", FileMode.OpenOrCreate))
        private Shedule_Save_Elements_List DeserializeXMLSet()
        {
            using (FileStream fs = new FileStream((Data_Creating_Specifications.Folder_Presets + @"\Serialize_Set_Creating_Specifications.xml"), FileMode.OpenOrCreate))
            {
                return (Shedule_Save_Elements_List)xml_Set.Deserialize(fs);
            }
        }
        private void Serialize_Download_Data_Click(object sender, RoutedEventArgs e)
        {
            using (Transaction newT2 = new Transaction(Revit_Document_Creating_Specifications.Document, "Выгрузка данных формата "))
            {
                newT2.Start();
                try
                {
                    Serialize_Data serializw_Information = DeserializeXML();
                    inputNumberSheetsFirst.Text = serializw_Information.number_Sheet_First.ToString();
                    inputNameSheets.Text = serializw_Information.sheet_Name;
                    chekFormat9.IsChecked = serializw_Information.sheet9;
                    chekFormatLayersOne.IsChecked = serializw_Information.first_Sheet_Unique;
                    shedule_Elements.SelectedItem = serializw_Information.First_List.ToString();
                    shedule_Elements_Next.SelectedItem = serializw_Information.Next_List.ToString();
                    sizeSegments.Text = serializw_Information.segment_Size.ToString();
                    sizeSegments_Next.Text = serializw_Information.sizeSegments_Next.ToString();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                newT2.Commit();
            }
        }
        private void Shedule_Set_Сonfirmation(object sender, RoutedEventArgs e)
        {
            Set_WPF_Creating_Specifications set_WPF_Creating_Specifications = new Set_WPF_Creating_Specifications();
            set_WPF_Creating_Specifications.ShowDialog();
            shedule_Set_SelectionChanged();
            Scrol_Viewer1_Creating_Specifications_NUmber();
        }
        private void shedule_Set_SelectionChanged_First()
        {
            Shedule_Save_Elements shedule_Save_Elements_One_Position = new Shedule_Save_Elements("Без набора", null);
            shedule_Save_Elements_List.shedule_Save_Elements_List.Add(shedule_Save_Elements_One_Position);
            Shedule_Save_Elements_List_Combo();
        }
        private void shedule_Set_SelectionChanged()
        {
            if (Data_Creating_Specifications.name_Save_Element_Positon != "")
            {
                List<string> elem = new List<string>();
                foreach (string element in Data_Creating_Specifications.list_Filter2.Items)
                {
                    elem.Add(element.ToString());
                }
                Shedule_Save_Elements shedule_Save_Elements_One_Position_And = new Shedule_Save_Elements(Data_Creating_Specifications.name_Save_Element_Positon, elem);
                shedule_Save_Elements_List.shedule_Save_Elements_List.Add(shedule_Save_Elements_One_Position_And);
                Shedule_Save_Elements_List_Combo();
            }
        }
        private void Serialize_Set_Data_Discharge_Click(string Name, List<string> Elements)
        {
            using (Transaction newT2 = new Transaction(Revit_Document_Creating_Specifications.Document, "Выгрузка данных формата "))
            {
                newT2.Start();
                try
                {
                    Shedule_Save_Elements serialize_Creating_Specifications_Information2 = new Shedule_Save_Elements(Name, Elements);

                    shedule_Save_Elements_List.shedule_Save_Elements_List.Add(serialize_Creating_Specifications_Information2);

                    SerializeXML2(shedule_Save_Elements_List);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                newT2.Commit();
            }
        }
        private void Shedule_Save_Elements_List_Combo()
        {
            if (iteration == false)
            {
                shedule_Set.SelectedItem = shedule_Save_Elements_List.shedule_Save_Elements_List.Last().Name;
                shedule_Set.Items.Add(shedule_Save_Elements_List.shedule_Save_Elements_List.Last().Name);
                iteration = true;
            }
            if (shedule_Save_Elements_List.shedule_Save_Elements_List.Last().Name != "Без набора" && iteration != false)
            {
                shedule_Set.Items.Add(shedule_Save_Elements_List.shedule_Save_Elements_List.Last().Name);
                SerializeXML2(shedule_Save_Elements_List);
            }
            
        }
        private void shedule_Set_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            using (Transaction newT2 = new Transaction(Revit_Document_Creating_Specifications.Document, "Выгрузка данных формата "))
            {
                newT2.Start();
                if (shedule_Set.SelectedItem != null)
                {
                    if (shedule_Set.SelectedItem.ToString() == "Без набора" && shedule_Set.SelectedItem.ToString().Count() > 0 && shedule_Set.SelectedItem.ToString() != "")
                    {
                        Data_Creating_Specifications.scrol_Viewer1_Creating_Specifications.Scrol_Viewer1_Creating_Specifications_Main();
                        Data_Creating_Specifications.list_Filter2.Items.Clear();
                        Scrol_Viewer1_Creating_View();
                    }
                    if (shedule_Set.SelectedItem.ToString() != "Без набора" && shedule_Set.SelectedItem.ToString().Count() > 0 && shedule_Set.SelectedItem.ToString() != "")
                    {
                        try
                        {
                            bool first_Elements = false;
                            foreach (Shedule_Save_Elements shedule_Save_Elements in shedule_Save_Elements_List.shedule_Save_Elements_List)
                            {
                                if (shedule_Save_Elements.Name != "Без набора" && shedule_Save_Elements.Name == shedule_Set.SelectedItem.ToString())
                                {
                                    if (first_Elements == false)
                                    {
                                        Data_Creating_Specifications.list_Filter2.Items.Clear();
                                    }
                                    string a = shedule_Save_Elements.Name;
                                    foreach (string elements in shedule_Save_Elements.shedule_Elements_Collection_Save)
                                    {
                                        bool filter_True = false;
                                        foreach (string element in Data_Creating_Specifications.list_Filter_Model)
                                        {
                                            if (element == elements)
                                            {
                                                filter_True = true;
                                            }
                                        }
                                        if (filter_True==true)
                                        {
                                            Data_Creating_Specifications.list_Filter2.Items.Add(elements);
                                        }
                                    }
                                    first_Elements = true;
                                }
                            }
                            Data_Creating_Specifications.list_Filter1.Items.Clear();
                            int number = Data_Creating_Specifications.list_Filter_Model.Count;
                            for (int i = 0; i < number; i++)
                            {
                                {
                                    bool filter_True = false;
                                    for (int j = 0; j < Data_Creating_Specifications.list_Filter2.Items.Count; j++)
                                    {
                                        if (Data_Creating_Specifications.list_Filter_Model[i] == Data_Creating_Specifications.list_Filter2.Items[j].ToString())
                                        {
                                            filter_True = true;
                                        }
                                    }
                                    if (filter_True == false)
                                    {
                                        Data_Creating_Specifications.list_Filter1.Items.Add(Data_Creating_Specifications.list_Filter_Model[i]);
                                    }
                                }
                            }
                            Scrol_Viewer1_Creating_View();
                           
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message);
                        }
                        
                    }
                    newT2.Commit();
                    Scrol_Viewer1_Creating_Specifications_NUmber();
                }
            }
        }

        private void Shedule_Set_Download(object sender, RoutedEventArgs e)
        {
            Shedule_Save_Elements_List shedule_Save_Elements_List_R = DeserializeXMLSet();
            Shedule_Save_Elements_List shedule_Save_Elements_List_Save = new Shedule_Save_Elements_List();
            List<Shedule_Save_Elements> list = new List<Shedule_Save_Elements>();
            foreach (Shedule_Save_Elements shedule_Save_Elements in shedule_Save_Elements_List_R.shedule_Save_Elements_List)
            {
                if (shedule_Save_Elements.Name != "Без набора")
                {
                    Shedule_Save_Elements serialize_Creating_Specifications_Information2 = new Shedule_Save_Elements(shedule_Save_Elements.Name, shedule_Save_Elements.shedule_Elements_Collection_Save);
                    list.Add(serialize_Creating_Specifications_Information2);
                }
            }
            shedule_Save_Elements_List.shedule_Save_Elements_List.Clear();
            shedule_Save_Elements_List.shedule_Save_Elements_List = list;
            foreach (Shedule_Save_Elements name in shedule_Save_Elements_List.shedule_Save_Elements_List)
            {
                shedule_Set.Items.Add(name.Name);
            }
            Scrol_Viewer1_Creating_View();
            Scrol_Viewer1_Creating_Specifications_NUmber();
        }
        private void Shedule_Delete(object sender, RoutedEventArgs e)
        {
            Shedule_Save_Elements_List shedule_Save_Elements_List_R = shedule_Save_Elements_List;
            List<Shedule_Save_Elements> list2 = new List<Shedule_Save_Elements>();
            foreach (Shedule_Save_Elements shedule_Save_Elements in shedule_Save_Elements_List_R.shedule_Save_Elements_List)
            {
                if (shedule_Save_Elements.Name.ToString() != "Без набора")
                {
                    Shedule_Save_Elements serialize_Creating_Specifications_Information2 = new Shedule_Save_Elements(shedule_Save_Elements.Name, shedule_Save_Elements.shedule_Elements_Collection_Save);
                    list2.Add(serialize_Creating_Specifications_Information2);
                }
            }
            shedule_Save_Elements_List.shedule_Save_Elements_List = list2;
            //shedule_Set.Items.Clear();
            if (shedule_Set.SelectedItem.ToString() != "Без набора")
            {
                shedule_Set.Items.Remove(shedule_Set.SelectedItem);
            }
            shedule_Set.SelectedItem = shedule_Save_Elements_List.shedule_Save_Elements_List.First().Name.ToString();
            Scrol_Viewer1_Creating_Specifications_NUmber();


        }
        private void List_Duplicate()
        {
            ICollection <ViewSheet> elements_Sheets_Duplicate = null;
            ICollection<Element> scrol_Viewer1_Creating_Specifications = (ICollection<Element>)new FilteredElementCollector(Revit_Document_Creating_Specifications.Document).OfCategory(BuiltInCategory.OST_Sheets);
            foreach(ViewSheet elemnts in scrol_Viewer1_Creating_Specifications)
            {
                if(elemnts.SheetNumber.Contains("‏‏‎ ‎"))
                {
                    elements_Sheets_Duplicate.Add(elemnts);
                }
            }
            if(elements_Sheets_Duplicate != null)
            {

            }
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void Close_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public bool Wpf_Position_Chek()
        {
            try
            {
                bool chek = true;
                string not_Valid_Position = "";
                Autodesk.Revit.DB.View activeView = Revit_Document_Creating_Specifications.Document.ActiveView;
                ICollection<ViewSheet> elements_Sheets_Duplicate = new List<ViewSheet>();
                
                //var uniqueItems = Data_Creating_Specifications.list_Filter2.Items
                //             .Cast<ListViewItem>()      // Преобразуем в IEnumerable<ListViewItem>
                //             .GroupBy(item => item.Text) // Группируем по тексту элемента
                //             .Select(group => group.First()) // Выбираем первый элемент из каждой группы
                //             .ToList();
                List<string> items_Shedule_Start = new List<string>();
                for (int i = 0; i< Data_Creating_Specifications.list_Filter2.Items.Count;i++)
                {
                    items_Shedule_Start.Add(Data_Creating_Specifications.list_Filter2.Items[i].ToString());
                }
                List<string> items_Shedule = items_Shedule_Start.Distinct().ToList();
                //foreach(string elem in Data_Creating_Specifications.list_Filter2.Items)
                //{
                //    if (items_Shedule.Count > 0)
                //    {
                //        for (int i = 0; i < items_Shedule.Count;i++)
                //        {
                //            if (items_Shedule[i]==elem)
                //            {
                //                items_Shedule.Add(elem);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        items_Shedule.Add(elem);
                //    }
                //}
                List<string> items_Sheets_Start = new List<string>();
                FilteredElementCollector Sheets = new FilteredElementCollector(Revit_Document_Creating_Specifications.Document).OfCategory(BuiltInCategory.OST_Sheets);
                foreach(ViewSheet sheet in Sheets) 
                {
                    if (sheet.SheetNumber.Contains("‏‏‎ ‎"))
                    {
                        elements_Sheets_Duplicate.Add(sheet);
                    }
                }
                bool view_Active_View = false;
                foreach(Element elemnts in elements_Sheets_Duplicate)
                {
                    if (activeView != null && activeView.Id == elemnts.Id)
                    {
                        view_Active_View = true;
                    }
                }
                if (view_Active_View)
                {
                    chek = false;
                    not_Valid_Position = not_Valid_Position + "\n -активный вид Revit подлежит удалению, необходимо перейти на другой вид";
                }
                if (Data_Creating_Specifications.list_Filter2.Items.Count != items_Shedule.Count)
                {
                    chek = false;
                    not_Valid_Position = not_Valid_Position + "\n -в списке спецификаций для обработки есть дубликаты, необходимо удалить лишние экземпляры";
                }
                if (Data_Creating_Specifications.list_Filter2.Items.Count < 1)
                {
                    chek = false;
                    not_Valid_Position = not_Valid_Position + "\n -набор спецификаций для обработки пуст";
                }
                if (inputNumberSheetsFirst.Text.ToString() == "")
                {
                    chek = false;
                    not_Valid_Position = not_Valid_Position + "\n -начальный номер спецификации не указан";
                }
                if (inputNameSheets.Text.ToString() == "")
                {
                    chek = false;
                    not_Valid_Position = not_Valid_Position + "\n -наименование листов не указано";
                }
                if (sizeSegments.Text.ToString() == "")
                {
                    chek = false;
                    not_Valid_Position = not_Valid_Position + "\n -размер сегмента первого листа не указан";
                }
                if (sizeSegments.Text.ToString() != "")
                {
                    if (Int32.Parse(sizeSegments.Text.ToString()) < 100)
                    {
                        chek = false;
                        not_Valid_Position = not_Valid_Position + "\n -размер сегмента первого листа меньше 100";
                    }
                }
                if (sizeSegments_Next.Text.ToString() == "")
                {
                    chek = false;
                    not_Valid_Position = not_Valid_Position + "\n -размер сегмента второго листа не указан";
                }
                if (sizeSegments_Next.Text.ToString() != "")
                {
                    if (Int32.Parse(sizeSegments_Next.Text.ToString()) < 100 )
                    {
                        chek = false;
                        not_Valid_Position = not_Valid_Position + "\n -размер сегмента второго листа меньше 100";
                    }
                }
                if (shedule_Elements.Text.ToString() == "")
                {
                    chek = false;
                    not_Valid_Position = not_Valid_Position + "\n -типоразмер первого листа не выбран";
                }
                if (shedule_Elements_Next.Text.ToString() == "")
                {
                    chek = false;
                    not_Valid_Position = not_Valid_Position + "\n -типоразмер второго листа не выбран";
                }
                if (chek == false)
                {
                    S_Mistake_String s_Mistake_String = new S_Mistake_String("Процесс запуска прерван по следующим ошибкам: " + not_Valid_Position);
                    s_Mistake_String.ShowDialog();
                }
                return chek;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //TaskDialog.Show("вав", "Кнопка активна");
                Data_Creating_Specifications.sheets_Collection.Clear();
                Data_Creating_Specifications.list_Elements.Clear();
                Data_Creating_Specifications.start_Size = 0;
                Data_Creating_Specifications.list_Filter_Bool = true;
                var viewModel = new SharedParameterManagerViewModel();
                var view = new SharedParameterManagerWindow(viewModel);
              
                if (Wpf_Position_Chek())
                {
                    //TaskDialog.Show("вав", "Чекер пройден");
                    if (Data_Creating_Specifications.sheet_Collection_Filtered.Count > 0 && Data_Creating_Specifications.sheet_Collection_Filtered != null)
                    {
                        view.ShowDialog();
                    }
                    if (Data_Creating_Specifications.list_Filter_Bool == true)
                    {
                        if (Data_Creating_Specifications.list_Filter2.Items.Count > 0 && Data_Creating_Specifications.list_Filter2.Items != null)
                        {
                            Segment_Shedule_Manager_Creating_Specifications segment_Shedule_Manager_Creating_Specifications1 = new Segment_Shedule_Manager_Creating_Specifications();
                        }
                        if (Data_Creating_Specifications.segment_Filter_Bool == true)
                        {
                            //Segment_Shedule_Manager_Creating_Specifications segment_Shedule_Manager_Creating_Specifications = new Segment_Shedule_Manager_Creating_Specifications();
                            Data_Creating_Specifications.shedule_Nume = shedule_Elements.Text.ToString();
                            Data_Creating_Specifications.segments_Height = Double.Parse(sizeSegments.Text.ToString()) ;
                            Data_Creating_Specifications.segments_Height_Next = Double.Parse(sizeSegments_Next.Text.ToString()) ;
                            Data_Creating_Specifications.start_Number = Int32.Parse(inputNumberSheetsFirst.Text.ToString());
                            Data_Creating_Specifications.start_Nume = inputNameSheets.Text.ToString();
                            Data_Creating_Specifications.shedule_Nume = shedule_Elements.SelectedValue.ToString();
                            Data_Creating_Specifications.shedule_Nume_Next = shedule_Elements_Next.SelectedValue.ToString();
                            Data_Creating_Specifications.chekFormatLayersOne = (bool)chekFormatLayersOne.IsChecked;
                            if (chekFormat9.IsChecked.Value)
                            {
                                Data_Creating_Specifications.sheet_9_Size = 32;
                            }
                            Segments_Creating_Specifications segments_Creating_Specifications = new Segments_Creating_Specifications();
                            //TaskDialog.Show("вав", "До активации основного алгоритма дошли");
                            segments_Creating_Specifications.Segments_Creating_Specifications_Main();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                // Прокручиваем `ScrollViewer`, учитывая направление движения колесика
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta / 3.0);
                e.Handled = true; // Отмечаем событие как обработанное
            }
        }
    }
}

