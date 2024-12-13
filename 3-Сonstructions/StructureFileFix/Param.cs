using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using SSDK;

namespace masshtab
{
    public class Param
    {
        public string Name { get; set; }
        public Definition def;
        public List<Category> categories = new List<Category>();
        public ForgeTypeId paramGroup;
        public Guid guid;

        public Param(Parameter param, Document doc)
        {
            try
            {
                def = param.Definition;
                Name = def.Name;

                InternalDefinition intDef = def as InternalDefinition;
                if (intDef != null) paramGroup = intDef.GetGroupTypeId();

                guid = param.GUID;


                ElementBinding elemBind = this.GetBindingByParamName(Name, doc);

                foreach (Category cat in elemBind.Categories)
                {
                    categories.Add(cat);
                }
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }

        public bool RemoveOrAddFromRebarCategory(Document doc, Element elem, bool addOrDeleteCat)
        {

            Autodesk.Revit.ApplicationServices.Application app = doc.Application;

            ElementBinding elemBind = this.GetBindingByParamName(Name, doc);

            //получаю список категорий
            CategorySet newCatSet = app.Create.NewCategorySet();
            int rebarcatid = (int)(new ElementId(BuiltInCategory.OST_Rebar).Value);
            foreach (Category cat in elemBind.Categories)
            {
                int catId = (int)(cat.Id.Value);
                if (catId != rebarcatid)
                {
                    newCatSet.Insert(cat);
                }
            }

            if (addOrDeleteCat)
            {
                Category cat = elem.Category;
                newCatSet.Insert(cat);
            }

            TypeBinding newBind = app.Create.NewTypeBinding(newCatSet);
            if (doc.ParameterBindings.Insert(def, newBind, paramGroup))
            {
                return true;
            }
            else
            {
                if (doc.ParameterBindings.ReInsert(def, newBind, paramGroup))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void AddToProjectParameters(Document doc, Element elem)
        {
            try
            {
                Autodesk.Revit.ApplicationServices.Application app = doc.Application;
                //string oldSharedParamsFile = app.SharedParametersFilename;




                ExternalDefinition exDef = null;
                string sharedFile = app.SharedParametersFilename;
                DefinitionFile sharedParamFile = app.OpenSharedParameterFile();
                foreach (DefinitionGroup defgroup in sharedParamFile.Groups)
                {
                    foreach (Definition def in defgroup.Definitions)
                    {
                        if (def.Name == Name)
                        {
                            exDef = def as ExternalDefinition;
                        }
                    }
                }
                if (exDef == null) throw new Exception("В файле общих параметров не найден общий параметр " + Name);


                CategorySet catSet = app.Create.NewCategorySet();
                catSet.Insert(elem.Category);
                TypeBinding newBind = app.Create.NewTypeBinding(catSet);

                doc.ParameterBindings.Insert(exDef, newBind, paramGroup);

                //app.SharedParametersFilename = oldSharedParamsFile;

                Parameter testParam = elem.LookupParameter(Name);
                if (testParam == null) throw new Exception("Не удалось добавить обший параметр " + Name);
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }



        private ElementBinding GetBindingByParamName(String paramName, Document doc)
        {

            Autodesk.Revit.ApplicationServices.Application app = doc.Application;
            DefinitionBindingMapIterator iter = doc.ParameterBindings.ForwardIterator();
            while (iter.MoveNext())
            {
                Definition curDef = iter.Key;
                if (!Name.Equals(curDef.Name)) continue;

                def = curDef;
                ElementBinding elemBind = (ElementBinding)iter.Current;
                return elemBind;
            }
            throw new Exception("не найден параметр " + paramName);
        }
    }
}