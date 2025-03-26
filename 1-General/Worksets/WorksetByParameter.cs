
using System;
using System.Collections.Generic;

namespace WPFApplication.Worksets
{
    [Serializable]
    public class WorksetByParameter : WorksetBy
    {
        public List<string> ParameterNames;

        public WorksetByParameter()
        {

        }
    }
}
