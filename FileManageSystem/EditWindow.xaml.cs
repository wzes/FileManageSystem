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
using System.Windows.Shapes;

namespace FileManageSystem
{
    /// <summary>
    /// EditWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditWindow : Window
    {
        public EditWindow()
        {
            InitializeComponent();
            text_content.Text = DiskManage.getStrContent(CategoryManage.selectedFile.Disk_start, CategoryManage.selectedFile.Size);
        }

        public void save_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            int size = 0;
            DiskManage.deleteFile(CategoryManage.selectedFile.Disk_start);  //删除原本磁盘
            int start = DiskManage.saveContent(text_content.Text, ref size);
            CategoryManage.selectedFile.Disk_start = start;
            CategoryManage.selectedFile.Size = size;
            CategoryManage.selectedFile.Occupation_space = (int)Math.Ceiling(Convert.ToDouble(size / 512.0)) * 512;
            CategoryManage.selectedFile.Date = DateTime.Now.ToString();
            CategoryManage.Update();
        }
    }
}
