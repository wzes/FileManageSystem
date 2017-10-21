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
    /// FileDetailWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DetailWindow : Window
    {
        public DetailWindow(File file)
        {
            InitializeComponent();
            showFile(file);
            this.Title = file.Name + " 属性";
        }
        //显示文件
        public void showFile(File file)
        {
            detains_name.Text = file.Name;
            detains_size.Text = file.Size+ " B";
            detains_op_size.Text = file.Occupation_space + " B";

            detains_path.Text = file.Path;
            detains_date.Text = file.Date;
            detains_type.Text = file.Type;
            if(file.Authority == File.FileAuthority.OR)
            {
                detains_is_read.IsChecked = true;
                detains_is_write.IsChecked = false;
            }
            else
            {
                detains_is_read.IsChecked = false;
                detains_is_write.IsChecked = true;
            }
        }
    }
}
