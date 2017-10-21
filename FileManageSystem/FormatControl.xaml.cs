using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace FileManageSystem
{
    /// <summary>
    /// RenameControl.xaml 的交互逻辑
    /// </summary>
    public partial class FormatControl : UserControl
    {
        public FormatControl()
        {
            InitializeComponent();
        }

        void sure_click(object sender, RoutedEventArgs e)
        {
            CategoryManage.Create.SubFiles = new ObservableCollection<File>();
            CategoryManage.Create.Date = DateTime.Now.ToString();
            CategoryManage.Create.Name = "CxtDisk";
            CategoryManage.Create.Path = "CxtDisk";
            CategoryManage.Create.Size = 0;
            CategoryManage.Create.Occupation_space = 0;
            CategoryManage.listFiles = CategoryManage.Create.SubFiles;
            CategoryManage.CurrentFile = CategoryManage.Create;
            DiskManage.clearDisk();
            CategoryManage.Update();
            
        }
        void cancle_click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
