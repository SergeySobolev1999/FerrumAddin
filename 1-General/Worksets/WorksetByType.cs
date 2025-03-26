
using System;
using System.Collections.Generic;

namespace WPFApplication.Worksets
{
    [Serializable]
    public class WorksetByType : WorksetBy
    {
        public List<string> TypeNames;

        public WorksetByType()
        {

        }
    }
}
