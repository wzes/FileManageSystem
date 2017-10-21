using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManageSystem
{
    //192个字节
    public class File : INotifyPropertyChanged
    {
        public enum FileAuthority { OR, M, H };    //文件权限 ， 1.只读    2. 可改(包括删除） 3. 隐藏
        private string name;                //文件名        40个字节
        private int id;                     //标识符        4个字节
        private string type;                //文件类型      4个字节
        private FileAuthority authority;    //文件权限      4个字节
        private string path;                //文件位置      100个字节
        private string date;                //文件修改日期    20个字节
        private int disk_start;             //起始位置      4个字节 
        private int file_number;            //文件数量      4个字节
        private int folder_number;          //文件夹数量    4个字节
        private int occupation_space;       //占用空间      4个字节
        private int size;                   //文件大小      4个字节
        private ObservableCollection<File> subFiles = new ObservableCollection<File>();
        

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotiFy(string property)
        {
            PropertyChangedEventHandler PropertyChanged = this.PropertyChanged;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                NotiFy("Name");
            }
        }
        
        public FileAuthority Authority
        {
            get
            {
                return authority;
            }

            set
            {
                authority = value;
                NotiFy("Authority");
            }
        }

        public string Path
        {
            get
            {
                return path;
            }

            set
            {
                path = value;
                NotiFy("Path");
            }
        }

        public string Date
        {
            get
            {
                return date;
            }

            set
            {
                date = value;
                NotiFy("Date");
            }
        }

        public int Disk_start
        {
            get
            {
                return disk_start;
            }

            set
            {
                disk_start = value;
                NotiFy("Disk_start");
            }
        }

        public int File_number
        {
            get
            {
                return file_number;
            }

            set
            {
                file_number = value;
                NotiFy("File_number");
            }
        }

        public int Folder_number
        {
            get
            {
                return folder_number;
            }

            set
            {
                folder_number = value;
                NotiFy("Folder_number");
            }
        }

        public int Occupation_space
        {
            get
            {
                return occupation_space;
            }

            set
            {
                occupation_space = value;
                NotiFy("Occupation_space");
            }
        }

        public int Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
                NotiFy("Size");
            }
        }

        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
                NotiFy("Type");
            }
        }

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
                NotiFy("Id");
            }
        }

        public ObservableCollection<File> SubFiles
        {
            get
            {
                return subFiles;
            }

            set
            {
                subFiles = value;
                NotiFy("SubFiles");
            }
        }
    }
}
