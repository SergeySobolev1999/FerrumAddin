using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using SSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using static Autodesk.Revit.DB.SpecTypeId;

namespace WPFApplication.newMainRenaming
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class newMainRenaming : IExternalCommand
    {
        public static Guid guid_COD = new Guid("631cd69e-065f-4ec2-8894-4359325312c3");
        public static Guid guidADSKShortName = new Guid("f194bf60-b880-4217-b793-1e0c30dda5e9");
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                SSDK_Data.licenses_Name = Environment.UserName;
                //if (SSDK_Data.licenses_Connection)
                //{
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();
            List<Element> selectedElements = selectedIds.Select(id => uidoc.Document.GetElement(id)).Where(e => e != null).ToList();
            List<Element> collections = new List<Element>();
            int iteration = 0;
            if (selectedIds.Count > 0)
            {
                collections = Filtered_Elements(selectedElements , uidoc.Document);
                if (collections.Count is 0)
                {
                    S_Mistake_String s_Mistake_String = new S_Mistake_String($"Ошибка. Среди предвариельно выделенных элементов нет подходящих для обработки");
                    s_Mistake_String.ShowDialog();
                }
            }
            else
            {
                List<Element> all_Elements = (List<Element>)new FilteredElementCollector(uidoc.Document).WhereElementIsNotElementType().ToElements();
                collections = Filtered_Elements(all_Elements, uidoc.Document);
                if (collections.Count is 0)
                {
                    S_Mistake_String s_Mistake_String = new S_Mistake_String($"Ошибка. Среди элементов модели нет подходящих для обработки");
                    s_Mistake_String.ShowDialog();
                }
            }
            if (collections.Count != 0)
            {
                using (TransactionGroup transactionGroup = new TransactionGroup(uidoc.Document, "Переименование многослойных конструкций"))
                {
                    transactionGroup.Start();
                    int warning_Parameter = 0;
                    int warning_Material = 0;
                    foreach (Element element in collections)
                    {
                        string nameResult = "";
                        Element elementType = uidoc.Document.GetElement(element.GetTypeId());
                        List<Autodesk.Revit.DB.Material> material_Collections = GetMaterials(element, uidoc.Document);
                        int iterarion_WarinngMaterial = 0;
                        foreach (Autodesk.Revit.DB.Material material in material_Collections)
                        {
                            if (material.Name == "Стена по умолчанию")
                            {
                                iterarion_WarinngMaterial++;
                            }
                        }
                        if (elementType.get_Parameter(guid_COD)==null|| elementType.get_Parameter(guid_COD).AsDouble() == 0)
                        {
                            warning_Parameter++;
                        }
                        var codificationAndPrefics = new Dictionary<(double, double), string>
                        {
                            { (201,201.099),"ZH_НСЩ_КЛД_" },
                            { (202,202.099),"ZH_НСЩ_КЛД_" },
                            { (201.100,201.199),"ZH_НСЩ_СТН_ПНЛ_" },
                            { (201.200,201.299),"ZH_НСЩ_МНЛ_" },
                            { (203.000,203.099),"ZH_ПРГ_КЛД_" },
                            { (203.100,203.199),"ZH_ПРГ_ССТ_МТЛ_КРКС_" },
                            { (204,206.999),"ZH_ПОЛ_" },
                            { (221,221.999),"ZH_ПТЛ_" },
                            { (222,222.099),"ZH_НСЩ_ПЛТ_ПРК_" },
                            { (222.100,222.199),"ZH_НСЩ_МНЛ_ПРК_" },
                            { (223.000,223.009),"ZH_ОТД_ФСД_" },
                            { (223.012,223.099),"ZH_ОТД_ФСД_" },
                            { (224.000,224.999),"ZH_ОТД_ФСД_" },
                            { (223.100,223.109),"ZH_ОТД_ФСД_СЛЖ_РСТ_" },
                            { (223.110,223.119),"ZH_ОТД_ФСД_СЛЖ_ВСТ_" },
                            { (236,250.999),"ZH_ОТД_ВНТ_" },
                            { (267.000,267.099),"ZH_ПРП_КЛД_" },
                            { (267.100,267.199),"ZH_ПРП_СТН_ПНЛ_" },
                            { (267.200,267.299),"ZH_ПРП_МНЛ_" },
                            { (268,268.099),"ZH_ЭКР_ЛДЖ_КЛД_" },
                            { (268.100,268.199),"ZH_ЭКР_ЛДЖ_СТН_ПНЛ_" },
                            { (268.200,268.299),"ZH_ЭКР_ЛДЖ_МНЛ_" },
                            { (269.000,269.099),"ZH_ГДР_ФНД_" },
                            { (269.100,269.199),"ZH_ГДР_ПРМ_КРШ_" },
                            { (270.000,270.099),"ZH_ТПЛ_ФНД_" },
                            { (270.100,270.199),"ZH_ТПЛ_ЦКЛ_" },
                            { (219.000,219.099),"ZH_КРВ_ПЛС_" },
                            { (219.100,219.199),"ZH_КРВ_СКТ_" },
                            { (219.200,219.299),"ZH_ДПЛ_ПЛЩ_" },
                            { (214.001,214.010),"ZH_ВНТЛ_СТН_ШДЛ_" },
                            { (223.011,223.011),"ZH_ОТД_ФСД_ТРФ_" },
                            { (272.000,272.999),"ZH_УСЛ_" },
                            { (274.001,274.001),"ZH_ГНП_ПЛЩ_" },
                            { (274.002,274.002),"ZH_ГНП_ВХД_" },
                            { (203.901,203.901),"ZH_ПРГ_СТН_ШДЛ_" },
                            { (203.902,203.902),"ZH_ПРГ_СТН_ИК_КМР_" },
                            { (227.000,227.999),"ZH_ОТД_ФСД_ВСТ_ОКН_" },
                        };
                        if (iterarion_WarinngMaterial == 0 && elementType.get_Parameter(guid_COD) != null)
                        {

                            double parameter_Value = elementType.get_Parameter(guid_COD).AsDouble() * 304.8;
                            nameResult = codificationAndPrefics.Where(a => parameter_Value >= a.Key.Item1 && parameter_Value <= a.Key.Item2).Select(a => a.Value).FirstOrDefault();
                            double thickness = 0;
                            if (elementType.get_Parameter(guidADSKShortName) != null && elementType.get_Parameter(guidADSKShortName).AsValueString() != "" && element.Category.Name == "Перекрытия")
                            {
                                    nameResult += elementType.get_Parameter(guidADSKShortName).AsValueString()+"_";
                            }
                            Dictionary<int, string> materialCombination = new Dictionary<int, string>();
                            int numLayer = 0;
                            foreach (Autodesk.Revit.DB.Material material in material_Collections)
                            {
                                foreach (var position in GetDescriptionsMaterial(material, ref thickness, element, uidoc.Document))
                                {
                                    numLayer++;
                                    materialCombination.Add(numLayer, position.Value);
                                }
                            }
                            nameResult += thickness.ToString() + "_";
                            materialCombination = materialCombination.OrderByDescending(a => a.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                            foreach (var pos in materialCombination)
                            {
                                nameResult += pos.Value;
                            }
                            using (Transaction transaction1 = new Transaction(uidoc.Document, "Переименование"))
                                {
                                transaction1.Start();
                                    if(nameResult.Count()>5)
                                    SSDK_Parameter.Set_Type_Name(elementType, nameResult);
                                transaction1.Commit();
                            }
                            iteration++;
                        }
                        warning_Material += iterarion_WarinngMaterial;
                        
                    }
                    transactionGroup.Assimilate();
                    string warining_Rusult = "";
                    if (warning_Parameter > 0)
                    {
                        warining_Rusult += $"Ошибка. Параметр 'ZH_Код_Тип' не найден в {warning_Parameter.ToString()} элементах\n";
                    }
                    if (warning_Material > 0)
                    {
                        warining_Rusult += $"Ошибка. Задан материал 'По категории' в {warning_Material.ToString()} позициях";
                    }
                    if (warning_Parameter > 0 || warning_Material > 0)
                    {
                        S_Mistake_String s_Mistake_String = new S_Mistake_String(warining_Rusult);
                        s_Mistake_String.ShowDialog();
                    }
                }
            }
            if (iteration > 0)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String($"Запись завершена. Успешно обработаных элементов: {iteration.ToString()}");
                s_Mistake_String.ShowDialog();
            }
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
            return Result.Succeeded;
        }

        public List<Element> Filtered_Elements(List<Element> elements , Document document)
        {
            List<Element> collections = new List<Element>();
            foreach (Element element in elements)
            {
                Element elementType = document.GetElement(element.GetTypeId());
                FamilySymbol familySymbol = elementType as FamilySymbol;
                var nameAndCat = new Dictionary<string, BuiltInCategory>
                {
                    { "Стены", BuiltInCategory.OST_Walls },
                    { "Перекрытия", BuiltInCategory.OST_Floors },
                    { "Потолки", BuiltInCategory.OST_Ceilings },
                    { "Крыши", BuiltInCategory.OST_Roofs },
                };
                if (element.Category == null) { }
                else if (element.GetTypeId() == null) { }
                else if (element.Category.Name == "Крыши" && document.GetElement(element.GetTypeId()) is RoofType && (document.GetElement(element.GetTypeId()) as RoofType).FamilyName == "Наклонное остекление") { }
                else if (element is FamilyInstance familyInstance && familyInstance.Symbol.Family != null) { }
                else
                {
                    if (nameAndCat.Values.Contains((BuiltInCategory)element.Category.Id.Value))
                    {
                        collections.Add(element);
                    }
                }
            }
            collections = collections.GroupBy(a => a.GetTypeId()).Select(g=>g.First()).ToList();
            return collections;
        }
        public List<Autodesk.Revit.DB.Material> GetMaterials(Element element, Document document)
        {
            List<Autodesk.Revit.DB.Material> materials = new List<Autodesk.Revit.DB.Material>();

            if (element is HostObject hostObject)
            {
                CompoundStructure structure = GetStructure(document ,element);
                if (structure != null)
                {
                    foreach (CompoundStructureLayer layer in structure.GetLayers())
                    {
                        Autodesk.Revit.DB.Material material = document.GetElement(layer.MaterialId) as Autodesk.Revit.DB.Material;
                        if (material != null)
                        {
                            materials.Add(material);
                        }
                    }
                }
            }

            return materials;
        }
        private CompoundStructure GetStructure(Document document ,Element element)
        {
            if (element is Wall wall)
                return wall.WallType?.GetCompoundStructure();

            if (element is Floor floor)
                return floor.FloorType?.GetCompoundStructure();

            if (element is RoofBase roof)
                return roof.RoofType?.GetCompoundStructure();

            if (element is Ceiling ceiling)
            {
                var type = document.GetElement(ceiling.GetTypeId()) as CeilingType;
                return type?.GetCompoundStructure();
            }

            return null;
        }
        public Dictionary<int,string> GetDescriptionsMaterial(Autodesk.Revit.DB.Material material, ref double thickness, Element element, Document document)
        {
            double wignth = 0;
            int layersId = 0;
            CompoundStructure structure = null;
            if (IsWall(element)) {structure = (element as Wall).WallType.GetCompoundStructure();}
            if (IsFloor(element)) { structure = (element as Floor).FloorType.GetCompoundStructure(); }
            if (IsRoofBase(element)) { structure = (element as RoofBase).RoofType.GetCompoundStructure(); }
            if (IsCeiling(element)) { structure = ((CeilingType)document.GetElement(element.GetTypeId())).GetCompoundStructure(); }
            foreach (CompoundStructureLayer layer in structure.GetLayers())
            {
                if (layer.MaterialId != ElementId.InvalidElementId && material.Id == layer.MaterialId)
                {
                    wignth = layer.Width * 304.8;
                    layersId= layer.LayerId;
                }
            }
            thickness += wignth;
            string str = material.get_Parameter(BuiltInParameter.ALL_MODEL_DESCRIPTION).AsValueString().ToLower();
            string renamingStr = "";
            var charvowels = new Dictionary<string, string>
            {
                { "а","" },
                { "е","" },
                { "ё","" },
                { "и","" },
                { "й","" },
                { "о","" },
                { "у","" },
                { "ы","" },
                { "ю","" },
                { "я","" },
                { " ","_" },
            };
            renamingStr = string.Concat(str.Select(c => charvowels.ContainsKey(c.ToString()) ? charvowels[c.ToString()] : c.ToString())) + "_" + wignth.ToString() + "_";

            Dictionary<int, string> codificationAndPrefics = new Dictionary<int, string>()
            {
                { layersId,renamingStr }
            };
            return codificationAndPrefics;
        }
        public bool IsWall(Element element){return element is Wall;}
        public bool IsFloor(Element element) { return element is Floor; }
        public bool IsRoofBase(Element element) { return element is RoofBase; }
        public bool IsCeiling(Element element) { return element is Ceiling; }
    }
}
