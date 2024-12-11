using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RevitAPI_Basic_Course.Creating_Specifications
{

    //[Serializable]
    //public class Serialize_Creating_Specifications
    //{
    //    public List<Serializw_Information> serialize_Information {get;set;} = new List<Serializw_Information>();
    //}

    [Serializable]

    public class Serialize_Data
    {
        [XmlAttribute("ows_Business Unit")]
        public string First_List { get;set; }
        public string Next_List { get; set; }
        public int number_Sheet_First { get;set;}
        public string sheet_Name { get;set;}
        public bool sheet9 {  get;set;}
        public double segment_Size { get;set;}
        public double sizeSegments_Next { get; set; }
        public bool first_Sheet_Unique { get;set;}
        public Serialize_Data() { }
        public Serialize_Data(string First_List, string Next_List, int number_Sheet_First, string sheet_Name, bool sheet9, double segment_Size, double sizeSegments_Next, bool first_Sheet_Unique)
        {
            this.Next_List = Next_List;
            this.First_List = First_List;
            this.number_Sheet_First = number_Sheet_First;
            this.sheet_Name = sheet_Name;
            this.sheet9 = sheet9;
            this.segment_Size = segment_Size;
            this.sizeSegments_Next = sizeSegments_Next;
            this.first_Sheet_Unique = first_Sheet_Unique;
        }

    }
    [Serializable]
    public class Serialize_Set_Data
    {
        [XmlElement("Миша. Хорошо")]
        public string Name { get; set; }
        List<string> Elements { get; set; }
        public Serialize_Set_Data() { }
        public Serialize_Set_Data(string Name, List<string> Elements)
        {
            this.Name = Name;
            this.Elements = Elements;
        }
    }
}
