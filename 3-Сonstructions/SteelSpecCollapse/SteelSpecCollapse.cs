using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace masshtab
{
    [Transaction(TransactionMode.Manual)]
    public class SteelSpecCollapse : IExternalCommand
    {
        private bool hidecolumn(in string value)
        {
            bool hide = false;
            switch (value)
            {
                case "0": hide = true; break;
                case "0.0": hide = true; break;
                case "0,0": hide = true; break;
                case "0.00": hide = true; break;
                case "0,00": hide = true; break;
            }
            return hide;
        }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                string className = "Закрепление и наборы"; DateTime dateTime = DateTime.Now;
            if (RevitApi.UiApplication == null) { RevitApi.Initialize(commandData); }
            //создание log-файла
            new Logger(dateTime, className, "Старт работы;");

            Document doc = RevitApi.Document; UIDocument uidoc = RevitApi.UiDocument;

            new Logger(dateTime, className, "Сбор элементов;");

            List<ViewSchedule> schedules = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Schedules)   //фильтр по категории Спецификации
                                                                         .WhereElementIsNotElementType()    //фильтр только экземпляры
                                                                         .Cast<ViewSchedule>()                     //элементы категории Спецификации
                                                                         .ToList();                         //формируем список

            List<ViewSchedule> steelschedules = new List<ViewSchedule>();


            foreach (ViewSchedule schedule in schedules) //заполняем список ведомостей расхода стали
            {
                string name = schedule.Name;
                if (name.Contains("ВРС"))
                {
                    steelschedules.Add(schedule);
                }
                if (name.Contains("Ведомость расхода стали"))
                {
                    steelschedules.Add(schedule);
                }
            }

            new Logger(dateTime, className, "Диалоговое окно;");
            //Диалог
            var viewModel = new sscViewModel();
            ///тут можно вставить десериализацию вьюмодели
            var wpfview = new sscwpf(viewModel);
            viewModel.CloseRequest += (s, e) => wpfview.Close();
            bool? ok = wpfview.ShowDialog();
            if (ok != null && ok == true) { } else { return Result.Cancelled; }
            ///тут можно сериализовать вьюмодель

            bool all = viewModel.all; bool active = viewModel.visible;

            new Logger(dateTime, className, "Проверяем, является ли открытый вид спецификацией;");

            View v = doc.ActiveView;
            bool vrs = v.Name.Contains("ВРС") | v.Name.Contains("Ведомость расхода стали");
            bool isActiveView_vrs = v.Title.Contains("Спецификация") && vrs;



            using (Transaction t = new Transaction(doc))
            {

                if (active == true)
                {
                    if (isActiveView_vrs == false)
                    {
                            //S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка.  Текущий открытый вид не является ведомостью расхода стали.\n" +
                            //"Если все же является - щелкните мышью на любую из ячеек таблицы.\n" +
                            //"В имени спецификации должно содержаться ВРС либо Ведомость расхода стали");
                            new S_Mistake_String("Ошибка.  Текущий открытый вид не является ведомостью расхода стали.\n" +
                            "Если все же является - щелкните мышью на любую из ячеек таблицы.\n" +
                            "В имени спецификации должно содержаться ВРС либо Ведомость расхода стали").ShowDialog();
                            //new iWindow(info1txt).ShowDialog();
                        new Logger(dateTime, className, "Текущий вид не является ВРС.");
                        return Result.Cancelled;
                    }
                    new Logger(dateTime, className, "Сценарий: активный вид;");
                    ViewSchedule activeView = (ViewSchedule)uidoc.ActiveView;
                    new Logger(dateTime, className, "Ведомость: " + activeView.Name);

                    t.Start("Подчистка текущей ВРС");
                    new Logger(dateTime, className, "Открываем транзакцию;");

                    string[] names = new string[] { "", "Марка конструкции", "Напрягаемая арматура класса", "Изделия арматурные", "Изделия закладные" }; //заголовки
                    int cc = activeView.Definition.GetFieldCount();

                    for (int i = 0; i < cc; i++)
                    {
                        activeView.Definition.GetField(i).IsHidden = false; //включаем все поля
                    }
                    TableSectionData tb = activeView.GetTableData().GetSectionData(SectionType.Body);
                    TableSectionData th = activeView.GetTableData().GetSectionData(SectionType.Header);

                    int nc = tb.NumberOfColumns;
                    int nr = tb.NumberOfRows;
                    for (int c = 0; c < nc; c++) //столбец
                    {
                        ScheduleField sf = activeView.Definition.GetField(c);
                        string heading = tb.GetCellText(0, c); //группа заголовков столбца (самая верхняя ячейка столбца)
                        bool hideColumn = true;
                        for (int r = 0; r < nr; r++) //строка
                        {
                            if (tb.GetCellType(r, c) == CellType.ParameterText)
                            {
                                string tbt = tb.GetCellText(r, c); //значение в ячейке
                                hideColumn = hidecolumn(tbt); //нужно ли скрывать ячейку
                                if (!hideColumn) break; //на первой же ячейке с ненулевым значением решаем не скрывать столбец
                            }
                        }
                        if (hideColumn) sf.IsHidden = true; //скрываем столбцы, где все значения нулевые

                        bool headcontainsfield = false;
                        foreach (string name in names) //проходим по списку заголовков names
                        {
                            if (name == heading) headcontainsfield = true;
                        }
                        if (!headcontainsfield) sf.IsHidden = true; //скрываем столбцы, не входящие в заголовки names
                        if(sf.FieldIndex==5) sf.IsHidden = true;
                    }

                    t.Commit();
                    new Logger(dateTime, className, "Закрываем транзакцию;");

                }
                else
                {
                    new Logger(dateTime, className, "Сценарий: все ВРС;");
                    t.Start("Подчистка всех ВРС");
                    new Logger(dateTime, className, "Открываем транзакцию;");

                    foreach (ViewSchedule schedule in steelschedules) //проходим по каждой ВРС
                    {
                        new Logger(dateTime, className, "Ведомость: " + schedule.Name);
                        string[] names = new string[] { "", "Марка конструкции", "Напрягаемая арматура класса", "Изделия арматурные", "Изделия закладные" }; //заголовки
                        int cc = schedule.Definition.GetFieldCount();

                        for (int i = 0; i < cc; i++)
                        {
                            schedule.Definition.GetField(i).IsHidden = false; //включаем все поля
                        }
                        TableSectionData tb = schedule.GetTableData().GetSectionData(SectionType.Body);
                        TableSectionData th = schedule.GetTableData().GetSectionData(SectionType.Header);

                        int nc = tb.NumberOfColumns;
                        int nr = tb.NumberOfRows;
                        for (int c = 0; c < nc; c++) //столбец
                        {
                            ScheduleField sf = schedule.Definition.GetField(c);
                            string heading = tb.GetCellText(0, c); //группа заголовков столбца (самая верхняя ячейка столбца)
                            bool hideColumn = true;
                            for (int r = 0; r < nr; r++) //строка
                            {
                                if (tb.GetCellType(r, c) == CellType.ParameterText)
                                {
                                    string tbt = tb.GetCellText(r, c); //значение в ячейке
                                    hideColumn = hidecolumn(tbt); //нужно ли скрывать ячейку
                                    if (!hideColumn) break; //на первой же ячейке с ненулевым значением решаем не скрывать столбец
                                }
                            }
                            if (hideColumn) sf.IsHidden = true;  //скрываем столбцы, где все значения нулевые

                            bool headcontainsfield = false;
                            foreach (string name in names) //проходим по списку заголовков names
                            {
                                if (name == heading) headcontainsfield = true; 
                            }
                            if (!headcontainsfield) sf.IsHidden = true; //скрываем столбцы, не входящие в заголовки names
                            if (sf.FieldIndex == 5) sf.IsHidden = true;
                        }

                    }

                    new iWindow("Успешно!\nВсе ВРС в проекте подчищены").ShowDialog();
                    t.Commit();
                    new Logger(dateTime, className, "Закрываем транзакцию;");
                }


            }
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }

            return Result.Succeeded;
        }
    }
}
