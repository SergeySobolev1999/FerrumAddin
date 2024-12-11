using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAPI_Basic_Course.Creating_Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WPFApplication.SharedParameterManager
{
    public class Segment_Shedule_Manager_Creating_Specifications
    {
        public Segment_Shedule_Manager_Creating_Specifications()
        {
            try
            {
                ICollection<ViewSchedule> shedule_Equil = new List<ViewSchedule>();
                ICollection<Element> shedule_Elements = new FilteredElementCollector(Revit_Document_Creating_Specifications.Document).OfCategory(BuiltInCategory.OST_Schedules).WhereElementIsNotElementType().ToElements();
            if (shedule_Elements != null)
            {
                foreach (ViewSchedule element in shedule_Elements)
                {
                    bool true_elements = false;
                    int a = element.GetSegmentCount();
                    if (a > 1)
                    {

                        for (int i = 0; i < Data_Creating_Specifications.list_Filter2.Items.Count; i++)
                        {
                            int b = element.GetSegmentCount();
                            if (b > 1)
                            {

                                if (element.Name.ToString() == Data_Creating_Specifications.list_Filter2.Items[i].ToString())
                                {
                                    true_elements = true;
                                }
                            }
                            //TaskDialog.Show("вы", element.Name + "/" + Data_Creating_Specifications.list_Filter2.Items[i].ToString() + "/" + true_elements.ToString());
                        }

                    }

                    if (true_elements == true)
                    {
                        
                        shedule_Equil.Add(element);
                    }

                }
            }
            if(shedule_Equil.Count>0)
            {
                    int a = 0;
                Data_Creating_Specifications.shedule_Equil = shedule_Equil;
                    using (Transaction newT = new Transaction(Revit_Document_Creating_Specifications.Document, "Удаление сегментов: "))
                    {
                        newT.Start();
                        try
                        {
                            foreach (ViewSchedule element in Data_Creating_Specifications.shedule_Equil)
                            {
                                if (element.GetSegmentCount() > 1)
                                {
                                    for (int i = element.GetSegmentCount() - 1; i >= 1; i--)
                                    {

                                        element.DeleteSegment(0);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message);
                        }
                        newT.Commit();
                    }
                    Data_Creating_Specifications.shedule_Equil.Clear();
                    //Segment_Shedule_Manager_WPF_Creating_Specifications segment_Shedule_Manager_WPF_Creating_Specifications = new Segment_Shedule_Manager_WPF_Creating_Specifications();
                    //segment_Shedule_Manager_WPF_Creating_Specifications.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}
