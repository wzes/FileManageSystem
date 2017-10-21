using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FileManageSystem
{
    public class IsFolder : IValueConverter
    {
       
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            ObservableCollection<File> files = (ObservableCollection<File>)value;
            //ObservableCollection<File> m = new ObservableCollection<File>();
            //for (int i = 0; i < files.Count; i++)
            //{
            //    m.Add(files[i]);
            //}
            //bool b = false;
            for(int i = 0; i < files.Count; i++)
            {
                if (files[i].Type.Equals("Folder"))
                {
                    //b = true;
                    return value;
                    //for(int j = 0; j < files[i].SubFiles.Count; j++)
                    //{
                    //    if (files[i].SubFiles[j].Type.Equals("File"))
                    //    {
                    //        files[i].SubFiles.Remove(files[i].SubFiles[j]);
                    //    }
                   // }
                }
                
            }
            return null;
           // return (ObservableCollection<File>)value;
        }
    }
}
