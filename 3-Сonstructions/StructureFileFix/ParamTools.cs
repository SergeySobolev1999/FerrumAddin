﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using SSDK;

namespace masshtab
{
    public static class ParamTools
    {
        public static bool CheckParameterExistsInFile(DefinitionFile deffile, Guid paramGuid)
        {
            try
            {
                if (deffile == null)
                {
                    new S_Mistake_String("Не подключен файл общих параметров").ShowDialog();
                    //throw new Exception("Не подключен файл общих параметров");
                }
                foreach (DefinitionGroup defgr in deffile.Groups)
                {
                    foreach (ExternalDefinition exdf in defgr.Definitions)
                    {
                        if (paramGuid.Equals(exdf.GUID))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
            return false;
        }

        public static ExternalDefinition AddParameterToDefFile(DefinitionFile defFile, string groupName, Param myparam)
        {

            DefinitionGroup tempGroup = null;
            List<DefinitionGroup> groups = defFile.Groups.Where(i => i.Name == groupName).ToList();
            if (groups.Count == 0)
            {
                try
                {
                    tempGroup = defFile.Groups.Create(groupName);
                }
                catch (Exception)
                {
                    new S_Mistake_String("Не удалось создать группу \" + groupName + \" в файле общих параметров \" + defFile.Filename").ShowDialog();
                    //throw new Exception("Не удалось создать группу " + groupName + " в файле общих параметров " + defFile.Filename);
                }
            }
            else
            {
                tempGroup = groups.First();
            }


            Definitions defs = tempGroup.Definitions;
            ExternalDefinitionCreationOptions defOptions =
                  new ExternalDefinitionCreationOptions(myparam.Name, myparam.def.GetDataType());
            defOptions.GUID = myparam.guid;

            ExternalDefinition exDef = defs.Create(defOptions) as ExternalDefinition;
            if (exDef == null)
            {
                new S_Mistake_String("Не удалось создать общий параметр \" + myparam.Name").ShowDialog();
                //throw new Exception("Не удалось создать общий параметр " + myparam.Name);
            }
            return exDef;
        }
    }
}