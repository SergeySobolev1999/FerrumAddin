using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace RevitAPI_Basic_Course.Creating_Specifications
{
    public class Scrol_Viewer1_Creating_Specifications
    {
        public void Scrol_Viewer1_Creating_Specifications_Main()
        {
            try
            {
                var scrol_Viewer1_Creating_Specifications_Lines = new StackPanel();
            ICollection<Element> scrol_Viewer1_Creating_Specifications = new FilteredElementCollector(Revit_Document_Creating_Specifications.Document).OfCategory(BuiltInCategory.OST_Schedules).WhereElementIsNotElementType().ToElements();
            ICollection<Element> scrol_Viewer1_Creating_Specifications_filter = new List<Element>();
            foreach (Element element in scrol_Viewer1_Creating_Specifications)
            {
                if (element.Name.Contains("Ведомость изменений") == false && element.Name.Contains("2-АСт") == false && element.Name.Contains("2-А_Обм Стены") == false && element.Name.Contains("ADSK_") == false)
                {
                    scrol_Viewer1_Creating_Specifications_filter.Add(element);
                }
            }
            Data_Creating_Specifications.sheets_Filter = scrol_Viewer1_Creating_Specifications_filter;
            ScrollViewer scrollViewer = new ScrollViewer();
            ListView listView = new ListView();
            ListView listView2 = new ListView();
            foreach (Element element in scrol_Viewer1_Creating_Specifications_filter)
            {
                if (element.Name.Contains(Data_Creating_Specifications.filter_Text_Sheets) && Data_Creating_Specifications.filter_Text_Sheets != "" && Data_Creating_Specifications.filter_Text_Sheets != null)
                {
                    listView.Items.Add(element.Name.ToString());
                }
            }
            foreach (Element element in scrol_Viewer1_Creating_Specifications_filter)
            {
                listView2.Items.Add(element.Name.ToString());
            }
            if (listView.Items.Count > 0)
            {
                Data_Creating_Specifications.list_Filter1 = listView;
            }
            else
            {
                Data_Creating_Specifications.list_Filter1 = listView2;
            }
            if (Main_Creating_Specifications.iteration_Export == false)
            {
                Data_Creating_Specifications.list_Filter_Model.Clear();

                for (int i = 0; i < Data_Creating_Specifications.list_Filter1.Items.Count; i++)
                {
                    Data_Creating_Specifications.list_Filter_Model.Add(Data_Creating_Specifications.list_Filter1.Items[i].ToString());
                }
                Main_Creating_Specifications.iteration_Export = true;

            }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
       
    }
}
