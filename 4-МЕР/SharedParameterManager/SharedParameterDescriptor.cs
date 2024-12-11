using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApplication.SharedParameterManager
{
    public class SharedParameterDescriptor
    {
        private readonly Element _sharedParameter;
        public string Name { get; }
        public ElementId Id { get; }


        public SharedParameterDescriptor(Element sharedParameter)
        {
            _sharedParameter = sharedParameter;
            Name = sharedParameter.Name;
            Id = sharedParameter.Id;
        }
    }
}
