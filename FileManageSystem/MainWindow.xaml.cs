using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FileManageSystem
{


    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public string[] filenames;
        public bool showDialog = false;
        public Point point;


        public MainWindow()
        {
            InitializeComponent();
            init();
            pathView.Text = "CxtDisk";
            //initData();
            bottom_space.Text = "总空间5120KB 已用" + CategoryManage.total_size_occupation / 1024 + " KB 剩余 " + (5120 - Math.Round(CategoryManage.total_size_occupation / 1024.0)) + "KB";
        }

       
        //初始化数据
        public void initData()
        {
            string[] filenames = { "DB", "Android", "Unity", "VS", "Python", "Ruby", "Scrapy", "Bootstrap" };
            CategoryManage.root = new ObservableCollection<File>();

            //新建一个目录
            File file0 = new File();
            file0.Name = "CxtDisk";
            file0.Path = "CxtDisk";
            file0.Date = DateTime.Now.ToString();
            file0.Size = 0;
            file0.Disk_start = -1;
            file0.Type = "Folder";

            //第一层
            for (int i = 0; i < filenames.Length; i++)
            {
                File mfile = new File();
                mfile.Name = filenames[i];
                mfile.Path = "CxtDisk/" + filenames[i];
                mfile.Date = DateTime.Now.ToString();
                mfile.Size = 0;
                mfile.Disk_start = -1;
                mfile.Type = "Folder";
                file0.SubFiles.Add(mfile);

                File file = new File();
                file.Name = "Homework";
                file.Path = mfile.Path + "/Homework";
                file.Date = DateTime.Now.ToString();
                file.Size = 0;
                file.Disk_start = -1;
                file.Type = "Folder";
                mfile.SubFiles.Add(file);

                File file3 = new File();
                file3.Name = "file" + i;
                file3.Path = file.Path+ "/file" + i;
                file3.Date = DateTime.Now.ToString();
                file3.Size = 0;
                file3.Disk_start = -1;
                file3.Type = "File";
                file.SubFiles.Add(file3);

            }
            

            CategoryManage.root.Add(file0);


            if(CategoryManage.root != null)
            {
                CategoryManage.listFiles = CategoryManage.root[0].SubFiles;
            }
            else
            {
                CategoryManage.listFiles = CategoryManage.root;
            }
            listBox.ItemsSource = CategoryManage.listFiles;
            treeView.ItemsSource = CategoryManage.root;


            CategoryManage.init();
            DiskManage.init();
            //DiskManage.save();
            //CategoryManage.save();
            CategoryManage.Update();
        }
        //初始化内存，恢复数据
        public void init()
        {
            CategoryManage.init();
            DiskManage.init();
            listBox.ItemsSource = CategoryManage.listFiles;
            treeView.ItemsSource = CategoryManage.root;
            //treeView.ItemsSource = CategoryManage.treeFiles;
        }
        //
        
        //右键监听
        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            MainWindow.POINT pit = new MainWindow.POINT();
            MainWindow.GetCursorPos(out pit);   //获取鼠标绝对位置
           
            StringBuilder sbAbs = new StringBuilder();
            point = new Point(pit.X, pit.Y);  

            if (!showDialog)
            {
                if (listBox.SelectedIndex != -1)
                {
                    No_Select_State();
                    AttributeWindow a = new AttributeWindow(listBox, false, point);
                    a.ShowDialog();
                    showDialog = false;
                }
                else if (treeView.SelectedItem != null)
                {
                    No_Select_State();
                    
                    AttributeWindow a = new AttributeWindow(listBox, false, point);
                    a.ShowDialog();
                }
                else
                {
                    No_Select_State();
                    AttributeWindow a = new AttributeWindow(listBox,false, point);
                    a.ShowDialog();
                }
            }
            else
            {
                showDialog = false;
            }
        }
        //改变item的选中状态
        private void TextBlock_MouseMove(object sender, MouseEventArgs e)
        {
            TextBlock Item = sender as TextBlock;
            listBox.SelectedItem = Item.DataContext;
            //选中item
            Select_State();
            CategoryManage.selectedFile = (File)listBox.SelectedItem;
            //File file = (File)listBox.SelectedItem;
            //pathView.Text = file.Path;
            //CategoryManage.CurrentFile = file;
        }
        //获取焦点
        void listBox_onMouseEnterItem(object sender, RoutedEventArgs e)
        {
            listBox.Focus();
        }
        //选中文件的状态
        public void Select_State()
        {
            copy.IsEnabled = true;
            cut.IsEnabled = true;
            open.IsEnabled = true;
            rename.IsEnabled = true;
            delete.IsEnabled = true;
            new_file.IsEnabled = false;
            new_folder.IsEnabled = false;
            if (CategoryManage.IsCopy || CategoryManage.IsCut)
            {
                stick.IsEnabled = true;
            }
            else
            {
                stick.IsEnabled = false;
            }
        }
        //没选中文件得分状态
        public void No_Select_State()
        {
            copy.IsEnabled = false;
            cut.IsEnabled = false;
            open.IsEnabled = false;
            rename.IsEnabled = false;
            delete.IsEnabled = false;
            new_file.IsEnabled = true;
            new_folder.IsEnabled = true;
            if (CategoryManage.IsCopy || CategoryManage.IsCut)
            {
                stick.IsEnabled = true;
            }
            else
            {
                stick.IsEnabled = false;
            }
        }
        //打开树目录结构
        public void treeView_open(object sender, RoutedEventArgs e)
        {
            File file = (File)treeView.SelectedItem;
            pathView.Text = file.Path;
            CategoryManage.CurrentFile = file;
            CategoryManage.LeftStack.Push(file);

            //File mfile = CategoryManage.getFiles(file, CategoryManage.Create);
            //CategoryManage.CurrentFile = mfile;


            if (file.Type.Equals("Folder"))
            {
                No_Select_State();
                CategoryManage.listFiles = file.SubFiles;
                listBox.ItemsSource = CategoryManage.listFiles;
                //CategoryManage.listFiles = mfile.SubFiles;
                //listBox.ItemsSource = CategoryManage.listFiles;
            }
            else
            {
                Select_State();
            }
        }
        //文件目录右键
        private void listBox_right_click(object sender, MouseButtonEventArgs e)
        {
            MainWindow.POINT pit = new MainWindow.POINT();
            MainWindow.GetCursorPos(out pit);   //获取鼠标绝对位置
            point = new Point(pit.X, pit.Y);

            AttributeWindow a = new AttributeWindow(listBox, true, point);
            a.ShowDialog();

            showDialog = true;
        }
        //查找
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            CategoryManage.searchlistFiles = new ObservableCollection<File>();
            string key = search.Text;
            if (key.Equals("")) return;
            if(CategoryManage.CurrentFile!= null)
            {
                SearchFile(CategoryManage.CurrentFile, key);
            }
            //如果没找到则全文搜索
            if(CategoryManage.searchlistFiles.Count == 0)
            {
                for (int i = 0; i < CategoryManage.files.Count(); i++)
                {
                    if (CategoryManage.files[i] == null) break;
                    if (CategoryManage.files[i].Type.Equals("File") && CategoryManage.files[i].Name.Equals(key))
                    {
                        CategoryManage.searchlistFiles.Add(CategoryManage.files[i]);
                    }
                }
            }
            listBox.ItemsSource = CategoryManage.searchlistFiles;
        }
        //递归当前文件夹搜索
        public void SearchFile(File file, string name)
        {
            if(file != null && file.SubFiles != null)
            {
                for( int i = 0; i < file.SubFiles.Count; i++)
                {
                    if (file.SubFiles[i].Name.Equals(name))
                    {
                        CategoryManage.searchlistFiles.Add(file.SubFiles[i]);
                    }
                    else
                    {
                        SearchFile(file.SubFiles[i], name);
                    }
                }
                
            }
        }
        //双击打开文件或文件夹
        private void listBox_click_openFile(object sender, RoutedEventArgs e)
        {
            File file = (File)listBox.SelectedItem;
            CategoryManage.CurrentFile = file;

            CategoryManage.LeftStack.Push(file);

            if (file.Type.Equals("Folder"))
            {
                CategoryManage.listFiles = file.SubFiles;
                listBox.ItemsSource = CategoryManage.listFiles;
            }
            else if (file.Type.Equals("File"))
            {
                new EditWindow().Show();
            }
            pathView.Text = file.Path;
        }
        //向左
        private void Click_Left(object sender, RoutedEventArgs e)
        {
            CategoryManage.RightStack.Push(CategoryManage.CurrentFile);
            File file = null;
            if (CategoryManage.LeftStack.Count > 0)
            {
                file = CategoryManage.LeftStack.Pop();
            }
            while (CategoryManage.LeftStack.Count > 0 &&
                 file == CategoryManage.CurrentFile)
            {
                file = CategoryManage.LeftStack.Pop();
            }
            if(file != null)
            {
                CategoryManage.CurrentFile = file;
                CategoryManage.listFiles = file.SubFiles;
                listBox.ItemsSource = CategoryManage.listFiles;
            }
        }
        //向右
        private void Click_Right(object sender, RoutedEventArgs e)
        {
            CategoryManage.LeftStack.Push(CategoryManage.CurrentFile);
            File file = null;
            if (CategoryManage.RightStack.Count > 0)
            {
                file = CategoryManage.RightStack.Pop();
            }
            while (CategoryManage.RightStack.Count > 0 &&
                 file == CategoryManage.CurrentFile)
            {
                file = CategoryManage.RightStack.Pop();
            }
            if (file != null)
            {
                CategoryManage.CurrentFile = file;
                CategoryManage.listFiles = file.SubFiles;
                listBox.ItemsSource = CategoryManage.listFiles;
            }
           
        }
        //上一层目录
        private void Click_Up(object sender, RoutedEventArgs e)
        {
            File file = CategoryManage.FindParent(CategoryManage.Create, CategoryManage.CurrentFile.Path);
            if (file != null)
            {
                CategoryManage.CurrentFile = file;
                CategoryManage.listFiles = file.SubFiles;
                listBox.ItemsSource = CategoryManage.listFiles;
            }
        }
        //新建文件夹
        private void Click_NewFolder(object sender, RoutedEventArgs e)
        {
            File file = new File();
            string name = "未命名_" + CategoryManage.findCategory();
            file.Name = name;
            file.Disk_start = -1;
            file.Path = CategoryManage.CurrentFile.Path + "/" + name;
            file.Date = DateTime.Now.ToString();
            file.Size = 0;
            file.Type = "Folder";
            CategoryManage.listFiles.Add(file);
            CategoryManage.Update();
        } 
        //重命名
        private async void Click_Rename(object sender, RoutedEventArgs e)
        {
            if (CategoryManage.CurrentFile != null)
            {
                RenameControl a = new RenameControl();
                await DialogHost.Show(a);
            }
        }
        //复制
        private void Click_Copy(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                CategoryManage.CurrentFile = (File)listBox.SelectedItem;
                File file = new File();
                file.Name = CategoryManage.CurrentFile.Name;
                file.Path = CategoryManage.CurrentFile.Path;
                file.Date = CategoryManage.CurrentFile.Date;
                file.Size = CategoryManage.CurrentFile.Size;
                file.Disk_start = CategoryManage.CurrentFile.Disk_start;
                file.Occupation_space = CategoryManage.CurrentFile.Occupation_space;
                file.Type = CategoryManage.CurrentFile.Type;
                file.SubFiles = CategoryManage.CurrentFile.SubFiles;
                CategoryManage.copy_file = file;
                CategoryManage.IsCopy = true;
            }

        }
        //粘贴
        private void Click_Stick(object sender, RoutedEventArgs e)
        {
            //剪切
            if (CategoryManage.IsCut)
            {
                //删除
                CategoryManage.delete(CategoryManage.Create, CategoryManage.cut_file.Path);
                CategoryManage.cut_file.Path = CategoryManage.CurrentFile.Path + "/"+CategoryManage.cut_file.Name;
                CategoryManage.cut_file.Date = DateTime.Now.ToString();
                CategoryManage.listFiles.Add(CategoryManage.cut_file);
                CategoryManage.IsCut = false;
                CategoryManage.Update();
            }
            //复制
            else if (CategoryManage.IsCopy)
            {
                if(CategoryManage.copy_file != null)
                {

                    string str = DiskManage.getStrContent(CategoryManage.copy_file.Disk_start, CategoryManage.copy_file.Size);
                    int size = 0;
                    int start = DiskManage.saveContent(str, ref size);
                    CategoryManage.copy_file.Disk_start = start;
                    CategoryManage.copy_file.Size = size;

                    CategoryManage.copy_file.Path = CategoryManage.CurrentFile.Path + "/" + CategoryManage.copy_file.Name;
                    CategoryManage.copy_file.Date = DateTime.Now.ToString();

                    CategoryManage.listFiles.Add(CategoryManage.copy_file);
                    CategoryManage.Update();
                }
            }
           
        }
        //剪切
        private void Click_Cut(object sender, RoutedEventArgs e)
        {
            if (CategoryManage.CurrentFile != null)
            {
                File file = new File();
                file.Name = CategoryManage.CurrentFile.Name;
                file.Path = CategoryManage.CurrentFile.Path;
                file.Date = CategoryManage.CurrentFile.Date;
                file.Size = CategoryManage.CurrentFile.Size;
                file.Disk_start = CategoryManage.CurrentFile.Disk_start;
                file.Occupation_space = CategoryManage.CurrentFile.Occupation_space;
                file.Type = CategoryManage.CurrentFile.Type;
                file.SubFiles = CategoryManage.CurrentFile.SubFiles;
                CategoryManage.cut_file = file;
                CategoryManage.IsCut = true;
                CategoryManage.Update();

            }
        }
        //新建文件
        private void Click_NewFile(object sender, RoutedEventArgs e)
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
        //属性
        private void Click_Detains(object sender, RoutedEventArgs e)
        {
            if(listBox.SelectedItem != null)
            {
                CategoryManage.CurrentFile = (File)listBox.SelectedItem;
            }
            if (CategoryManage.CurrentFile != null)
            {
                new DetailWindow(CategoryManage.CurrentFile).Show();
            }
            else
            {
                new DetailWindow(CategoryManage.Create).Show();
            }
        }
        //打开
        private void Click_Open(object sender, RoutedEventArgs e)
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
        }
        //格式化
        private async void Click_Clear(object sender, RoutedEventArgs e)
        {
            FormatControl a = new FormatControl();
            await DialogHost.Show(a);
            listBox.ItemsSource = CategoryManage.listFiles;
        }
        //删除文件
        private void Click_Delete(object sender, RoutedEventArgs e)
        {
            if(listBox.SelectedItem != null)
            {
                CategoryManage.listFiles.Remove((File)listBox.SelectedItem);
                CategoryManage.Update();
            }
        }
        //获取位置
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);

    }
}
