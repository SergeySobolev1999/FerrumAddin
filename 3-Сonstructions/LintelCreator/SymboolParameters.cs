﻿using Autodesk.Revit.DB;
namespace FerrumAddin
{
    public class SymboolParameters
    {
        public Parameter ParameterValue { get; set; }
        public FamilySymbol FamilySymbolValue { get; set; }
        public SymboolParameters (Parameter parameter, FamilySymbol familySymbol)
        {
            ParameterValue = parameter;
            FamilySymbolValue = familySymbol;
        }
    }
}
