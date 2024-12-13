using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using SSDK;

namespace masshtab
{
    public class Rebar
    {

        public string Name { get; set; }
        public RebarBarType bartype;
        public Dictionary<string, ParamValue> ValuesStorage = new Dictionary<string, ParamValue>();

        public Rebar(RebarBarType BarType)
        {
            try
            {
                Name = BarType.Name;
                bartype = BarType;

                foreach (Parameter param in BarType.ParametersMap)
                {
                    string paramName = param.Definition.Name;
                    ParamValue mpv = new ParamValue(param);
                    ValuesStorage.Add(paramName, mpv);
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
