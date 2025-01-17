﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using SSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApplication.Licenses;
using WPFApplication.The_Floor_Is_Numeric;

namespace FerrumAddin
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandLintelCreator : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            SSDK_Data.username = Environment.UserName;
            if (SSDK_Data.licenses_Connection)
            {

                Document doc = commandData.Application.ActiveUIDocument.Document;
                Selection sel = commandData.Application.ActiveUIDocument.Selection;

                List<ElementId> windowsAndDoorsSelectionIds = sel.GetElementIds().ToList();
                List<FamilyInstance> windowsAndDoorsList = new List<FamilyInstance>();
                windowsAndDoorsList = GetWindowsAndDoorsFromCurrentSelection(doc, windowsAndDoorsSelectionIds);

                if (windowsAndDoorsList.Count == 0)
                {
                    if (doc.ActiveView as ViewSchedule != null)
                    {
                        message = "Для выбора окон и дверей перейдите на план или 3D вид!";
                        return Result.Cancelled;
                    }

                    WndowsAndDoorsSelectionFilter selFilter = new WndowsAndDoorsSelectionFilter();
                    IList<Reference> selWindowsAndDoors = null;
                    try
                    {
                        selWindowsAndDoors = sel.PickObjects(ObjectType.Element, selFilter, "Выберите окна и двери!");
                    }
                    catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                    {
                        return Result.Cancelled;
                    }

                    foreach (Reference roomRef in selWindowsAndDoors)
                    {
                        windowsAndDoorsList.Add(doc.GetElement(roomRef) as FamilyInstance);
                    }
                }

                List<Family> lintelFamilysList = new FilteredElementCollector(doc)
                    .OfClass(typeof(Family))
                    .Cast<Family>()
                    .Where(f => f.FamilyCategory.Id.IntegerValue.Equals((int)BuiltInCategory.OST_StructuralFraming))
                    .Where(f => f.GetFamilySymbolIds() != null)
                    .Where(f => f.GetFamilySymbolIds().Count != 0)
                    .Where(f => (doc.GetElement(f.GetFamilySymbolIds().First()) as FamilySymbol).get_Parameter(BuiltInParameter.ALL_MODEL_MODEL).AsString() == "Перемычки составные")
                    .OrderBy(f => f.Name, new AlphanumComparatorFastString())
                    .ToList();

                if (lintelFamilysList.Count == 0)
                {
                    message = "В проекте не найдены семейства перемычек! Загрузите семейства с наличием ''Перемычки составные'' в парметре типа ''Модель''.";
                    return Result.Cancelled;
                }

                List<Parameter> openingParameters = new List<Parameter>();
                foreach (FamilyInstance windowOrDoor in windowsAndDoorsList)
                {
                    ParameterSet windowOrDoorSymbolParameterSet = windowOrDoor.Symbol.Parameters;
                    foreach (Parameter param in windowOrDoorSymbolParameterSet)
                    {
                        if (((int)param.Definition.ParameterGroup).Equals((int)BuiltInParameterGroup.PG_GEOMETRY) && openingParameters.FirstOrDefault(p => p.Id == param.Id) == null)
                        {
                            openingParameters.Add(param);
                        }

                    }

                    ParameterSet windowOrDooInstanceParameterSet = windowOrDoor.Parameters;
                    foreach (Parameter param in windowOrDooInstanceParameterSet)
                    {
                        if (((int)param.Definition.ParameterGroup).Equals((int)BuiltInParameterGroup.PG_GEOMETRY) && openingParameters.FirstOrDefault(p => p.Id == param.Id) == null)
                        {
                            openingParameters.Add(param);
                        }
                    }
                }
                openingParameters = openingParameters.OrderBy(p => p.Definition.Name, new AlphanumComparatorFastString()).ToList();

                //Вызов формы
                LintelCreatorForm lintelCreatorWPF = new LintelCreatorForm(doc, lintelFamilysList, openingParameters);
                lintelCreatorWPF.ShowDialog();
                if (lintelCreatorWPF.DialogResult != true)
                {
                    return Result.Cancelled;
                }

                string OpeningHeightParamName = lintelCreatorWPF.OpeningHeightParam.Definition.Name;
                string OpeningWidthParamName = lintelCreatorWPF.OpeningWidthParam.Definition.Name;

                FamilySymbol lintelTargetFamilySymbol = lintelCreatorWPF.LintelTargetFamilySymbol;

                using (TransactionGroup tg = new TransactionGroup(doc))
                {
                    tg.Start("Создание перемычек");
                    foreach (FamilyInstance windowOrDoor in windowsAndDoorsList)
                    {
                        FamilyInstance newLintel = null;

                        Parameter openingHeightParam = windowOrDoor.LookupParameter(OpeningHeightParamName);
                        if (openingHeightParam == null)
                        {
                            openingHeightParam = windowOrDoor.Symbol.LookupParameter(OpeningHeightParamName);
                        }
                        Parameter openingWidthParam = windowOrDoor.LookupParameter(OpeningWidthParamName);
                        if (openingWidthParam == null)
                        {
                            openingWidthParam = windowOrDoor.Symbol.LookupParameter(OpeningWidthParamName);
                        }

                        Level level = doc.GetElement(windowOrDoor.LevelId) as Level;
                        XYZ locationPoint = (windowOrDoor.Location as LocationPoint).Point - level.Elevation * XYZ.BasisZ + openingHeightParam.AsDouble() * XYZ.BasisZ;

                        using (Transaction t = new Transaction(doc))
                        {
                            t.Start("Создание и поворот перемычки");
                            lintelTargetFamilySymbol.Activate();
                            newLintel = doc.Create.NewFamilyInstance(locationPoint, lintelTargetFamilySymbol, level, Autodesk.Revit.DB.Structure.StructuralType.NonStructural) as FamilyInstance;

                            if (!windowOrDoor.FacingOrientation.IsAlmostEqualTo(newLintel.FacingOrientation))
                            {
                                Line rotateAxis = Line.CreateBound((newLintel.Location as LocationPoint).Point, (newLintel.Location as LocationPoint).Point + 1 * XYZ.BasisZ);
                                double u1 = windowOrDoor.FacingOrientation.AngleOnPlaneTo(XYZ.BasisX, XYZ.BasisZ);
                                double u1a = u1 * (180 / Math.PI);
                                double u2 = newLintel.FacingOrientation.AngleOnPlaneTo(XYZ.BasisX, XYZ.BasisZ);
                                double u2a = u2 * (180 / Math.PI);
                                double rotateAngle = (u2a - u1a) * (Math.PI / 180);

                                ElementTransformUtils.RotateElement(doc, newLintel.Id, rotateAxis, rotateAngle);
                            }
                            t.Commit();
                        }

                        using (Transaction t = new Transaction(doc))
                        {
                            t.Start("Заполнение параметра \"Длина линии видимости\"");
                            if (newLintel.LookupParameter("Длина линии видимости") != null)
                            {
                                newLintel.LookupParameter("Длина линии видимости").Set((newLintel.Location as LocationPoint).Point.Z - level.Elevation);
                            }
                            t.Commit();
                        }

                    }
                    tg.Assimilate();
                }
            }
            else
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. Ваша лицензия недоступна. Выполните переподключение");
                s_Mistake_String.ShowDialog();
            }

            return Result.Succeeded;
        }

        private static List<FamilyInstance> GetWindowsAndDoorsFromCurrentSelection(Document doc, List<ElementId> selIds)
        {
            List<FamilyInstance> tempLintelsList = new List<FamilyInstance>();
            foreach (ElementId lintelId in selIds)
            {
                if (doc.GetElement(lintelId) is FamilyInstance
                    && null != doc.GetElement(lintelId).Category
                    && (doc.GetElement(lintelId).Category.Id.IntegerValue.Equals((int)BuiltInCategory.OST_Windows)
                    || doc.GetElement(lintelId).Category.Id.IntegerValue.Equals((int)BuiltInCategory.OST_Doors)))
                {
                    tempLintelsList.Add(doc.GetElement(lintelId) as FamilyInstance);
                }
            }
            return tempLintelsList;
        }
    }
}
