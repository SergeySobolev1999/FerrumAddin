﻿using System;
using Autodesk.Revit.DB;
using SSDK;


namespace masshtab
{
    public class ParamValue
    {
        public Param projectParam;
        public StorageType storageType;
        public bool IsValid = false;
        public bool IsNull = false;

        public string StringValue;
        public double DoubleValue;
        public int IntegerValue;
        public int ElementIdValue;

        public ParamValue(Parameter revitParam)
        {
            try
            {
                if (!revitParam.HasValue)
                {
                    IsNull = true;
                    return;
                }
                storageType = revitParam.StorageType;
                switch (storageType)
                {
                    case StorageType.None:
                        break;
                    case StorageType.Integer:
                        IntegerValue = revitParam.AsInteger();
                        IsValid = true;
                        break;
                    case StorageType.Double:
                        DoubleValue = revitParam.AsDouble();
                        IsValid = true;
                        break;
                    case StorageType.String:
                        StringValue = revitParam.AsString();
                        IsValid = true;
                        break;
                    case StorageType.ElementId:
                        ElementIdValue = revitParam.AsElementId().IntegerValue;
                        IsValid = true;
                        break;
                    default:
                        IsValid = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }

        public override string ToString()
        {

            switch (storageType)
            {
                case StorageType.None:
                    return "none value";
                case StorageType.Integer:
                    return IntegerValue.ToString();
                case StorageType.Double:
                    return DoubleValue.ToString("F2");
                case StorageType.String:
                    return StringValue;
                case StorageType.ElementId:
                    return ElementIdValue.ToString();
                default:
                    throw new Exception("Invalid value for StorageType");
            }
        }

        public void SetValue(Parameter revitParam)
        {
            try
            {
                if (revitParam.IsReadOnly) return;
                switch (revitParam.StorageType)
                {
                    case StorageType.None:
                        return;
                    case StorageType.Integer:
                        revitParam.Set(IntegerValue);
                        return;

                    case StorageType.Double:
                        revitParam.Set(DoubleValue);
                        return;

                    case StorageType.String:
                        revitParam.Set(StringValue);
                        return;

                    case StorageType.ElementId:
                        ElementId id = new ElementId(ElementIdValue);
                        revitParam.Set(id);
                        return;

                    default:
                        throw new Exception("Invalid value for StorageType");
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