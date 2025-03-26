
using System;
using System.Collections.Generic;

namespace WPFApplication.Worksets
{
    [Serializable]
    public class WorksetByFamily : WorksetBy
    {
        public List<string> FamilyNames;

        public WorksetByFamily()
        {

        }
    }
}
