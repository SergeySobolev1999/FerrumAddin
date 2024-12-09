﻿#region Namespaces
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using AW = Autodesk.Windows;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Collections.Generic;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;
using RibbonItem = Autodesk.Revit.UI.RibbonItem;
using ComboBox = Autodesk.Revit.UI.ComboBox;
using Autodesk.Revit.UI.Events;
using System.Threading.Tasks;
using System.Windows.Controls;
using Autodesk.Windows;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;
using System.Xml.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using Autodesk.Revit.ApplicationServices;
using Transform = Autodesk.Revit.DB.Transform;
using System.Runtime.InteropServices;
using System.Windows.Forms;
#endregion

namespace FerrumAddin
{
    public class App : IExternalApplication
    {
        public AW.RibbonItem GetButton(string tabName, string panelName, string itemName)
        {
            AW.RibbonControl ribbon = AW.ComponentManager.Ribbon;
            foreach (AW.RibbonTab tab in ribbon.Tabs)
            {
                if (tab.Name == tabName)
                {
                    foreach (AW.RibbonPanel panel in tab.Panels) 
                    {
                        if (panel.Source.Title == panelName)
                        {
                            return panel.FindItem("CustomCtrl_%CustomCtrl_%"
                              + tabName + "%" + panelName + "%" + itemName,
                              true) as AW.RibbonItem;
                        }
                        //фпывп
                    }
                }
            }
            return null;
        }


        public static BitmapImage Convert(System.Drawing.Image img)
        {
            using (var memory = new MemoryStream())
            {
                img.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;

            }
        }
        public static string downloadDir;
        private static readonly string[] fileUrls =
        {
        "https://github.com/SergeySobolev1999/FerrumAddin/raw/refs/heads/master/DLL/FerrumAddin.dll"
        };
        private static async Task CheckForUpdates()
        {

            foreach (var url in fileUrls)
            {
                var fileName = Path.GetFileName(url);
                var localPath = Path.Combine(downloadDir, fileName);

                string oldHash = null;
                if (File.Exists(localPath))
                {
                    oldHash = GetFileHash(localPath);
                }

                var tempPath = Path.Combine(downloadDir, "new" + fileName);
                await DownloadFile(url, tempPath);
                var newHash = GetFileHash(tempPath);

                if (oldHash != newHash)
                {
                    
                    Update update = new Update();
                    update.ShowDialog();
                    Console.WriteLine($"{fileName} был обновлен.");
                }
                else
                {
                    File.Delete(tempPath);
                    Console.WriteLine($"{fileName} не изменился.");
                }
            }
        }

        private static async Task DownloadFile(string url, string localPath)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsByteArrayAsync();
                File.WriteAllBytes(localPath, content);
            }
        }

        private static string GetFileHash(string filePath)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
        public static UIControlledApplication application;
        public static UIApplication uiapp;
        public static string name;
        public RibbonPanel panelMEP;
        public RibbonPanel panelAR;
        public RibbonPanel panelKR;
        public RibbonPanel panelGeneral;
        public Result OnStartup(UIControlledApplication a)
        {
            application = a;
            Type type = a.GetType();

            string propertyName = "m_uiapplication";
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic
              | BindingFlags.GetField | BindingFlags.Instance;
            Binder binder = null;
            object[] args = null;

            object result = type.InvokeMember(
                propertyName, flags, binder, a, args);

            uiapp = (UIApplication)result;

            name = uiapp.Application.Username;
            List<string> admins = new List<string>();
            string filePath = "P:\\10_Документы\\Bim\\Библиотека ресурсов\\Revit\\Плагины\\Железно\\Admin.txt";
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        admins.Add(line);
                    }
                }
                
            }
            else
            {
                AlwaysLoad = true;
            }
            AllowLoad = false;
            downloadDir = a.ControlledApplication.CurrentUserAddinsLocation+"\\ZHELEZNO_PLUGIN";
            CheckForUpdates();
            //xmlFilePath = a.ControlledApplication.CurrentUserAddinsLocation + "\\Settings.xml";
            xmlFilePath = a.ControlledApplication.CurrentUserAddinsLocation + "\\ZHELEZNO_PLUGIN\\Settings";
            XElement root;
            if (System.IO.File.Exists(xmlFilePath))
            {
                root = XElement.Load(xmlFilePath);
            }
            else
            {
                root = new XElement("Settings");
            }


            XElement frmMangerElement = root.Element("frmManger");
            if (frmMangerElement == null)
            {
                frmMangerElement = new XElement("frmManger");
                root.Add(frmMangerElement);
            }
            XElement frmTabPath = root.Element("TabPath");
            if (frmTabPath == null)
            {
                frmTabPath = new XElement("TabPath");
                frmTabPath.SetAttributeValue("Path", a.ControlledApplication.CurrentUserAddinsLocation + "\\ZHELEZNO_PLUGIN\\TabItems");
                root.Add(frmTabPath);
            }
            TabPath = frmTabPath.Attribute("Path").Value;
            root.Save(xmlFilePath);

            string tabName = "Железно";

            a.CreateRibbonTab(tabName);
            RibbonPanel panelFerrum = a.CreateRibbonPanel(tabName, "Железно");
            PushButtonData conf = new PushButtonData("frmConfig", "Настройки", Assembly.GetExecutingAssembly().Location, "FerrumAddin.ConfiguratorShow");
            conf.Image = Convert(Properties.Resources.ferrum);
            conf.LargeImage = Convert(Properties.Resources.ferrum);
            ComboBoxData comboBoxData = new ComboBoxData("ChangeRazd");
            List<RibbonItem> items = panelFerrum.AddStackedItems(conf, comboBoxData).ToList();
            AW.RibbonItem ri = GetButton(tabName, "Железно", comboBoxData.Name);
            ri.Width = 110;
            ComboBox cb = (items[1] as ComboBox);
            cb.AddItems(new List<ComboBoxMemberData>() { new ComboBoxMemberData("Common", "Общие"),
                                                                             new ComboBoxMemberData("Views", "Виды"),
                                                                             new ComboBoxMemberData("General", "Общее"),
                                                                             new ComboBoxMemberData("AR", "АР"),
                                                                             new ComboBoxMemberData("KR", "КР"),
                                                                             new ComboBoxMemberData("MEP", "MEP"),
                                                                             new ComboBoxMemberData("Control", "Управление")});

            cb.CurrentChanged += Cb_CurrentChanged;


            PushButtonData FamilyManager = new PushButtonData("frmManager", "Менеджер\nсемейств", Assembly.GetExecutingAssembly().Location, "FerrumAddin.FamilyManagerShow"); 
            FamilyManager.Image = Convert(Properties.Resources.FamilyManager);
            FamilyManager.LargeImage = Convert(Properties.Resources.FamilyManager);
            panelFerrum.AddItem(FamilyManager);

            PushButtonData Comparison = new PushButtonData("frmComparison", "Сопоставление\nсемейств", Assembly.GetExecutingAssembly().Location, "FerrumAddin.FM.ComparisonWindowShow");
            Comparison.Image = Convert(Properties.Resources.FamilyManager);
            Comparison.LargeImage = Convert(Properties.Resources.FamilyManager);          
            panelFerrum.AddItem(Comparison);

            //Панель Общее

            panelGeneral = a.CreateRibbonPanel(tabName, "Общее");
            panelGeneral.Visible = false;

            PushButtonData PinnerWorksets = new PushButtonData("PinnerWorksets", "Закрепление\nи наборы", Assembly.GetExecutingAssembly().Location, "masshtab.PinnerWorksets");
            PinnerWorksets.Image = Convert(Properties.Resources.icon32);
            PinnerWorksets.LargeImage = Convert(Properties.Resources.icon32);
            panelGeneral.AddItem(PinnerWorksets);

           

            //Панель МЕР
            panelMEP = a.CreateRibbonPanel(tabName, "ВИС");
            panelMEP.Visible = false;

            PushButtonData MEPName = new PushButtonData("mepName", "Наименование труб|воздуховодов", Assembly.GetExecutingAssembly().Location, "FerrumAddin.CommandMepName");
            panelMEP.AddItem(MEPName);

            //Панель АР
            panelAR = a.CreateRibbonPanel(tabName, "АР");
            panelAR.Visible = false;

            PushButtonData Main_The_Floor_Is_Numeric = new PushButtonData("Main_The_Floor_Is_Numeric", "Запись\nэтажа", Assembly.GetExecutingAssembly().Location, "WPFApplication.The_Floor_Is_Numeric.Main_The_Floor_Is_Numeric"); 
            Main_The_Floor_Is_Numeric.Image = Convert(Properties.Resources.icon32);
            Main_The_Floor_Is_Numeric.LargeImage = Convert(Properties.Resources.icon32);
            panelAR.AddItem(Main_The_Floor_Is_Numeric);

            PushButtonData Main_Parameter_On_Group_Stained_Glass_Windows = new PushButtonData("Main_Parameter_On_Group_Stained_Glass_Windows", "Витражи\nпереименование", Assembly.GetExecutingAssembly().Location, "WPFApplication.Parameter_On_Group_Stained_Glass_Windows.Main_Parameter_On_Group_Stained_Glass_Windows");
            Main_Parameter_On_Group_Stained_Glass_Windows.Image = Convert(Properties.Resources.logo_icon_248043);
            Main_Parameter_On_Group_Stained_Glass_Windows.LargeImage = Convert(Properties.Resources.logo_icon_248043);
            panelAR.AddItem(Main_Parameter_On_Group_Stained_Glass_Windows);

            PushButtonData Main_Mark_On_Group_Stained_Glass_Windows = new PushButtonData("Main_Mark_On_Group_Stained_Glass_Windows", "Витражи\nмаркировки", Assembly.GetExecutingAssembly().Location, "WPFApplication.Mark_On_Group_Stained_Glass_Windows.Main_Mark_On_Group_Stained_Glass_Windows");
            Main_Mark_On_Group_Stained_Glass_Windows.Image = Convert(Properties.Resources.logo_icon_248043);
            Main_Mark_On_Group_Stained_Glass_Windows.LargeImage = Convert(Properties.Resources.logo_icon_248043);
            panelAR.AddItem(Main_Mark_On_Group_Stained_Glass_Windows);

            PushButtonData Main_Assembling_On_Group_Stained_Glass_Windows = new PushButtonData("Main_Assembling_On_Group_Stained_Glass_Windows", "Витражи\nсборки", Assembly.GetExecutingAssembly().Location, "WPFApplication.Assembling_Project_On_Group_Stained_Glass_Windows.Main_Assembling_On_Group_Stained_Glass_Windows");
            Main_Assembling_On_Group_Stained_Glass_Windows.Image = Convert(Properties.Resources.logo_icon_248043);
            Main_Assembling_On_Group_Stained_Glass_Windows.LargeImage = Convert(Properties.Resources.logo_icon_248043);
            panelAR.AddItem(Main_Assembling_On_Group_Stained_Glass_Windows);

            //Панель КР
            panelKR = a.CreateRibbonPanel(tabName, "КР");
            panelKR.Visible = false;

            PushButtonData LintelCreator = new PushButtonData("LintelCreator", "Создание перемычек", Assembly.GetExecutingAssembly().Location, "FerrumAddin.CommandLintelCreator");
            LintelCreator.Image = Convert(Properties.Resources.FamilyManager);
            LintelCreator.LargeImage = Convert(Properties.Resources.FamilyManager);
            panelKR.AddItem(LintelCreator);

            PushButtonData SteelSpecCollapse = new PushButtonData("SteelSpecCollapse", "ВРС", Assembly.GetExecutingAssembly().Location, "masshtab.SteelSpecCollapse");
            SteelSpecCollapse.Image = Convert(Properties.Resources.icon32);
            SteelSpecCollapse.LargeImage = Convert(Properties.Resources.icon32);
            panelKR.AddItem(SteelSpecCollapse);

            PushButtonData StructureFileFix = new PushButtonData("StructureFileFix", "Ускоритель КЖ", Assembly.GetExecutingAssembly().Location, "masshtab.StructureFileFix"); 
            StructureFileFix.Image = Convert(Properties.Resources.icon32);
            StructureFileFix.LargeImage = Convert(Properties.Resources.icon32);
            panelKR.AddItem(StructureFileFix);

            FamilyManagerWindow dock = new FamilyManagerWindow();
            dockableWindow = dock;

            DockablePaneId id = new DockablePaneId(new Guid("{68D44FAC-CF09-46B2-9544-D5A3F809373C}"));
            try
            {
                a.RegisterDockablePane(id, "Менеджер семейств Железно",
                        dockableWindow as IDockablePaneProvider);
                if ((admins.Count != 0 && admins.Contains(name)) || AlwaysLoad == true)
                {
                }
                else
                {
                    a.ControlledApplication.FamilyLoadingIntoDocument += ControlledApplication_FamilyLoadingIntoDocument;
                }
                a.ControlledApplication.DocumentOpened += ControlledApplication_DocumentOpened;
                a.ViewActivated += A_ViewActivated;
                LoadEvent = ExternalEvent.Create(new LoadEvent());
            }
            catch (Exception ex)
            {

            }

            ButtonConf(root);

            return Result.Succeeded;
        }
        public static string xmlFilePath;
        public static string TabPath;
        public static string FamilyFolder;
        
        public static Dictionary<string, bool> GetElementStates(XElement root)
        {
            var elementStates = new Dictionary<string, bool>();

            foreach (var element in root.Elements())
            {
                if (bool.TryParse(element.Attribute("IsChecked")?.Value, out bool isChecked))
                {
                    elementStates[element.Name.LocalName] = isChecked;
                }
            }

            return elementStates;
        }
        public static void ButtonConf(XElement root)
        {
            Dictionary<string,bool> names = GetElementStates(root);
            Autodesk.Windows.RibbonControl ribbon = Autodesk.Windows.ComponentManager.Ribbon;
            foreach (Autodesk.Windows.RibbonTab tab in ribbon.Tabs)
            {
                if (tab.Title.Contains("Железно"))
                {
                    foreach (Autodesk.Windows.RibbonPanel panel in tab.Panels)
                    {

                        RibbonItemCollection collctn = panel.Source.Items;
                        foreach (Autodesk.Windows.RibbonItem ri in collctn)
                        {
                            string name = ri.Id.Split('%').Last();
                            if (names.Keys.ToList().Contains(name))
                            {
                                ri.IsVisible = names[name];
                                ri.ShowText = names[name];
                                ri.ShowImage = names[name];
                            }
                        }
                        
                    }
                }
            }
        }

        private void A_ViewActivated(object sender, ViewActivatedEventArgs e)
        {
            if (AllowLoad == false)
            {
                Document d = e.Document;
                dockableWindow.CustomInitiator(d);
            }
        }

        private void ControlledApplication_DocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs e)
        {
            if (AllowLoad == false)
            {
                Document d = e.Document;
                dockableWindow.CustomInitiator(d);
            }
        }

        public static ExternalEvent LoadEvent;
        public static bool AllowLoad = false;
        public static bool AlwaysLoad = false;
        private void ControlledApplication_FamilyLoadingIntoDocument(object sender, Autodesk.Revit.DB.Events.FamilyLoadingIntoDocumentEventArgs e)
        {
            if (AllowLoad == true || AlwaysLoad == true)
            {
              
            }
            else
            {
                e.Cancel();
                TaskDialog.Show("Запрет загрузки", "Загрузите семейство из менеджера семейств");
            }
        }

        public static FamilyManagerWindow dockableWindow = null;
        ExternalCommandData edata = null;

        

        private void Cb_CurrentChanged(object sender, Autodesk.Revit.UI.Events.ComboBoxCurrentChangedEventArgs e)
        {
            string vkl = e.NewValue.ItemText;
            switch (vkl)
            {
                case "Общее":
                    panelGeneral.Visible = true;
                    panelMEP.Visible = false;
                    panelAR.Visible = false;
                    panelKR.Visible = false;
                    break;
                case "MEP":
                    panelGeneral.Visible = false;
                    panelMEP.Visible = true;
                    panelAR.Visible = false;
                    panelKR.Visible = false;
                    break;
                case "АР":
                    panelGeneral.Visible = false;
                    panelMEP.Visible = false;
                    panelAR.Visible = true;
                    panelKR.Visible = false;
                    break;
                case "КР":
                    panelGeneral.Visible = false;
                    panelKR.Visible = true;
                    panelMEP.Visible = false;
                    panelAR.Visible = false;
                    break;
                default:
                    panelGeneral.Visible = false;
                    panelMEP.Visible=false;
                    panelAR.Visible = false;
                    panelKR.Visible = false;
                    break;
            }
        }

        public static void a_DialogBoxShowing(
  object sender,
  DialogBoxShowingEventArgs e)
        {
            if (e.DialogId == "Dialog_Revit_DocWarnDialog")
                e.OverrideResult((int)System.Windows.Forms.DialogResult.Yes);
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            Process process = Process.GetCurrentProcess();
            var updaterProcess = Process.Start(new ProcessStartInfo(downloadDir + "\\Updater.exe", process.Id.ToString()));
            return Result.Succeeded;
        }
    }

    public class LoadEvent : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            var nameAndCat = new Dictionary<string, BuiltInCategory>
        {
            { "Стены", BuiltInCategory.OST_Walls },
            { "Перекрытия", BuiltInCategory.OST_Floors },
            { "Потолки", BuiltInCategory.OST_Ceilings },
            { "Витражи", BuiltInCategory.OST_Walls },
            { "Крыши" , BuiltInCategory.OST_Roofs},
            { "Ограждения" , BuiltInCategory.OST_StairsRailing},
            { "Пандусы", BuiltInCategory.OST_Ramps }
        };

            Document docToCopy = FamilyManagerWindow.doc;
            List<MenuItem> list = new List<MenuItem>();
            foreach (TabItemViewModel tab in FamilyManagerWindow.mvm.TabItems)
            {
                list.AddRange(tab.MenuItems.Where(x => x.IsSelected).ToList());
            }
            App.AllowLoad = true;
            List<Document> documents = new List<Document>();
            
            foreach (MenuItem tab in list)
            {
                bool isFirstOptionChecked = FamilyManagerWindow.IsFirstOptionChecked();
                if (tab.Path.EndsWith("rfa"))
                {
                    string familyName = System.IO.Path.GetFileNameWithoutExtension(tab.Path);
                    Family existingFamily = new FilteredElementCollector(docToCopy)
                        .OfClass(typeof(Family))
                        .Cast<Family>()
                        .FirstOrDefault(fam => fam.Name == familyName);

                    if (existingFamily != null)
                    {
                        if (isFirstOptionChecked)
                        {
                            // Замена существующего семейства
                            using (Transaction tx = new Transaction(docToCopy))
                            {
                                tx.Start("Загрузка семейств");
                                FailureHandlingOptions failureOptions = tx.GetFailureHandlingOptions();
                                failureOptions.SetFailuresPreprocessor(new MyFailuresPreprocessor());
                                failureOptions.SetClearAfterRollback(true); // Опционально
                                tx.SetFailureHandlingOptions(failureOptions);
                                MyFamilyLoadOptions loadOptions = new MyFamilyLoadOptions();
                                docToCopy.LoadFamily(tab.Path, loadOptions, out Family load);
                                tx.Commit();
                            }
                        }
                        else
                        {
                            using (Transaction tx = new Transaction(docToCopy))
                            {
                                tx.Start("Загрузка семейств");
                                FailureHandlingOptions failureOptions = tx.GetFailureHandlingOptions();
                                failureOptions.SetFailuresPreprocessor(new MyFailuresPreprocessor());
                                failureOptions.SetClearAfterRollback(true); // Опционально
                                tx.SetFailureHandlingOptions(failureOptions);
                                MyFamilyLoadOptions loadOptions = new MyFamilyLoadOptions();
                                string famPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.GetFileNameWithoutExtension(tab.Path )+ "_1.rfa");
                                File.Copy(tab.Path, famPath, true);
                                docToCopy.LoadFamily(famPath, loadOptions, out Family load);
                                File.Delete(famPath);
                                tx.Commit();
                            }
                        }
                    }
                    else
                    {
                        // Загрузка без конфликтов
                        using (Transaction tx = new Transaction(docToCopy))
                        {
                            tx.Start("Загрузка семейств");
                            FailureHandlingOptions failureOptions = tx.GetFailureHandlingOptions();
                            failureOptions.SetFailuresPreprocessor(new MyFailuresPreprocessor());
                            failureOptions.SetClearAfterRollback(true); // Опционально
                            tx.SetFailureHandlingOptions(failureOptions);
                            MyFamilyLoadOptions loadOptions = new MyFamilyLoadOptions();
                            docToCopy.LoadFamily(tab.Path, loadOptions, out Family load);
                            tx.Commit();
                        }
                    }
                }
                else
                {
                    Document document = app.Application.OpenDocumentFile(tab.Path);
                    documents.Add(document);
                    List<ElementId> el = new FilteredElementCollector(document)
                        .OfCategory(nameAndCat[tab.Category])
                        .WhereElementIsElementType()
                        .Where(x => x.Name == tab.Name)
                        .Select(x => x.Id)
                        .ToList();

                    bool elementExists = ElementExists(docToCopy, nameAndCat[tab.Category], tab.Name);

                    if (!elementExists)
                    {
                        // Копирование без конфликтов
                        using (Transaction tx = new Transaction(docToCopy))
                        {
                            tx.Start("Загрузка семейств");
                            FailureHandlingOptions failureOptions = tx.GetFailureHandlingOptions();
                            failureOptions.SetFailuresPreprocessor(new MyFailuresPreprocessor());
                            failureOptions.SetClearAfterRollback(true); // Опционально
                            tx.SetFailureHandlingOptions(failureOptions);
                            ElementTransformUtils.CopyElements(document, el, docToCopy, null, null);
                            tx.Commit();
                        }
                    }
                    else
                    {
                        CopyPasteOptions options = new CopyPasteOptions();
                        options.SetDuplicateTypeNamesHandler(new MyCopyHandler());
                        if (isFirstOptionChecked)
                        {
                            // Замена существующего элемента
                            using (Transaction tx = new Transaction(docToCopy))
                            {
                                tx.Start("Загрузка семейств");
                                FailureHandlingOptions failureOptions = tx.GetFailureHandlingOptions();
                                failureOptions.SetFailuresPreprocessor(new MyFailuresPreprocessor());
                                failureOptions.SetClearAfterRollback(true); // Опционально
                                tx.SetFailureHandlingOptions(failureOptions);

                                // Получаем элементы из исходного документа
                                List<Element> elementsToCopy = new List<Element>();
                                foreach (ElementId id in el)
                                {
                                    Element elem = document.GetElement(id);
                                    elementsToCopy.Add(elem);
                                }

                                // Проходим по каждому элементу для обработки
                                foreach (Element sourceElement in elementsToCopy)
                                {
                                    if (sourceElement is ElementType sourceType)
                                    {
                                        // Копируем тип в целевой документ
                                        ICollection<ElementId> copiedIds = ElementTransformUtils.CopyElements(
                                            document,
                                            new List<ElementId> { sourceType.Id },
                                            docToCopy,
                                            Transform.Identity,
                                            options);

                                        ElementId copiedTypeId = copiedIds.First();
                                        ElementType copiedType = docToCopy.GetElement(copiedTypeId) as ElementType;

                                        // Ищем существующий тип с таким же именем в целевом документе
                                        ElementType existingType = FindTypeByNameAndClass(docToCopy, sourceType.Name, sourceType.GetType());

                                        if (existingType != null && existingType.Id != copiedType.Id)
                                        {
                                            // Заменяем все элементы, использующие старый тип, на новый тип
                                            ReplaceElementsType(docToCopy, existingType.Id, copiedType.Id);

                                            // Удаляем старый тип
                                            docToCopy.Delete(existingType.Id);
                                        }
                                        // Если типа не было, ничего дополнительно делать не нужно
                                    }
                                }

                                tx.Commit();
                            }
                        }

                        else
                        {
                            // Переименование копируемого элемента
                            using (Transaction tx = new Transaction(docToCopy))
                            {
                                tx.Start("Загрузка семейств");
                                FailureHandlingOptions failureOptions = tx.GetFailureHandlingOptions();
                                failureOptions.SetFailuresPreprocessor(new MyFailuresPreprocessor());
                                failureOptions.SetClearAfterRollback(true); // Опционально
                                tx.SetFailureHandlingOptions(failureOptions);
                                ICollection<ElementId> copiedIds = ElementTransformUtils.CopyElements(document, el, docToCopy, Transform.Identity, options);
                                ElementId copiedId = copiedIds.First();
                                Element copiedElement = docToCopy.GetElement(copiedId);

                                string newName = GetUniqueElementName(docToCopy, nameAndCat[tab.Category], tab.Name);
                                copiedElement.Name = newName;

                                tx.Commit();
                            }
                        }
                    }
                    
                }
            }
            App.AllowLoad = false;
            FamilyManagerWindow.Reload();
            foreach (Document doc in documents)
            {
                doc.Close(false);
            }
        }
        public string GetName()
        {
            return "LoadEventHandler";
        }

        // Метод для поиска типа по имени и классу в целевом документе
        private ElementType FindTypeByNameAndClass(Document doc, string typeName, Type typeClass)
        {
            return new FilteredElementCollector(doc)
                .OfClass(typeClass)
                .Cast<ElementType>()
                .FirstOrDefault(e => e.Name.Equals(typeName, StringComparison.InvariantCultureIgnoreCase));
        }

        // Метод для замены типа у всех элементов
        private void ReplaceElementsType(Document doc, ElementId oldTypeId, ElementId newTypeId)
        {
            // Находим все элементы, использующие старый тип
            List<Element> collector = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .Where(e => e.GetTypeId() == oldTypeId).ToList();

            foreach (Element elem in collector)
            {
                // Устанавливаем новый тип
                elem.ChangeTypeId(newTypeId);
            }
        }


        private bool ElementExists(Document doc, BuiltInCategory category, string elementName)
        {
            return new FilteredElementCollector(doc)
                .OfCategory(category)
                .WhereElementIsElementType()
                .Any(x => x.Name == elementName);
        }

        private string GetUniqueFamilyName(Document doc, string baseName)
        {
            string newName = baseName;
            int i = 1;
            while (new FilteredElementCollector(doc)
                .OfClass(typeof(Family))
                .Cast<Family>()
                .Any(fam => fam.Name == newName))
            {
                newName = $"{baseName}_{i}";
                i++;
            }
            return newName;
        }

        private string GetUniqueElementName(Document doc, BuiltInCategory category, string baseName)
        {
            string newName = baseName;
            int i = 1;
            while (new FilteredElementCollector(doc)
                .OfCategory(category)
                .WhereElementIsElementType()
                .Any(e => e.Name == newName))
            {
                newName = $"{baseName}_{i}";
                i++;
            }
            return newName;
        }
    }

    public class MyCopyHandler : IDuplicateTypeNamesHandler
    {


        public DuplicateTypeAction OnDuplicateTypeNamesFound(DuplicateTypeNamesHandlerArgs args)
        {
            return DuplicateTypeAction.UseDestinationTypes;
        }
    }

    public class MyFamilyLoadOptions : IFamilyLoadOptions
    {
        public bool OnFamilyFound(
       bool familyInUse,
       out bool overwriteParameterValues)
        {
            overwriteParameterValues = true;
            return true;
        }

        public bool OnSharedFamilyFound(
          Family sharedFamily,
          bool familyInUse,
          out FamilySource source,
          out bool overwriteParameterValues)
        {
            source = FamilySource.Family;
            overwriteParameterValues = false;
            return true;
        }
    }

    public class MyFailuresPreprocessor : IFailuresPreprocessor
    {
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
        {
            /*IList<FailureMessageAccessor> failures
    = failuresAccessor.GetFailureMessages();

            foreach (FailureMessageAccessor f in failures)
            {
                FailureSeverity fseverity = failuresAccessor.GetSeverity();

                if (fseverity == FailureSeverity.Warning)
                {
                    failuresAccessor.DeleteWarning(f);
                }
                else
                {
                    failuresAccessor.ResolveFailure(f);
                    return FailureProcessingResult.ProceedWithCommit;
                }
            }
            return FailureProcessingResult.Continue;*/
            IList<FailureMessageAccessor> failures = failuresAccessor.GetFailureMessages();

            foreach (FailureMessageAccessor failure in failures)
            {
                // Определяем степень серьезности ошибки
                FailureSeverity severity = failure.GetSeverity();

                // Обрабатываем в зависимости от степени серьезности
                if (severity == FailureSeverity.Warning)
                {
                    // Удаляем предупреждения
                    failuresAccessor.DeleteWarning(failure);
                }
                else
                {
                    // Проверяем, можно ли автоматически решить ошибку
                    if (failure.HasResolutions())
                    {
                        // Применяем первое доступное решение
                        failuresAccessor.ResolveFailure(failure);
                        return FailureProcessingResult.ProceedWithCommit;
                    }
                    else
                    {
                        // Если решений нет, удаляем ошибку или логируем
                        failuresAccessor.DeleteWarning(failure);
                    }
                }
            }

            // Продолжаем транзакцию без отображения диалоговых окон
            return FailureProcessingResult.Continue;
        }
    }


    public class CommandAvailability : IExternalCommandAvailability
    {
        // interface member method
        public bool IsCommandAvailable(UIApplication app, CategorySet cate)
        {
            // zero doc state
            if (app.ActiveUIDocument == null)
            {
                // disable register btn
                return true;
            }
            // enable register btn
            return false;
        }
    }


}
