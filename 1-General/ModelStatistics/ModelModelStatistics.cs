using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumAddin.ModelStatistics
{
    public class StatisticData
    {
        public string Secuence_Number { get; set; }  = "";
        public string Id_Persons { get; set; } = "";
        public string File_Name { get; set; } = "";
        public string Post { get; set; } = "";
        public string User { get; set; } = "";
        public string Date_Open { get; set; } = "";
        public string Time_Open { get; set; } = "";
        public string Time_Synchr { get; set; } = "";
        public StatisticData(string secuence_number, string id_persons, string file_name, string post, string user, string date_open, string time_Open, string time_Synchr)
        {
            this.Secuence_Number = secuence_number;
            this.Id_Persons = id_persons;
            this.File_Name = file_name;
            this.Post = post;
            this.User = user;
            this.Date_Open = date_open;
            this.Time_Open = time_Open;
            this.Time_Synchr = time_Synchr;
        }
    }
    public class StatisticModelElements
    {
        public string Secuence_Number { get; set; } = "";
        public string Id_Persons { get; set; } = "";
        public string File_Name { get; set; } = "";
        public string Post { get; set; } = "";
        public string User { get; set; } = "";
        public string Date { get; set; } = "";
        public string Number_Elements { get; set; } = "";
        public StatisticModelElements(string secuence_number, string id_persons, string file_name, string post, string user, string date, string number_elements)
        {
            this.Secuence_Number = secuence_number;
            this.Id_Persons = id_persons;
            this.File_Name = file_name;
            this.Post = post;
            this.User = user;
            this.Date = date;
            this.Number_Elements = number_elements;
        }
    }
}
