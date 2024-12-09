﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace masshtab
{
    public static class ParamTools
    {
        public static bool CheckParameterExistsInFile(DefinitionFile deffile, Guid paramGuid)
        {
            if (deffile == null)
            {
                throw new Exception("Не подключен файл общих параметров");
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
                    throw new Exception("Не удалось создать группу " + groupName + " в файле общих параметров " + defFile.Filename);
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
                throw new Exception("Не удалось создать общий параметр " + myparam.Name);
            }
            return exDef;
        }
    }
}