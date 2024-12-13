using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SSDK;

namespace WPFApplication.The_Floor_Is_Numeric
{
    /// <summary>
    /// Логика взаимодействия для WPF_Main_The_Floor_Is_Numeric.xaml
    /// </summary>
    public partial class WPF_Main_The_Floor_Is_Numeric : Window
    {
        Record_The_Floor_Is_Numeric record_The_Floor_Is_Numeric = new Record_The_Floor_Is_Numeric();
        public WPF_Main_The_Floor_Is_Numeric(ExternalCommandData commandData)
        {
            Revit_Document_The_Floor_Is_Numeric.Initialize(commandData);
            InitializeComponent();
            record_The_Floor_Is_Numeric.Work_Set_Download_The_Floor_Is_Numeric();
            Scrol_Viewer1_The_Floor_Is_Numericw();
        }

        private void Start_The_Floor_Is_Numeric(object sender, RoutedEventArgs e)
        {
            Data_The_Floor_Is_Numeric.number_True_Element = 0;
            Data_The_Floor_Is_Numeric.door_Checked = (bool)Door_The_Floor_Is_Numeric.IsChecked.Value;
            Data_The_Floor_Is_Numeric.window_Checked = (bool)Window_The_Floor_Is_Numeric.IsChecked.Value;
            Data_The_Floor_Is_Numeric.wall_Checked = (bool)Wall_The_Floor_Is_Numeric.IsChecked.Value;
            Data_The_Floor_Is_Numeric.model_Group_Checked = (bool)Model_Group_The_Floor_Is_Numeric.IsChecked.Value;
            Data_The_Floor_Is_Numeric.Room_Checked = (bool)Room_The_Floor_Is_Numeric.IsChecked.Value;
            Data_The_Floor_Is_Numeric.Floor_Checked = (bool)Floors_The_Floor_Is_Numeric.IsChecked.Value; 
            if (Parameter_True())
            {
                Record_The_Floor_Is_Numeric record_The_Floor_Is_Numeric1 = new Record_The_Floor_Is_Numeric();
                record_The_Floor_Is_Numeric1.Record_The_Floor_Is_Numeric_Position();
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Запись завершена. Успешно обработаных элементов: " + Data_The_Floor_Is_Numeric.number_True_Element.ToString());
                s_Mistake_String.ShowDialog();
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
        public void Scrol_Viewer1_The_Floor_Is_Numericw()
        {
            scroll_Viewer_Work_Set_All_Model.Content = Data_The_Floor_Is_Numeric.work_Set_Collection;
        }
        public void Scrol_Viewer2_The_Floor_Is_Numericw()
        {
            scroll_Viewer_Work_Set_Ignore.Content = Data_The_Floor_Is_Numeric.work_Set_Igonre_Collection;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Data_The_Floor_Is_Numeric.work_Set_Collection.SelectedItems.Count > 0)
            {
                System.Windows.Controls.ListView listView1 = new System.Windows.Controls.ListView();
                for (int i = 0; i < Data_The_Floor_Is_Numeric.work_Set_Collection.Items.Count; i++)
                {
                    int number = 0;
                    for (int j = 0; j < Data_The_Floor_Is_Numeric.work_Set_Collection.SelectedItems.Count; j++)
                    {
                        if (Data_The_Floor_Is_Numeric.work_Set_Collection.Items[i] == Data_The_Floor_Is_Numeric.work_Set_Collection.SelectedItems[j])
                        {
                            number++;
                        }
                    }
                    if (number == 1)
                    {
                        bool filter_Two = true;
                        foreach (string element in Data_The_Floor_Is_Numeric.work_Set_Igonre_Collection.Items)
                        {
                            if (element == Data_The_Floor_Is_Numeric.work_Set_Collection.Items[i].ToString() && Data_The_Floor_Is_Numeric.work_Set_Igonre_Collection.Items.Count > 0)
                            {
                                filter_Two = false;
                            }
                        }
                        if (filter_Two == true)
                        {
                            Data_The_Floor_Is_Numeric.work_Set_Igonre_Collection.Items.Add(Data_The_Floor_Is_Numeric.work_Set_Collection.Items[i]);
                        }
                    }
                    else
                    {
                        listView1.Items.Add(Data_The_Floor_Is_Numeric.work_Set_Collection.Items[i]);
                    }
                }
                Data_The_Floor_Is_Numeric.work_Set_Collection = listView1;
                Scrol_Viewer1_The_Floor_Is_Numericw();
                Scrol_Viewer2_The_Floor_Is_Numericw();
            }
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Data_The_Floor_Is_Numeric.work_Set_Igonre_Collection.SelectedItems.Count > 0)
            {
                System.Windows.Controls.ListView listView1 = new System.Windows.Controls.ListView();
                for (int i = 0; i < Data_The_Floor_Is_Numeric.work_Set_Igonre_Collection.Items.Count; i++)
                {
                    int number = 0;
                    for (int j = 0; j < Data_The_Floor_Is_Numeric.work_Set_Igonre_Collection.SelectedItems.Count; j++)
                    {
                        if (Data_The_Floor_Is_Numeric.work_Set_Igonre_Collection.Items[i] == Data_The_Floor_Is_Numeric.work_Set_Igonre_Collection.SelectedItems[j])
                        {
                            number++;
                        }
                    }
                    if (number == 1)
                    {
                        Data_The_Floor_Is_Numeric.work_Set_Collection.Items.Add(Data_The_Floor_Is_Numeric.work_Set_Igonre_Collection.Items[i]);
                    }
                    else
                    {
                        listView1.Items.Add(Data_The_Floor_Is_Numeric.work_Set_Igonre_Collection.Items[i]);
                    }
                }
                Data_The_Floor_Is_Numeric.work_Set_Igonre_Collection = listView1;
                Scrol_Viewer1_The_Floor_Is_Numericw();
                Scrol_Viewer2_The_Floor_Is_Numericw();
            }
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
            }
        }
       public bool Parameter_True()
        {
            try
            {
                bool chek = true;
            string not_Valid_Position = "";
            List<Category> categoriesWithParameter = new List<Category>();

            // Получаем BindingMap, чтобы проверить привязки параметров
            BindingMap bindingMap = Revit_Document_The_Floor_Is_Numeric.Document.ParameterBindings;

            DefinitionBindingMapIterator iterator = bindingMap.ForwardIterator();
            while (iterator.MoveNext())
            {
                Definition definition = iterator.Key;

                // Проверяем, совпадает ли имя параметра
                if (definition.Name == "ZH_Этаж_Числовой")
                {
                    ElementBinding binding = iterator.Current as ElementBinding;
                    if (binding != null)
                    {
                        foreach (Category category in binding.Categories)
                        {
                            categoriesWithParameter.Add(category);
                        }
                    }
                }
            }
            if (Data_The_Floor_Is_Numeric.door_Checked)
            {
                bool category_True = false;
                foreach (Category category in categoriesWithParameter)
                {
                    if (category.Name == "Двери")
                    {
                        category_True = true;
                    }

                }
                if (!category_True)
                {
                    chek = false;
                    not_Valid_Position = not_Valid_Position + "\n -параметр не назначен категории 'Двери', обратитесь к BIM координатору";
                }
            }
            if (Data_The_Floor_Is_Numeric.window_Checked)
            {
                bool category_True = false;
                foreach (Category category in categoriesWithParameter)
                {
                    if (category.Name == "Окна")
                    {
                        category_True = true;
                    }

                }
                if (!category_True)
                {
                    chek = false;
                    not_Valid_Position = not_Valid_Position + "\n -параметр не назначен категории 'Окна', обратитесь к BIM координатору";
                }
            }
            if (Data_The_Floor_Is_Numeric.wall_Checked)
            {
                bool category_True = false;
                foreach (Category category in categoriesWithParameter)
                {
                    if (category.Name == "Стены")
                    {
                        category_True = true;
                    }

                }
                if (!category_True)
                {
                    chek = false;
                    not_Valid_Position = not_Valid_Position + "\n -параметр не назначен категории 'Стены', обратитесь к BIM координатору";
                }
            }
            if (Data_The_Floor_Is_Numeric.model_Group_Checked)
            {
                bool category_True = false;
                foreach (Category category in categoriesWithParameter)
                {
                    if (category.Name == "Группы модели")
                    {
                        category_True = true;
                    }

                }
                if (!category_True)
                {
                    chek = false;
                    not_Valid_Position = not_Valid_Position + "\n -параметр не назначен категории 'Группа модели', обратитесь к BIM координатору";
                }
            }
            if (Data_The_Floor_Is_Numeric.Room_Checked)
            {
                bool category_True = false;
                foreach (Category category in categoriesWithParameter)
                {
                    if (category.Name == "Помещения")
                    {
                        category_True = true;
                    }

                }
                if (!category_True)
                {
                    chek = false;
                    not_Valid_Position = not_Valid_Position + "\n -параметр не назначен категории 'Помещения', обратитесь к BIM координатору";
                }
            }
            if (Data_The_Floor_Is_Numeric.Floor_Checked)
            {
                bool category_True = false;
                foreach (Category category in categoriesWithParameter)
                {
                    if (category.Name == "Перекрытия")
                    {
                        category_True = true;
                    }

                }
                if (!category_True)
                {
                    chek = false;
                    not_Valid_Position = not_Valid_Position + "\n -параметр не назначен категории 'Перекрытия', обратитесь к BIM координатору";
                }
            }
            if (chek == false)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Процесс запуска прерван по следующим ошибкам: " + not_Valid_Position);
                s_Mistake_String.ShowDialog();
            }
            return chek;
            }
            catch (Exception ex)
            {
                S_Mistake_String s_Mistake_String = new S_Mistake_String("Ошибка. " + ex.Message);
                s_Mistake_String.ShowDialog();
                return true;
            }
        }

    }
}
