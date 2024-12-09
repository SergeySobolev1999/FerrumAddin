using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System;
using System.IO;

namespace masshtab
{
    public class Logger
    {
        public static string logPath = "C:/ProgramData/Autodesk/Revit/Addins/2024/";
        public Logger(in DateTime dateTime, in string function, in string txt)
        {
            UIDocument uidoc = RevitApi.UiDocument;
            Document doc = RevitApi.Document;
            UIApplication uiApp = RevitApi.UiApplication;
            Autodesk.Revit.ApplicationServices.Application rvtApp = uiApp.Application;
            //имя документа без пользователя и отсоединено
            string docName = doc.Title.ToString(); docName = docName.Replace(",", " ");
            string userName = rvtApp.Username; string docNameUserName = "_" + userName; docName = docName.Replace(docNameUserName, "");
            //имя лог-файла с временем запуска плагина
            string date = dateTime.ToString(); date = date.Replace(":", "-");
            string logFilePath = logPath + date + "," + userName + "," + docName + "," + function + "," + ".txt";
            //время отработки для текста лог-файла
            DateTime dateTime1 = DateTime.Now; string time = dateTime1.ToLongTimeString();
            //запись данных
            try
            {
                File.AppendAllText(logFilePath, "\n" + time + " " + txt);
            }
            catch (Exception) { }
        }
    }
}
