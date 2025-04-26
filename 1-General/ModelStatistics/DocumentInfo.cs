using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;

namespace FerrumAddin.ModelStatistics
{
    public class DocumentInfo
    {
        // ========== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ==========
        public string DocumentInfoPosition(Document doc)
        {
            try
            {
                if (doc == null)
                {
                    TaskDialog.Show("Ошибка", "Нет открытого документа!");
                    return "Failed";
                }

                string report = GenerateDocumentReport(doc);
                return report;
            }
            catch (Exception ex)
            {
                return "Failed";
            }
        }
    private string GenerateDocumentReport(Document doc)
        {
        // ===== 1. Базовая информация =====
        string report = "";
        string path = doc.PathName;
        bool isFamily = doc.IsFamilyDocument;
        bool isTemplate = path.EndsWith(".rte", StringComparison.OrdinalIgnoreCase);
        bool isFamilyTemplate = path.EndsWith(".rft", StringComparison.OrdinalIgnoreCase);
        if (isFamily)
        {
            if (isFamilyTemplate)
            {
                report += "ШАБЛОН_СЕМЕЙСТВА_";
            }
            else
            {
                report += "СЕМЕЙСТВО_";
            }
        }
        else
        {
            if (isTemplate)
            {
                report += "ШАБЛОН_ПРОЕКТА_";
            }
            else
            {
                report += "ПРОЕКТ_";
                if (doc.IsWorkshared)
                {
                    report += "ФХ_";
                }
                else
                {
                    report += "ЛФ_";
                }
            }
        }
        // ===== 3. Проверка совместной работы =====
        report += $"Версия Revit: {doc.Application.VersionName}";
        return report;
        }
    }
}
