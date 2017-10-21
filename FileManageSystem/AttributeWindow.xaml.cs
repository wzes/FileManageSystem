using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FileManageSystem
{
    /// <summary>
    /// AttributeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AttributeWindow : Window
    {
        ListBox listBox;
        public AttributeWindow(ListBox listBox, bool IsItems, System.Windows.Point point)
        {
            InitializeComponent();
            this.listBox = listBox;
            System.Windows.Point p = point;
            //根据屏幕显示器会有所改变
            Window win = Application.Current.MainWindow;
            PresentationSource source = PresentationSource.FromVisual(win);
            Matrix m = source.CompositionTarget.TransformFromDevice;
            double widthRatio = m.M11;
            double heightRatio = m.M22;
            this.Top = p.Y * heightRatio;
            this.Left = p.X * widthRatio;
            if (IsItems)
            {
                new_item.IsEnabled = false;
            }
            else
            {
                open_item.IsEnabled = false;
                delete_item.IsEnabled = false;
                copy_item.IsEnabled = false;
                cut_item.IsEnabled = false;
                rename_item.IsEnabled = false;
            }
            if (CategoryManage.IsCopy || CategoryManage.IsCut)
            {
                stick_item.IsEnabled = true;
            }
            else
            {
                stick_item.IsEnabled = false;
            }
        }

        private void CalculateScreenSize()
        {
            Window win = Application.Current.MainWindow;

            PresentationSource source = PresentationSource.FromVisual(win);

            Matrix m = source.CompositionTarget.TransformFromDevice;

            double widthRatio = m.M11;

            double heightRatio = m.M22;

            double screenWidth = SystemParameters.PrimaryScreenWidth * widthRatio;

            double screenHeight = SystemParameters.PrimaryScreenHeight * heightRatio;


        }
        //
        public void click(object sender, RoutedEventArgs e)
        {
            int si = listBox_attribution.SelectedIndex;
            if(si != -1)
            {
                switch (si)
                {
                    case 0:
                        open_click();
                        break;
                    case 1:
                        this.DialogResult = true;  //取消
                        break;
                    case 3:
                        copy_click();
                        break;
                    case 4:
                        cut_click();
                        break;
                    case 5:
                        cut_click();
                        break;
                    case 6:
                        rename_click();
                        break;
                    case 7:
                        delete_click();
                        break;
                    case 8:
                        new_file_click();
                        break;
                    case 10:
                        detains_click();
                        break;
                }
            }
            
        }
        //
        public void new_file_click()
        {
            if (CategoryManage.CurrentFile.Type.Equals("Folder"))
            {
                File file = new File();
                string name = "未命名_" + CategoryManage.findCategory();
                file.Name = name;

                file.Path = CategoryManage.CurrentFile.Path + "/" + name;
                file.Date = DateTime.Now.ToString();
                file.Disk_start = -1;
                file.Size = 0;
                file.Type = "File";
                CategoryManage.listFiles.Add(file);
                CategoryManage.Update();
            }
            this.DialogResult = true;

        }
        //
        public void detains_click()
        {
            if (CategoryManage.selectedFile != null)
            {
                new DetailWindow(CategoryManage.selectedFile).Show();
            }
            else
            {
                new DetailWindow(CategoryManage.Create).Show();
            }
            this.DialogResult = true;
        }
        //
        public void copy_click()
        {
            if (CategoryManage.selectedFile != null)
            {
                File file = new File();
                file.Name = CategoryManage.selectedFile.Name;
                file.Path = CategoryManage.selectedFile.Path;
                file.Date = CategoryManage.selectedFile.Date;
                file.Size = CategoryManage.selectedFile.Size;
                file.Disk_start = CategoryManage.selectedFile.Disk_start;
                file.Occupation_space = CategoryManage.selectedFile.Occupation_space;
                file.Type = CategoryManage.selectedFile.Type;
                file.SubFiles = CategoryManage.selectedFile.SubFiles;
                CategoryManage.copy_file = file;
                CategoryManage.IsCopy = true;
               
            }
            this.DialogResult = true;
        }
        //
        public void open_click()
        {
            File file = CategoryManage.selectedFile;
            if (file.Type.Equals("Folder"))
            {
                CategoryManage.listFiles = file.SubFiles;
                listBox.ItemsSource = CategoryManage.listFiles;
            }
            else if (file.Type.Equals("File"))
            {
                new EditWindow().Show();
            }
            this.DialogResult = true;
        }
        //
        public void stick_click()
        {
            if (CategoryManage.IsCut)
            {
                //删除
                CategoryManage.delete(CategoryManage.Create, CategoryManage.cut_file.Path);

                CategoryManage.cut_file.Path = CategoryManage.CurrentFile.Path + "/" + CategoryManage.cut_file.Name;
                CategoryManage.cut_file.Date = DateTime.Now.ToString();
                CategoryManage.listFiles.Add(CategoryManage.cut_file);
                CategoryManage.IsCut = false;
                CategoryManage.Update();
            }
            else if (CategoryManage.IsCopy)
            {
                //复制
                if (CategoryManage.copy_file != null)
                {
                    string str = DiskManage.getStrContent(CategoryManage.copy_file.Disk_start, CategoryManage.copy_file.Size);
                    int size = 0;
                    int start = DiskManage.saveContent(str, ref size);
                    CategoryManage.copy_file.Disk_start = start;
                    CategoryManage.copy_file.Size = size;
                    CategoryManage.copy_file.Date = DateTime.Now.ToString();

                    CategoryManage.listFiles.Add(CategoryManage.copy_file);
                    CategoryManage.Update();
                }
            }

            this.DialogResult = true;
        }
        //
        public void cut_click()
        {
            if (CategoryManage.selectedFile != null)
            {
                File file = new File();
                file.Name = CategoryManage.selectedFile.Name;
                file.Path = CategoryManage.selectedFile.Path;
                file.Date = CategoryManage.selectedFile.Date;
                file.Size = CategoryManage.selectedFile.Size;
                file.Disk_start = CategoryManage.selectedFile.Disk_start;
                file.Occupation_space = CategoryManage.selectedFile.Occupation_space;
                file.Type = CategoryManage.selectedFile.Type;
                file.SubFiles = CategoryManage.selectedFile.SubFiles;
                CategoryManage.copy_file = file;
                CategoryManage.IsCut = true;
                CategoryManage.Update();
            }
            this.DialogResult = true;
        }
        //
        public void delete_click()
        {
            CategoryManage.listFiles.Remove(CategoryManage.selectedFile);


            CategoryManage.Update();

            this.DialogResult = true;
        }
        //
        public async void rename_click()
        {
            if (CategoryManage.CurrentFile != null)
            {
                RenameControl a = new RenameControl();
                await DialogHost.Show(a);
                this.DialogResult = true;
                CategoryManage.Update();
            }
        }
    }
}
