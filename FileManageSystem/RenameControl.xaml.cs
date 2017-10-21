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

namespace FileManageSystem
{
    /// <summary>
    /// RenameControl.xaml 的交互逻辑
    /// </summary>
    public partial class RenameControl : UserControl
    {
        public RenameControl()
        {
            InitializeComponent();
            rename_textbox.Text = CategoryManage.selectedFile.Name;
        }

        void save_click(object sender, RoutedEventArgs e)
        {
            if (!rename_textbox.Text.Equals(""))
            {
                if(rename_textbox.Text.Length > 19)
                {
                    MessageBox.Show("文件名过长！");
                }
                else if(!CategoryManage.IsSameName(CategoryManage.selectedFile, rename_textbox.Text))
                {
                    CategoryManage.selectedFile.Name = rename_textbox.Text;
                    CategoryManage.selectedFile.Path = CategoryManage.selectedFile.Path.Substring(0, CategoryManage.selectedFile.Path.LastIndexOf('/')+1) +
                       rename_textbox.Text;
                    CategoryManage.selectedFile.Date = DateTime.Now.ToString();
                    CategoryManage.Update();
                }
                else
                {
                    MessageBox.Show("文件名已存在！");
                }
            }
        }
        void cancle_click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
