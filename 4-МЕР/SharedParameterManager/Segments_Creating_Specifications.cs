using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using Autodesk.Revit.ApplicationServices;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using View = Autodesk.Revit.DB.View;
using SSDK;

namespace RevitAPI_Basic_Course.Creating_Specifications
{
    public class Segments_Creating_Specifications
    {
        public void Segments_Creating_Specifications_Main()
        {
            Data_Creating_Specifications.sheet_number = 0;
            Data_Creating_Specifications.shedule_Number = 0;
            try
            {
                for (int i = 0; i < Data_Creating_Specifications.list_Filter2.Items.Count; i++)
                {
                    string shedule_Name = Data_Creating_Specifications.list_Filter2.Items[i].ToString();
                    Segments_Creating_SpecificationsSheet(shedule_Name);
                    Data_Creating_Specifications.shedule_Number++;
                }
                ReNumber_Segments_Creating_SpecificationsSheet();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        public void Segments_Creating_SpecificationsSheet(string shedule_Name)
        {
            try
            {
                using (Transaction newT = new Transaction(Revit_Document_Creating_Specifications.Document, "Генерация спецификации: " + shedule_Name))
                {
                    newT.Start();
                    string[] shedule_Name_min = Data_Creating_Specifications.shedule_Nume.ToString().Split(new[] { "Тип:" }, StringSplitOptions.None);
                    string[] shedule_Name_Next_min = Data_Creating_Specifications.shedule_Nume_Next.ToString().Split(new[] { "Тип:" }, StringSplitOptions.None);
                    Data_Creating_Specifications.shedule_Name_Min_Last = shedule_Name_min[shedule_Name_min.Count() - 1];
                    Data_Creating_Specifications.collector = new FilteredElementCollector(Revit_Document_Creating_Specifications.Document);
                    Data_Creating_Specifications.titleBlock = Data_Creating_Specifications.collector.OfCategory(BuiltInCategory.OST_TitleBlocks).ToElements().First(symbol => symbol.Name == Data_Creating_Specifications.shedule_Name_Min_Last);
                    Data_Creating_Specifications.bbx_Frame = Data_Creating_Specifications.titleBlock.get_BoundingBox((Autodesk.Revit.DB.View)Revit_Document_Creating_Specifications
                        .Document.GetElement(Data_Creating_Specifications.titleBlock.OwnerViewId));
                    Data_Creating_Specifications.titleBlockid = Data_Creating_Specifications.titleBlock.Id;
                    Data_Creating_Specifications.height_FrameX = (Data_Creating_Specifications.bbx_Frame.Max.X - Data_Creating_Specifications.bbx_Frame.Min.X);
                    Data_Creating_Specifications.height_FrameY = (Data_Creating_Specifications.bbx_Frame.Max.Y - Data_Creating_Specifications.bbx_Frame.Min.Y);
                    //Получаем информация о спецификации
                    ViewSchedule schedules = (ViewSchedule)new FilteredElementCollector(Revit_Document_Creating_Specifications.Document)
                        .OfCategory(BuiltInCategory.OST_Schedules).WhereElementIsNotElementType().First(symbol => symbol.Name == shedule_Name);
                    Data_Creating_Specifications.cap_Height = schedules.GetScheduleHeightsOnSheet().TitleHeight * 304.8 + schedules
                        .GetScheduleHeightsOnSheet().ColumnHeaderHeight * 304.8;
                    //Получаем информация о размерах спецификации
                    ViewSheet sheet = ViewSheet.Create(Data_Creating_Specifications.titleBlock.Document, Data_Creating_Specifications.titleBlockid);
                    XYZ point = new XYZ(-Data_Creating_Specifications.height_FrameX, Data_Creating_Specifications.height_FrameY, 0);
                    ScheduleSheetInstance scheduleSheetInstance = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications
                        .Document, sheet.Id, schedules.Id, point);
                    BoundingBoxXYZ bbx = scheduleSheetInstance.get_BoundingBox((Autodesk.Revit.DB.View)Revit_Document_Creating_Specifications
                        .Document.GetElement(scheduleSheetInstance.OwnerViewId));
                    double height = (bbx.Max.Y - bbx.Min.Y) * 304.8 - 2.1162489764333;
                    Revit_Document_Creating_Specifications.Document.Delete(sheet.Id);
                    bool segment_Number_All = false;
                    int iteration_Number = 0;
                    bool iteration_Boost = false;

                    for (int i = 0; height > 0; i++)
                    {
                        bool iteration_Position = false;
                        if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
                        {
                            Sheet_Next();
                        }
                        //Старт на первом листе. Спека не во всю высоту листа. Спека стартуем с угла. Итоговый фрагмент
                        if (height < Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size &&
                            Data_Creating_Specifications.sheet_number == 0 &&
                            Data_Creating_Specifications.shedule_Number == 0 &&
                            segment_Number_All == false &&
                            iteration_Position == false)
                        {
                            ViewSheet sheet_Next1 = ViewSheet.Create(Data_Creating_Specifications.titleBlock.Document, Data_Creating_Specifications.titleBlockid);
                            Data_Creating_Specifications.sheets_Collection.Add(sheet_Next1);
                            XYZ point1 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - Data_Creating_Specifications.sheet_9_Size / 304.8 - 5 / 304.8, 0);
                            ScheduleSheetInstance scheduleSheetInstanceNext = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications
                                .Document, sheet_Next1.Id, schedules.Id, point1);
                            Data_Creating_Specifications.lastlist = sheet_Next1;
                            Data_Creating_Specifications.start_Size = height;
                            height = 0;
                            iteration_Position = true;
                        }
                        //Старт на следущем листе. Спека не во всю высоту листа. Спека стартуем со смещением. Итоговый фрагмент
                        if (height > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size - Data_Creating_Specifications.start_Size - 40 &&
                            height < Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size - 40 &&
                            Data_Creating_Specifications.cap_Height+20 > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.start_Size - Data_Creating_Specifications.sheet_9_Size - 40 &&
                            Data_Creating_Specifications.shedule_Number > 0 &&
                            segment_Number_All == false &&
                            iteration_Position == false)
                        {
                            Data_Creating_Specifications.start_Size = 0;
                            Data_Creating_Specifications.sheet_number++;
                            if (Data_Creating_Specifications.chekFormatLayersOne == true)
                            {
                                Sheet_Next();
                            }
                            ViewSheet sheet_Next1 = ViewSheet.Create(Data_Creating_Specifications.titleBlock.Document, Data_Creating_Specifications.titleBlockid);
                            Data_Creating_Specifications.sheets_Collection.Add(sheet_Next1);
                            Data_Creating_Specifications.lastlist = sheet_Next1;
                            XYZ point2 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - Data_Creating_Specifications.sheet_9_Size / 304.8 - 5 / 304.8, 0);
                            ScheduleSheetInstance scheduleSheetInstanceNext = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications.Document,
                                Data_Creating_Specifications.lastlist.Id, schedules.Id, point2);
                            Data_Creating_Specifications.start_Size = Data_Creating_Specifications.start_Size + height;
                            height = 0;
                            iteration_Position = true;
                        }
                        //Старт на любом листе. Спека не во всю высоту листа. Спека стартуем со смещением. Итоговый фрагмент
                        if (height + Data_Creating_Specifications.start_Size < Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size &&
                            Data_Creating_Specifications.shedule_Number > 0 &&
                            segment_Number_All == false &&
                            iteration_Position == false)
                        {
                            //if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
                            //{
                            //    Sheet_Next();
                            //}
                            XYZ point2 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - 5 / 304.8 - Data_Creating_Specifications.sheet_9_Size / 304.8 
                                - Data_Creating_Specifications.start_Size / 304.8 , 0);
                            ScheduleSheetInstance scheduleSheetInstanceNext = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications.Document,
                                Data_Creating_Specifications.lastlist.Id, schedules.Id, point2);
                            Data_Creating_Specifications.start_Size = Data_Creating_Specifications.start_Size + height;
                            height = 0;
                            iteration_Position = true;
                        }
                        //Старт на следующем листе ввиду малого размера. Спека не во всю высоту листа. Спека стартуем с угла. Итоговый фрагмент
                        if (height + Data_Creating_Specifications.start_Size > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size &&
                            Data_Creating_Specifications.cap_Height > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.start_Size - Data_Creating_Specifications.sheet_9_Size &&
                            height < Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size &&
                            segment_Number_All == false &&
                            iteration_Position == false)
                        {
                            //if (Data_Creating_Specifications.chekFormatLayersOne == true)
                            //{
                            //    Sheet_Next();
                            //}
                            Data_Creating_Specifications.sheet_number++;
                            if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
                            {
                                Sheet_Next();
                            }
                            ViewSheet sheet_Next3 = ViewSheet.Create(Data_Creating_Specifications.titleBlock.Document, Data_Creating_Specifications.titleBlockid);
                            Data_Creating_Specifications.sheets_Collection.Add(sheet_Next3);
                            XYZ point3 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - 5 / 304.8 - Data_Creating_Specifications.sheet_9_Size / 304.8, 0);
                            ScheduleSheetInstance scheduleSheetInstanceNext = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications.Document,
                                sheet_Next3.Id, schedules.Id, point3);
                            Data_Creating_Specifications.lastlist = sheet_Next3;
                            Data_Creating_Specifications.start_Size = height;
                            height = 0;
                            iteration_Position = true;
                        }

                        //Старт на текущем листе. Спека стартуем со смещением. Массив начало
                        if (height + Data_Creating_Specifications.start_Size > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size &&
                           Data_Creating_Specifications.cap_Height < Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.start_Size - Data_Creating_Specifications.sheet_9_Size - 40 &&
                           Data_Creating_Specifications.shedule_Number != 0 &&
                            segment_Number_All == false &&
                            iteration_Position == false)
                        {
                            IList<double> first_Segment = new List<double>();
                            double size = 0;
                            if (height + Data_Creating_Specifications.start_Size - 40 < Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size)
                            {
                                size = 40;
                            }
                            first_Segment.Add((Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.start_Size - Data_Creating_Specifications.sheet_9_Size - size ) / 304.8);
                            schedules.Split(first_Segment);
                            XYZ point4 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - 5 / 304.8 - Data_Creating_Specifications.sheet_9_Size / 304.8 
                                - Data_Creating_Specifications.start_Size / 304.8 , 0);
                            ScheduleSheetInstance scheduleSheetInstanceNext3 = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications
                                .Document, Data_Creating_Specifications.lastlist.Id, schedules.Id, point4, 0);
                            height = Segment_Height(schedules);
                            Data_Creating_Specifications.start_Size = 0;
                            segment_Number_All = true;
                            iteration_Position = true;
                        }
                        //Старт на следующем листе. Спека стартуем со смещением. Массив начало
                        if (height + Data_Creating_Specifications.start_Size > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size &&
                           Data_Creating_Specifications.cap_Height > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.start_Size - Data_Creating_Specifications.sheet_9_Size - 40 &&
                           Data_Creating_Specifications.shedule_Number != 0 &&
                            segment_Number_All == false &&
                            iteration_Position == false)
                        {
                            Data_Creating_Specifications.sheet_number++;
                            if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
                            {
                                Sheet_Next();
                            }
                            Data_Creating_Specifications.start_Size = 0;
                            IList<double> first_Segment = new List<double>();
                            //double size = 0;
                            //if (height + Data_Creating_Specifications.start_Size - 40 < Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size)
                            //{
                            //    size = 40;
                            //}
                            first_Segment.Add((Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.start_Size - Data_Creating_Specifications.sheet_9_Size ) / 304.8);
                            schedules.Split(first_Segment);
                            ViewSheet sheet_Next3 = ViewSheet.Create(Data_Creating_Specifications.titleBlock.Document, Data_Creating_Specifications.titleBlockid);
                            Data_Creating_Specifications.sheets_Collection.Add(sheet_Next3);
                            Data_Creating_Specifications.lastlist = sheet_Next3;
                            XYZ point4 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - Data_Creating_Specifications.sheet_9_Size / 304.8 - 5 / 304.8, 0);
                            ScheduleSheetInstance scheduleSheetInstanceNext3 = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications
                                .Document, Data_Creating_Specifications.lastlist.Id, schedules.Id, point4, 0);
                            height = Segment_Height(schedules);
                            Data_Creating_Specifications.start_Size = 0;
                            segment_Number_All = true;
                            iteration_Position = true;
                        }
                        if (height + Data_Creating_Specifications.start_Size > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size - 40 &&
                          Data_Creating_Specifications.cap_Height < Data_Creating_Specifications.segments_Height -
                          Data_Creating_Specifications.start_Size - Data_Creating_Specifications.sheet_9_Size - 40 &&
                           segment_Number_All == false &&
                            iteration_Position == false)
                        {
                            IList<double> first_Segment = new List<double>();
                            double size = 0;
                            if (height + Data_Creating_Specifications.start_Size - 40 < Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size)
                            {
                                size = 40;
                            }
                            first_Segment.Add((Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.start_Size - Data_Creating_Specifications.sheet_9_Size - size ) / 304.8);
                            schedules.Split(first_Segment);
                            ViewSheet sheet_Next3 = ViewSheet.Create(Data_Creating_Specifications.titleBlock.Document, Data_Creating_Specifications.titleBlockid);
                            Data_Creating_Specifications.sheet_number++;
                            Data_Creating_Specifications.sheets_Collection.Add(sheet_Next3);
                            Data_Creating_Specifications.lastlist = sheet_Next3;
                            XYZ point4 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - 5 / 304.8 - Data_Creating_Specifications.sheet_9_Size / 304.8 
                                - Data_Creating_Specifications.start_Size / 304.8, 0);
                            ScheduleSheetInstance scheduleSheetInstanceNext3 = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications
                                .Document, Data_Creating_Specifications.lastlist.Id, schedules.Id, point4, 0);
                            height = Segment_Height(schedules);
                            Data_Creating_Specifications.start_Size = 0;
                            segment_Number_All = true;
                            iteration_Position = true;
                        }
                        //Старт на текущем листе. Спека неограниченной длины. Спека стартуем с угла. Массив неограниченный. Ускоритель
                        if (height > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size &&
                            height > (Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size) * 3 &&
                          Data_Creating_Specifications.cap_Height < Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size -
                          Data_Creating_Specifications.start_Size &&
                          Data_Creating_Specifications.sheet_number != 0 &&
                           segment_Number_All == true &&
                           iteration_Boost == false && 
                            iteration_Position == false)
                        {
                            IList<double> first_Segment = new List<double>();
                            if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
                            {
                                Sheet_Next();
                            }
                            for (int j = 0; height > (Data_Creating_Specifications.segments_Height * 3); j++)
                            {
                                Data_Creating_Specifications.sheet_number++;
                                first_Segment.Add((Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size ) / 304.8);
                                height = height - (Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size);
                            }
                            schedules.SplitSegment(schedules.GetSegmentCount() - 1, first_Segment);
                            for (int j = 0; j < first_Segment.Count; j++)
                            {
                                ViewSheet sheet_Next3 = ViewSheet.Create(Data_Creating_Specifications.titleBlock.Document, Data_Creating_Specifications.titleBlockid);
                                Data_Creating_Specifications.sheets_Collection.Add(sheet_Next3);
                                XYZ point4 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - Data_Creating_Specifications.sheet_9_Size / 304.8 - 5 / 304.8, 0);
                                ScheduleSheetInstance scheduleSheetInstanceNext3 = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications
                                    .Document, sheet_Next3.Id, schedules.Id, point4, j + 1);
                                segment_Number_All = true;
                                iteration_Position = true;
                            }
                            iteration_Boost = true;
                        }
                        //Старт на текущем листе. Спека неограниченной длины. Спека стартуем с угла. Массив неограниченный. База
                        if (height > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size &&
                          Data_Creating_Specifications.cap_Height < Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size -
                          Data_Creating_Specifications.start_Size &&
                           segment_Number_All == true &&
                            iteration_Position == false)
                        {
                            Data_Creating_Specifications.sheet_number++;
                            if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
                            {
                                Sheet_Next();
                            }
                            IList<double> first_Segment = new List<double>();
                            first_Segment.Add((Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.start_Size - Data_Creating_Specifications.sheet_9_Size) / 304.8);
                            schedules.SplitSegment(schedules.GetSegmentCount() - 1, first_Segment);
                            ViewSheet sheet_Next3 = ViewSheet.Create(Data_Creating_Specifications.titleBlock.Document, Data_Creating_Specifications.titleBlockid);
                            Data_Creating_Specifications.sheets_Collection.Add(sheet_Next3);
                            XYZ point4 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - Data_Creating_Specifications.sheet_9_Size / 304.8 - 5 / 304.8 , 0);
                            ScheduleSheetInstance scheduleSheetInstanceNext3 = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications
                                .Document, sheet_Next3.Id, schedules.Id, point4, schedules.GetSegmentCount() - 2);
                            height = Segment_Height(schedules) + 2.1162489764333 / 304.8;
                            segment_Number_All = true;
                            iteration_Position = true;
                        }

                        //Старт на cледующем листе. Завершение спеки. Спека стартуем с угла. Массив завершен
                        if (height < Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size &&
                          Data_Creating_Specifications.cap_Height < Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size -
                          Data_Creating_Specifications.start_Size &&
                           segment_Number_All == true &&
                            iteration_Position == false)
                        {
                            Data_Creating_Specifications.sheet_number++;
                            if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
                            {
                                Sheet_Next();
                            }
                            ViewSheet sheet_Next3 = ViewSheet.Create(Data_Creating_Specifications.titleBlock.Document, Data_Creating_Specifications.titleBlockid);
                            Data_Creating_Specifications.sheets_Collection.Add(sheet_Next3);
                            XYZ point4 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - Data_Creating_Specifications.sheet_9_Size / 304.8 - 5 / 304.8 , 0);
                            ScheduleSheetInstance scheduleSheetInstanceNext3 = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications
                                .Document, sheet_Next3.Id, schedules.Id, point4, schedules.GetSegmentCount() - 1);
                            height = Segment_Height(schedules);
                            Data_Creating_Specifications.start_Size = height;
                            //TaskDialog.Show("вып", schedules.Name.ToString() +"/"+ height.ToString());
                            segment_Number_All = false;
                            Data_Creating_Specifications.lastlist = sheet_Next3;
                            height = 0;
                            iteration_Position = true;
                        }

                        iteration_Number++;
                        if (iteration_Number == 500)
                        {
                            S_Mistake_String s_Mistake_String = new S_Mistake_String("Обнаружена зацыкленность. Будет выполнен принудительный выход из него");
                            s_Mistake_String.ShowDialog();
                            //TaskDialog.Show("GG", "Data_Creating_Specifications.start_Size" + Data_Creating_Specifications.start_Size.ToString()
                            //    + "\n"
                            //    + "height" + height.ToString().ToString()
                            //     + "\n"
                            //    + "Data_Creating_Specifications.segments_Height" + Data_Creating_Specifications.segments_Height.ToString()
                            //    + "\n"
                            //    + "Data_Creating_Specifications.sheet_9_Size" + Data_Creating_Specifications.sheet_9_Size.ToString()
                            //    + "\n"
                            //    + "Data_Creating_Specifications.cap_Height" + Data_Creating_Specifications.cap_Height.ToString()
                            //    + "\n"
                            //    + "Data_Creating_Specifications.segments_Height" + Data_Creating_Specifications.segments_Height.ToString()
                            //    + "\n"
                            //    + "Data_Creating_Specifications.shedule_Number" + Data_Creating_Specifications.shedule_Number.ToString()
                            //    + "\n"
                            //    + "segment_Number_All" + segment_Number_All.ToString());

                            //TaskDialog.Show("GG", "Data_Creating_Specifications.segments_Height" + Data_Creating_Specifications.segments_Height.ToString());
                            //TaskDialog.Show("GG", "Data_Creating_Specifications.sheet_9_Size" + Data_Creating_Specifications.sheet_9_Size.ToString());
                            //TaskDialog.Show("GG", "Data_Creating_Specifications.cap_Height" + Data_Creating_Specifications.cap_Height.ToString());
                            //TaskDialog.Show("GG", "Data_Creating_Specifications.segments_Height" + Data_Creating_Specifications.segments_Height.ToString());
                            //TaskDialog.Show("GG", "Data_Creating_Specifications.shedule_Number" + Data_Creating_Specifications.shedule_Number.ToString());
                            //TaskDialog.Show("GG", "segment_Number_All " + segment_Number_All.ToString());
                            height = 0;
                        }
                        ////Старт на текущем листе. Спека не во всю высоту листа. Спека стартуем со смещением. Массив из двух элементов
                        //if (height + Data_Creating_Specifications.start_Size > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size && 
                        //    enter == false &&
                        //    Data_Creating_Specifications.cap_Height < Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.start_Size && 
                        //    height < Data_Creating_Specifications.segments_Height)
                        //{
                        //    enter = true;
                        //}
                        ////Старт на текущем листе. Спека во всю высоту листа. Спека стартуем со смещением. Массив неограниченный
                        //if (height - Data_Creating_Specifications.cap_Height + Data_Creating_Specifications.start_Size > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size && 
                        //    Data_Creating_Specifications.start_Size > 0 && 
                        //    enter == false && 
                        //    Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.start_Size > Data_Creating_Specifications.cap_Height)
                        //{
                        //    enter = true;
                        //}
                        ////Старт на следующем листе. Спека во всю высоту листа. Спека стартуем с угла. Массив неограниченный
                        //if (height - Data_Creating_Specifications.cap_Height + Data_Creating_Specifications.start_Size > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size &&
                        //    Data_Creating_Specifications.start_Size > 0 && 
                        //    enter == false && 
                        //    Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.start_Size < Data_Creating_Specifications.cap_Height)
                        //{
                        //    enter = true;
                        //}
                        ////Старт на текущем листе. Спека во всю высоту листа. Спека стартуем с угла. Массив неограниченный
                        //if (height - Data_Creating_Specifications.cap_Height + Data_Creating_Specifications.start_Size > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size &&
                        //    Data_Creating_Specifications.start_Size == 0 && 
                        //    enter == false)
                        //{
                        //    enter = true;
                        //}
                    }



                    //Получаем информация о спецификации
                    //ViewSchedule schedules = (ViewSchedule)new FilteredElementCollector(Revit_Document_Creating_Specifications.Document)
                    //    .OfCategory(BuiltInCategory.OST_Schedules).WhereElementIsNotElementType().First(symbol => symbol.Name == shedule_Name);
                    //Data_Creating_Specifications.cap_Height = schedules.GetScheduleHeightsOnSheet().TitleHeight * 304.8 + schedules
                    //    .GetScheduleHeightsOnSheet().ColumnHeaderHeight * 304.8;
                    ////Получаем информация о размерах спецификации
                    //ViewSheet sheet = ViewSheet.Create(Data_Creating_Specifications.titleBlock.Document, Data_Creating_Specifications.titleBlockid);
                    //XYZ point = new XYZ(-Data_Creating_Specifications.height_FrameX, Data_Creating_Specifications.height_FrameY, 0);
                    //ScheduleSheetInstance scheduleSheetInstance = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications
                    //    .Document, sheet.Id, schedules.Id, point);
                    //BoundingBoxXYZ bbx = scheduleSheetInstance.get_BoundingBox((Autodesk.Revit.DB.View)Revit_Document_Creating_Specifications
                    //    .Document.GetElement(scheduleSheetInstance.OwnerViewId));
                    //double height = (bbx.Max.Y - bbx.Min.Y) * 304.8 - 2.1162489764333;
                    //Revit_Document_Creating_Specifications.Document.Delete(sheet.Id);
                    //Основная логика генерацииb







                    //bool enter = false;
                    ////Старт на первом листе. Спека не во всю высоту листа. Спека стартуем с угла. Итоговый фрагмент
                    //if (height - Data_Creating_Specifications.cap_Height < Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size && 
                    //    Data_Creating_Specifications.sheet_number == 0 && 
                    //    Data_Creating_Specifications.shedule_Number == 0 && 
                    //    enter == false)
                    //{
                    //    ViewSheet sheet_Next1 = ViewSheet.Create(Data_Creating_Specifications.titleBlock.Document, Data_Creating_Specifications.titleBlockid);
                    //    Data_Creating_Specifications.sheets_Collection.Add(sheet_Next1);
                    //    XYZ point1 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - Data_Creating_Specifications.sheet_9_Size / 304.8 - 5 / 304.8, 0);
                    //    ScheduleSheetInstance scheduleSheetInstanceNext = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications
                    //        .Document, sheet_Next1.Id, schedules.Id, point1);
                    //    Data_Creating_Specifications.lastlist = sheet_Next1;
                    //    Data_Creating_Specifications.shedule_Number++;
                    //    Data_Creating_Specifications.start_Size = Data_Creating_Specifications.start_Size + height;
                    //    enter = true;
                    //}
                    ////Старт на первом листе. Спека не во всю высоту листа. Спека стартуем со смещением. Итоговый фрагмент
                    //if (height + Data_Creating_Specifications.start_Size < Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size && 
                    //    Data_Creating_Specifications.shedule_Number > 0 && 
                    //    enter == false)
                    //{
                    //    if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
                    //    {
                    //        Sheet_Next();
                    //    }
                    //    XYZ point2 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - 5 / 304.8 - Data_Creating_Specifications.sheet_9_Size / 304.8 - Data_Creating_Specifications.start_Size / 304.8, 0);
                    //    ScheduleSheetInstance scheduleSheetInstanceNext = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications.Document,
                    //        Data_Creating_Specifications.lastlist.Id, schedules.Id, point2);
                    //    Data_Creating_Specifications.shedule_Number++;
                    //    Data_Creating_Specifications.start_Size = Data_Creating_Specifications.start_Size + height;
                    //    enter = true;
                    //}
                    ////Старт на следующем листе ввиду малого размера. Спека не во всю высоту листа. Спека стартуем с угла. Итоговый фрагмент
                    //if (height + Data_Creating_Specifications.start_Size > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size && 
                    //    enter == false &&
                    //    Data_Creating_Specifications.cap_Height > Data_Creating_Specifications.segments_Height -Data_Creating_Specifications.start_Size && height < Data_Creating_Specifications.segments_Height)
                    //{
                    //    enter = true;
                    //}
                    ////Старт на текущем листе. Спека не во всю высоту листа. Спека стартуем со смещением. Массив из двух элементов
                    //if (height + Data_Creating_Specifications.start_Size > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size && 
                    //    enter == false &&
                    //    Data_Creating_Specifications.cap_Height < Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.start_Size && 
                    //    height < Data_Creating_Specifications.segments_Height)
                    //{
                    //    enter = true;
                    //}
                    ////Старт на текущем листе. Спека во всю высоту листа. Спека стартуем со смещением. Массив неограниченный
                    //if (height - Data_Creating_Specifications.cap_Height + Data_Creating_Specifications.start_Size > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size && 
                    //    Data_Creating_Specifications.start_Size > 0 && 
                    //    enter == false && 
                    //    Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.start_Size > Data_Creating_Specifications.cap_Height)
                    //{
                    //    enter = true;
                    //}
                    ////Старт на следующем листе. Спека во всю высоту листа. Спека стартуем с угла. Массив неограниченный
                    //if (height - Data_Creating_Specifications.cap_Height + Data_Creating_Specifications.start_Size > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size &&
                    //    Data_Creating_Specifications.start_Size > 0 && 
                    //    enter == false && 
                    //    Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.start_Size < Data_Creating_Specifications.cap_Height)
                    //{
                    //    enter = true;
                    //}
                    ////Старт на текущем листе. Спека во всю высоту листа. Спека стартуем с угла. Массив неограниченный
                    //if (height - Data_Creating_Specifications.cap_Height + Data_Creating_Specifications.start_Size > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size &&
                    //    Data_Creating_Specifications.start_Size == 0 && 
                    //    enter == false)
                    //{
                    //    enter = true;
                    //}



                    ////Получаем информация о спецификации
                    //ViewSchedule schedules = (ViewSchedule)new FilteredElementCollector(Revit_Document_Creating_Specifications.Document)
                    //    .OfCategory(BuiltInCategory.OST_Schedules).WhereElementIsNotElementType().First(symbol => symbol.Name == shedule_Name);
                    //Data_Creating_Specifications.cap_Height = schedules.GetScheduleHeightsOnSheet().TitleHeight * 304.8 + schedules
                    //    .GetScheduleHeightsOnSheet().ColumnHeaderHeight * 304.8;
                    ////Получаем информация о размерах спецификации
                    //ViewSheet sheet = ViewSheet.Create(Data_Creating_Specifications.titleBlock.Document, Data_Creating_Specifications.titleBlockid);
                    //XYZ point = new XYZ(-Data_Creating_Specifications.height_FrameX, Data_Creating_Specifications.height_FrameY, 0);
                    //ScheduleSheetInstance scheduleSheetInstance = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications
                    //    .Document, sheet.Id, schedules.Id, point);
                    //BoundingBoxXYZ bbx = scheduleSheetInstance.get_BoundingBox((Autodesk.Revit.DB.View)Revit_Document_Creating_Specifications
                    //    .Document.GetElement(scheduleSheetInstance.OwnerViewId));
                    //double height = (bbx.Max.Y - bbx.Min.Y) * 304.8 - 2.1162489764333;
                    //Revit_Document_Creating_Specifications.Document.Delete(sheet.Id);
                    ////Основная логика генерацииb
                    //bool enter = false;
                    //if (height - Data_Creating_Specifications.cap_Height + Data_Creating_Specifications.start_Size < Data_Creating_Specifications.segments_Height- Data_Creating_Specifications.sheet_9_Size && Data_Creating_Specifications
                    //    .sheet_number == 0 && Data_Creating_Specifications.shedule_Number == 0 && enter == false)
                    //{

                    //    ViewSheet sheet_Next1 = ViewSheet.Create(Data_Creating_Specifications.titleBlock.Document, Data_Creating_Specifications.titleBlockid);

                    //    if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
                    //    {
                    //        Sheet_Next();
                    //    }
                    //    sheet_Next1.Name = Data_Creating_Specifications.start_Nume;
                    //    Data_Creating_Specifications.sheets_Collection.Add(sheet_Next1);
                    //    XYZ point1 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - Data_Creating_Specifications.sheet_9_Size/ 304.8 - 5 / 304.8, 0);
                    //    ScheduleSheetInstance scheduleSheetInstanceNext = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications
                    //        .Document, sheet_Next1.Id, schedules.Id, point1);
                    //    Data_Creating_Specifications.lastlist = sheet_Next1;
                    //    Data_Creating_Specifications.shedule_Number++;
                    //    //Data_Creating_Specifications.sheet_number++;
                    //    Data_Creating_Specifications.start_Size = Data_Creating_Specifications.start_Size + height;
                    //    enter = true;
                    //}
                    //if (height + Data_Creating_Specifications.start_Size < Data_Creating_Specifications.segments_Height-Data_Creating_Specifications.sheet_9_Size && Data_Creating_Specifications
                    //    .shedule_Number > 0 && enter == false)
                    //{
                    //    if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
                    //    {
                    //        Sheet_Next();
                    //    }
                    //    XYZ point2 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - 5 / 304.8- Data_Creating_Specifications.sheet_9_Size / 304.8 - Data_Creating_Specifications.start_Size / 304.8, 0);
                    //    ScheduleSheetInstance scheduleSheetInstanceNext = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications.Document,
                    //        Data_Creating_Specifications.lastlist.Id, schedules.Id, point2);
                    //    Data_Creating_Specifications.shedule_Number++;
                    //    Data_Creating_Specifications.start_Size = Data_Creating_Specifications.start_Size + height;
                    //    enter = true;
                    //}
                    //if (height + Data_Creating_Specifications.start_Size > Data_Creating_Specifications.segments_Height- Data_Creating_Specifications.sheet_9_Size && enter == false &&
                    //    Data_Creating_Specifications.cap_Height > Data_Creating_Specifications.segments_Height -
                    //    Data_Creating_Specifications.start_Size && height < Data_Creating_Specifications.segments_Height)
                    //{

                    //    ViewSheet sheet_Next3 = ViewSheet.Create(Data_Creating_Specifications.titleBlock.Document, Data_Creating_Specifications.titleBlockid);
                    //    Data_Creating_Specifications.sheet_number++;
                    //    if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
                    //    {
                    //        Sheet_Next();
                    //    }
                    //    sheet_Next3.Name = Data_Creating_Specifications.start_Nume;
                    //    Data_Creating_Specifications.sheets_Collection.Add(sheet_Next3);
                    //    XYZ point3 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - 5 / 304.8- Data_Creating_Specifications.sheet_9_Size / 304.8, 0);
                    //    ScheduleSheetInstance scheduleSheetInstanceNext = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications.Document,
                    //        sheet_Next3.Id, schedules.Id, point3);
                    //    Data_Creating_Specifications.lastlist = sheet_Next3;
                    //    Data_Creating_Specifications.shedule_Number++;
                    //    //Data_Creating_Specifications.sheet_number++;
                    //    Data_Creating_Specifications.start_Size = height;
                    //    enter = true;
                    //}
                    //if (height + Data_Creating_Specifications.start_Size > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size && enter == false &&
                    //   Data_Creating_Specifications.cap_Height < Data_Creating_Specifications.segments_Height -
                    //   Data_Creating_Specifications.start_Size && height < Data_Creating_Specifications.segments_Height)
                    //{
                    //    if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
                    //    {
                    //        Sheet_Next();
                    //    }
                    //    IList<double> first_Segment = new List<double>();
                    //    double segment_Split_Heiht = Data_Creating_Specifications.start_Size;
                    //    first_Segment.Add((Data_Creating_Specifications.segments_Height - segment_Split_Heiht) / 304.8);
                    //    schedules.Split(first_Segment);
                    //    XYZ point4 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - 5 / 304.8- Data_Creating_Specifications.sheet_9_Size / 304.8 - Data_Creating_Specifications.start_Size / 304.8, 0);
                    //    ScheduleSheetInstance scheduleSheetInstanceNext3 = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications
                    //        .Document, Data_Creating_Specifications.lastlist.Id, schedules.Id, point4, 0);
                    //    Data_Creating_Specifications.sheet_number++;
                    //    ViewSheet sheet_Next4 = ViewSheet.Create(Data_Creating_Specifications.titleBlock.Document, Data_Creating_Specifications.titleBlockid);


                    //    sheet_Next4.Name = Data_Creating_Specifications.start_Nume;
                    //    Data_Creating_Specifications.sheets_Collection.Add(sheet_Next4);
                    //    XYZ point5 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - Data_Creating_Specifications.sheet_9_Size / 304.8 - 5 / 304.8, 0);
                    //    ScheduleSheetInstance scheduleSheetInstanceNext5 = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications.Document,
                    //        sheet_Next4.Id, schedules.Id, point5, 1);
                    //    BoundingBoxXYZ bbx7 = scheduleSheetInstanceNext5.get_BoundingBox((Autodesk.Revit.DB.View)Revit_Document_Creating_Specifications
                    //                   .Document.GetElement(scheduleSheetInstanceNext5.OwnerViewId));
                    //    Data_Creating_Specifications.lastlist = sheet_Next4;
                    //    Data_Creating_Specifications.shedule_Number++;
                    //    //Data_Creating_Specifications.sheet_number++;
                    //    Data_Creating_Specifications.start_Size = (bbx7.Max.Y - bbx7.Min.Y) * 304.8 - 2.1162489764333;
                    //    enter = true;
                    //}
                    //if (height - Data_Creating_Specifications.cap_Height + Data_Creating_Specifications.start_Size > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size
                    //    && Data_Creating_Specifications.start_Size > 0 && enter == false && Data_Creating_Specifications.segments_Height - Data_Creating_Specifications
                    //    .start_Size > Data_Creating_Specifications.cap_Height)
                    //{
                    //    if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
                    //    {
                    //        Sheet_Next();
                    //    }
                    //    IList<double> first_Segment = new List<double>();
                    //    double segment_Split_Heiht = Data_Creating_Specifications.start_Size;

                    //    for (int i = 0; Data_Creating_Specifications.segments_Height  < height - Data_Creating_Specifications.cap_Height- segment_Split_Heiht- 32; i++)
                    //    {
                    //        if (segment_Split_Heiht > 0)
                    //        {
                    //            first_Segment.Add((Data_Creating_Specifications.segments_Height -Data_Creating_Specifications.sheet_9_Size  - segment_Split_Heiht) / 304.8);
                    //            height = height - (Data_Creating_Specifications.segments_Height -Data_Creating_Specifications.sheet_9_Size  - segment_Split_Heiht - Data_Creating_Specifications.cap_Height);
                    //            segment_Split_Heiht = 0;
                    //        }
                    //        else
                    //        {
                    //            first_Segment.Add(Data_Creating_Specifications.segments_Height/304.8- Data_Creating_Specifications.sheet_9_Size /304.8 );
                    //            height = height - (Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size  - Data_Creating_Specifications.cap_Height);
                    //        }
                    //    }
                    //    schedules.Split(first_Segment);
                    //    for (int i = 0; i < schedules.GetSegmentCount(); i++)
                    //    {
                    //        if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
                    //        {
                    //            Sheet_Next();
                    //        }
                    //        if (i == 0)
                    //        {
                    //            XYZ point6 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - 5 / 304.8- Data_Creating_Specifications.sheet_9_Size / 304.8 - Data_Creating_Specifications.start_Size / 304.8, 0);
                    //            ScheduleSheetInstance scheduleSheetInstanceNext6 = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications
                    //                .Document, Data_Creating_Specifications.lastlist.Id, schedules.Id, point6, i);
                    //        }
                    //        if (i > 0)
                    //        {
                    //            Data_Creating_Specifications.sheet_number++;
                    //            ViewSheet sheet_Next7 = ViewSheet.Create(Data_Creating_Specifications.titleBlock.Document, Data_Creating_Specifications.titleBlockid);

                    //            if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
                    //            {
                    //                Sheet_Next();
                    //            }
                    //            sheet_Next7.Name = Data_Creating_Specifications.start_Nume;
                    //            Data_Creating_Specifications.sheets_Collection.Add(sheet_Next7);
                    //            XYZ point7 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - Data_Creating_Specifications.sheet_9_Size / 304.8 - 5 / 304.8, 0);
                    //            ScheduleSheetInstance scheduleSheetInstanceNext7 = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications
                    //                .Document, sheet_Next7.Id, schedules.Id, point7, i);
                    //            Data_Creating_Specifications.lastlist = sheet_Next7;
                    //            if (i == schedules.GetSegmentCount() - 1)
                    //            {
                    //                BoundingBoxXYZ bbx7 = scheduleSheetInstanceNext7.get_BoundingBox((Autodesk.Revit.DB.View)Revit_Document_Creating_Specifications
                    //                    .Document.GetElement(scheduleSheetInstanceNext7.OwnerViewId));
                    //                Data_Creating_Specifications.start_Size = (bbx7.Max.Y - bbx7.Min.Y) * 304.8 - 2.1162489764333;
                    //            }

                    //        }
                    //    }
                    //    //Data_Creating_Specifications.shedule_Number++;
                    //    enter = true;
                    //}
                    //if (height - Data_Creating_Specifications.cap_Height + Data_Creating_Specifications.start_Size > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size &&
                    //    Data_Creating_Specifications.start_Size > 0 && enter == false && Data_Creating_Specifications.segments_Height - Data_Creating_Specifications
                    //    .start_Size < Data_Creating_Specifications.cap_Height)
                    //{
                    //    if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
                    //    {
                    //        Sheet_Next();
                    //    }
                    //    IList<double> first_Segment = new List<double>();
                    //    for (int i = 0; Data_Creating_Specifications.segments_Height  < height - Data_Creating_Specifications.cap_Height; i++)
                    //    {
                    //        first_Segment.Add(Data_Creating_Specifications.segments_Height / 304.8- Data_Creating_Specifications.sheet_9_Size / 304.8);
                    //        height = height - (Data_Creating_Specifications.segments_Height- Data_Creating_Specifications.sheet_9_Size  - Data_Creating_Specifications.cap_Height);
                    //    }
                    //    schedules.Split(first_Segment);
                    //    for (int i = 0; i < schedules.GetSegmentCount(); i++)
                    //    {
                    //        Data_Creating_Specifications.sheet_number++;
                    //        ViewSheet sheet_Next8 = ViewSheet.Create(Data_Creating_Specifications.titleBlock.Document, Data_Creating_Specifications.titleBlockid);

                    //        if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
                    //        {
                    //            Sheet_Next();
                    //        }
                    //        sheet_Next8.Name = Data_Creating_Specifications.start_Nume;
                    //        Data_Creating_Specifications.sheets_Collection.Add(sheet_Next8);
                    //        XYZ point8 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - Data_Creating_Specifications.sheet_9_Size / 304.8 - 5 / 304.8, 0);
                    //        ScheduleSheetInstance scheduleSheetInstanceNext8 = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications.Document,
                    //        sheet_Next8.Id, schedules.Id, point8, i);
                    //        if (i == schedules.GetSegmentCount() - 1)
                    //        {
                    //            BoundingBoxXYZ bbx8 = scheduleSheetInstanceNext8.get_BoundingBox((Autodesk.Revit.DB.View)Revit_Document_Creating_Specifications
                    //                .Document.GetElement(scheduleSheetInstanceNext8.OwnerViewId));
                    //            Data_Creating_Specifications.start_Size = (bbx8.Max.Y - bbx8.Min.Y) * 304.8 - 2.1162489764333;
                    //        }
                    //        Data_Creating_Specifications.sheet_number++;
                    //        Data_Creating_Specifications.lastlist = sheet_Next8;
                    //    }
                    //    //Data_Creating_Specifications.shedule_Number++;
                    //    enter = true;
                    //}
                    //if (height - Data_Creating_Specifications.cap_Height + Data_Creating_Specifications.start_Size > Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.sheet_9_Size &&
                    //    Data_Creating_Specifications.start_Size == 0 && enter == false)
                    //{
                    //    if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
                    //    {
                    //        Sheet_Next();
                    //    }
                    //    IList<double> first_Segment = new List<double>();
                    //    for (int i = 0; Data_Creating_Specifications.segments_Height- Data_Creating_Specifications.sheet_9_Size < height - Data_Creating_Specifications.cap_Height; i++)
                    //    {

                    //        first_Segment.Add(Data_Creating_Specifications.segments_Height / 304.8- Data_Creating_Specifications.sheet_9_Size / 304.8);
                    //        height = height - (Data_Creating_Specifications.segments_Height - Data_Creating_Specifications.cap_Height- Data_Creating_Specifications.sheet_9_Size ) ;
                    //    }
                    //    schedules.Split(first_Segment);
                    //    for (int i = 0; i < schedules.GetSegmentCount(); i++)
                    //    {
                    //        Data_Creating_Specifications.sheet_number++;
                    //        ViewSheet sheet_Next9 = ViewSheet.Create(Data_Creating_Specifications.titleBlock.Document, Data_Creating_Specifications.titleBlockid);

                    //        if (Data_Creating_Specifications.chekFormatLayersOne == true && Data_Creating_Specifications.sheet_number != 0)
                    //        {
                    //            Sheet_Next();
                    //        }
                    //        sheet_Next9.Name = Data_Creating_Specifications.start_Nume;
                    //        Data_Creating_Specifications.sheets_Collection.Add(sheet_Next9);
                    //        XYZ point9 = new XYZ(-Data_Creating_Specifications.height_FrameX + 20 / 304.8, Data_Creating_Specifications.height_FrameY - Data_Creating_Specifications.sheet_9_Size / 304.8 - 5 / 304.8, 0);
                    //        ScheduleSheetInstance scheduleSheetInstanceNext9 = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications.Document,
                    //        sheet_Next9.Id, schedules.Id, point9, i);
                    //        if (i == schedules.GetSegmentCount() - 1)
                    //        {
                    //            BoundingBoxXYZ bbx9 = scheduleSheetInstanceNext9.get_BoundingBox((Autodesk.Revit.DB.View)Revit_Document_Creating_Specifications
                    //                .Document.GetElement(scheduleSheetInstanceNext9.OwnerViewId));
                    //            Data_Creating_Specifications.start_Size = (bbx9.Max.Y - bbx9.Min.Y) * 304.8 - 2.1162489764333;
                    //        }
                    //        //Data_Creating_Specifications.sheet_number++;
                    //        Data_Creating_Specifications.lastlist = sheet_Next9;
                    //    }
                    //    //Data_Creating_Specifications.shedule_Number++;
                    //    enter = true;
                    //}
                    //enter = true;
                    //TaskDialog.Show("вы", Data_Creating_Specifications.shedule_Number.ToString());
                    //Revit_Document_Creating_Specifications.Document.Regenerate();

                    newT.Commit();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        public void ReNumber_Segments_Creating_SpecificationsSheet()
        {
            try
            {
                int shet_Number = 0;
                List<string> shets_All_Models_Number = new List<string>();
                ICollection<Element> sheets_All_Models = new List<Element>();
                using (Transaction newT1 = new Transaction(Revit_Document_Creating_Specifications.Document, "Определние списка всех листов модели: "))
                {
                    newT1.Start();
                    shet_Number = Data_Creating_Specifications.start_Number;
                    sheets_All_Models = new FilteredElementCollector(Revit_Document_Creating_Specifications.Document).OfCategory(BuiltInCategory.OST_Sheets).WhereElementIsNotElementType().ToElements();
                    foreach (ViewSheet element in sheets_All_Models)
                    {
                        shets_All_Models_Number.Add(element.SheetNumber);
                        //FilteredElementCollector viewportCollector = new FilteredElementCollector(Revit_Document_Creating_Specifications.Document).OfClass(typeof(Viewport))
                        //                                                                               .WhereElementIsNotElementType();



                        FilteredElementCollector collector = new FilteredElementCollector(Revit_Document_Creating_Specifications.Document, element.Id);
                        IList<Element> elements = collector.WhereElementIsNotElementType().ToElements();
                        IList<Element> specifications = elements.Where(elem => elem.Category?.Name == "Основные надписи").ToList();
                        foreach(Element element_On_Views in  specifications)
                        {
                            Parameter parameter = element_On_Views.LookupParameter("Девятиграфка_Шапка");
                            if (parameter != null)
                            {
                                parameter.Set(Data_Creating_Specifications.sheet_9_Size > 0 ? 1 : 0);
                            }
                        }
                       
                    }
                    shets_All_Models_Number.Sort();
                    newT1.Commit();
                }
                using (Transaction newT2 = new Transaction(Revit_Document_Creating_Specifications.Document, "Присваниевание имен и номеров листам: "))
                {
                    newT2.Start();
                    foreach (ViewSheet element_Select_Sheets_Collection in Data_Creating_Specifications.sheets_Collection)
                    {
                        element_Select_Sheets_Collection.Name = Data_Creating_Specifications.start_Nume;
                        element_Select_Sheets_Collection.SheetNumber = shet_Number.ToString() + "‏‏‎ ‎"; 
                        shet_Number++;
                    }
                    newT2.Commit();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        public void Sheet_Next()
        {
            try
            {
                string[] shedule_Name_Next_min = Data_Creating_Specifications.shedule_Nume_Next.ToString().Split(new[] { "Тип:" }, StringSplitOptions.None);
                Data_Creating_Specifications.shedule_Name_Min_Last = shedule_Name_Next_min[shedule_Name_Next_min.Count() - 1];
                Data_Creating_Specifications.collector = new FilteredElementCollector(Revit_Document_Creating_Specifications.Document);
                Data_Creating_Specifications.titleBlock = Data_Creating_Specifications.collector.OfCategory(BuiltInCategory.OST_TitleBlocks).ToElements().First(symbol => symbol.Name == Data_Creating_Specifications.shedule_Name_Min_Last);
                Data_Creating_Specifications.bbx_Frame = Data_Creating_Specifications.titleBlock.get_BoundingBox((Autodesk.Revit.DB.View)Revit_Document_Creating_Specifications
                    .Document.GetElement(Data_Creating_Specifications.titleBlock.OwnerViewId));
                Data_Creating_Specifications.titleBlockid = Data_Creating_Specifications.titleBlock.Id;
                Data_Creating_Specifications.height_FrameX = (Data_Creating_Specifications.bbx_Frame.Max.X - Data_Creating_Specifications.bbx_Frame.Min.X);
                Data_Creating_Specifications.height_FrameY = (Data_Creating_Specifications.bbx_Frame.Max.Y - Data_Creating_Specifications.bbx_Frame.Min.Y);
                Data_Creating_Specifications.segments_Height = Data_Creating_Specifications.segments_Height_Next;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        public double Segment_Height(ViewSchedule shedules)
        {
            try
            {
                ViewSheet sheet = ViewSheet.Create(Data_Creating_Specifications.titleBlock.Document, Data_Creating_Specifications.titleBlockid);
                XYZ point = new XYZ(-Data_Creating_Specifications.height_FrameX, Data_Creating_Specifications.height_FrameY, 0);
                ScheduleSheetInstance scheduleSheetInstance = ScheduleSheetInstance.Create(Revit_Document_Creating_Specifications
                    .Document, sheet.Id, shedules.Id, point, shedules.GetSegmentCount() - 1);
                BoundingBoxXYZ bbx = scheduleSheetInstance.get_BoundingBox((Autodesk.Revit.DB.View)Revit_Document_Creating_Specifications
                    .Document.GetElement(scheduleSheetInstance.OwnerViewId));
                double height = (bbx.Max.Y - bbx.Min.Y) * 304.8 - 2.1162489764333*2;
                Revit_Document_Creating_Specifications.Document.Delete(sheet.Id);
                return height;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return 0;
            }

        }
      
    }
}
