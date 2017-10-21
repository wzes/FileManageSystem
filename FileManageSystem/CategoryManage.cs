using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManageSystem
{
    public static class CategoryManage
    {
        public static int disk_size = 100 * 1024;  //磁盘大小
        public static int block_size = 192;      //每一块大小
        public static int block_number = disk_size / block_size;      //每一块大小
        public static char[] bitMap = new char[block_number];            //位向量
        public static Block[] blocks = new Block[block_number];
        public static File[] files = new File[block_number];

        public static ObservableCollection<File> root;     //显示列表
        public static ObservableCollection<File> listFiles;
        public static ObservableCollection<File> searchlistFiles;   //查找结果列表
        public static ObservableCollection<File> treeFiles;     //显示列表

        
        public static Stack<File> LeftStack = new Stack<File>();
        public static Stack<File> RightStack = new Stack<File>();
        public static File cut_file;

        public static bool IsCopy = false;
        public static bool IsCut = false;
        public static File copy_file;

        public static File selectedFile;

        public static File Create;
        public static File CurrentFile;
        public static string CurrentPath;
        public static int total_size = 0;
        public static int total_size_occupation = 0;
        


        public static File[] Files
        {
            get
            {
                return files;
            }

            set
            {
                files = value;
            }
        }

        //从磁盘读回数据
        public static void init()
        {
            Create = new File();
            root = new ObservableCollection<File>();
            treeFiles = new ObservableCollection<File>();

            readDisk();
            creatCategory();
            listFiles = Create.SubFiles;
            CurrentFile = root[0];
            updateSize();

            //File mfile = new File();
            //mfile.Name = Create.Name;
            //mfile.Path = Create.Path;
            //mfile.Date = Create.Date;
            //mfile.Size = Create.Size;
            //mfile.Disk_start = Create.Disk_start;
            //mfile.Occupation_space = Create.Occupation_space;
            //mfile.Type = Create.Type;
            //treeFiles.Add(mfile);
            //createTreeView(treeFiles[0], Create);
        }
        //更新目录
        public static void Update()
        {
            DiskManage.save();
            CategoryManage.save();
            CategoryManage.readDisk();
            CategoryManage.updateSize();
            //createTreeView(treeFiles[0], Create);
        }
        //从磁盘恢复目录
        public static void readDisk()
        {
            FileStream fs = new FileStream("CategoryDisk.dat", FileMode.OpenOrCreate, FileAccess.Read);
            int counter = 0;
            byte[] result = new byte[192];
            while (fs.Read(result, 0, 192) != 0)
            {
                File file = bytesToFile(result);  //恢复文件目录
                Files[counter++] = file;
            }
            fs.Close();
        }
        //更新大小
        public static void updateSize()
        {
            total_size = 0;
            total_size_occupation = 0;
            ClearCategory(Create);  //
            for (int i = 0; i < files.Count(); i++)
            {
                if (files[i] != null && files[i].Type.Equals("File"))
                {
                    int size = files[i].Size;
                    int occu = files[i].Occupation_space;
                    total_size_occupation += occu;
                    total_size += size;
                    string path = files[i].Path;
                    UpdateSize(Create, path, ref size);
                }
                else if (files[i] != null && files[i].Type.Equals("Folder"))
                {
                    files[i].Size = 0;
                    files[i].Occupation_space = 0;
                }
                else if(files[i] == null)
                {
                    break;
                }
            }
            Create.Size = total_size;
            Create.Occupation_space = total_size_occupation;
        }
        //通过files 数组恢复目录结构！
        public static void creatCategory()
        {
            Create.Name = files[0].Name;
            Create.Type = files[0].Type;
            Create.Size = files[0].Size;
            Create.Path = files[0].Path;
            Create.Date = files[0].Date;
            Create.Disk_start = files[0].Disk_start;
            root.Add(Create);
            for (int i = 1; i < files.Count(); i++)
            {
                if (files[i] != null)
                {
                    string path = files[i].Path;
                    //存在
                    if (findByName(Create, path) != null)
                    {
                        File file = new File();
                        file.Name = files[i].Name;
                        file.Path = files[i].Path;
                        file.Date = files[i].Date;
                        if(files[i].Type.Equals("Folder"))
                        {
                            file.Size = 0;
                        }
                        else
                        {
                            file.Size = files[i].Size;
                            file.Occupation_space = (int)Math.Ceiling(Convert.ToDouble(file.Size / 512.0)) * 512;
                        }
                        file.Disk_start = files[i].Disk_start;
                        file.Occupation_space = files[i].Occupation_space;
                        file.Type = files[i].Type;
                        findByName(Create, path).SubFiles.Add(file);
                    }
                }
                else
                {
                    break;
                }
            }
        }
        //找文件
        public static int findCategory()
        {
            bool get = false;
            int index = 0;
            if (listFiles != null)
            {
                for (; index < listFiles.Count(); index++)
                {
                    for (int j = 0; j < listFiles.Count(); j++)
                    {
                        if (listFiles[j].Name.IndexOf("未命名_" + index) != -1) //有相同的
                        {
                            get = true;
                        }
                    }
                    if (!get)
                    {
                        return index;
                    }
                    else
                    {
                        get = false;
                    }

                }
            }
            if (!get)
            {
                return index;
            }
            return 0; 
        }
        //创建目录结构
        public static void createTreeView(File root, File file)
        {
            if (file != null)
            {
                if (file.SubFiles != null)
                {
                    root.SubFiles = new ObservableCollection<File>();
                    for (int index = 0; index < file.SubFiles.Count(); index++)
                    {
                        if (file.SubFiles[index].Type.Equals("Folder"))
                        {
                            File mfile = new File();
                            mfile.Name = file.SubFiles[index].Name;
                            mfile.Path = file.SubFiles[index].Path;
                            mfile.Date = file.SubFiles[index].Date;
                            mfile.Size = file.SubFiles[index].Size;
                            mfile.Disk_start = file.SubFiles[index].Disk_start;
                            mfile.Occupation_space = file.SubFiles[index].Occupation_space;
                            mfile.Type = file.SubFiles[index].Type;
                            root.SubFiles.Add(mfile);
                            createTreeView(root.SubFiles[index], file.SubFiles[index]);
                        }
                    }
                }
            }
        }
        //获取
        public static File getFiles(File root, File file)
        {
            if (file != null)
            {
                if (root.Path.Equals(file.Path))
                {
                    return file;
                }

                if (file.SubFiles != null)
                {
                    for (int index = 0; index < file.SubFiles.Count(); index++)
                    {
                        if(getFiles(root, file.SubFiles[index]) != null)
                        {
                            return getFiles(root, file.SubFiles[index]);
                        }
                    }
                }
            }
            return null;
        }
        //删除
        public static void delete(File file, string path)
        {
            if (file != null && file.SubFiles != null)
            {
                for (int index = 0; index < file.SubFiles.Count(); index++)
                {
                    if (file.SubFiles[index].Path.Equals(path))
                    {
                        DiskManage.deleteFile(file.SubFiles[index].Disk_start);
                        file.SubFiles.Remove(file.SubFiles[index]);
                    }
                    else
                    {
                        delete(file.SubFiles[index], path);
                    }
                }
            }
            
        }
        //重置
        public static void ClearCategory(File file)
        {
            if (file != null)
            {
                if (file.SubFiles != null)
                {
                    for (int index = 0; index < file.SubFiles.Count(); index++)
                    {
                        if (file.SubFiles[index].Type.Equals("Folder"))
                        {
                            file.SubFiles[index].Size = 0;
                            ClearCategory(file.SubFiles[index]);
                        }
                    }
                }
            }
        }
        //找到父文件
        public static File FindParent(File file, string path)
        {
            if (file != null && file.SubFiles != null)
            {
                for (int index = 0; index < file.SubFiles.Count(); index++)
                {
                    string[] array = path.Split('/');
                    string filename = file.SubFiles[index].Name;
                    if (array.Count() < 2)
                    {
                        return Create;
                    }
                    if (filename.Equals(array[1]))
                    {
                        //找到文件夹 
                        if (array.Count() == 2)
                        {
                            if (filename.Equals(array[1]))
                            {
                                return file;
                            }
                        }
                        //继续深入查找
                        else if (array.Count() > 2)
                        {
                            return FindParent(file.SubFiles[index], path.Substring(path.IndexOf('/') + 1));
                        }
                    }
                }
            }
            return null;
        }
        //是否同名
        public static bool IsSameName(File file, string name)
        {
            if (file != null)
            {
                for (int index = 0; index < file.SubFiles.Count(); index++)
                {
                    if (file.SubFiles[index].Name.Equals(name))
                    {
                        return true;
                    }
                   
                }
                return false;
            }
            return false;
        }
        //更新大小
        public static void UpdateSize(File file, string path, ref int size)
        {
            if (file != null && file.SubFiles != null)
            {
                for (int index = 0; index < file.SubFiles.Count(); index++)
                {
                    string[] array = path.Split('/');
                    string filename = file.SubFiles[index].Name;
                    if (filename.Equals(array[1]))
                    {
                        //找到文件夹 
                        if (array.Count() == 2)
                        {
                            if (filename.Equals(array[0]))
                            {
                                file.SubFiles[index].Size += size;
                                file.SubFiles[index].Occupation_space += (int)Math.Ceiling(Convert.ToDouble(size / 512.0)) * 512;
                                size = 0;
                                return;
                            }
                        }
                        //继续深入查找
                        else if (array.Count() > 2)
                        {
                            file.SubFiles[index].Size += size;
                            file.SubFiles[index].Occupation_space += file.SubFiles[index].Occupation_space += (int)Math.Ceiling(Convert.ToDouble(size / 512.0)) * 512;
                            UpdateSize(file.SubFiles[index], path.Substring(path.IndexOf('/') + 1), ref size);
                        }
                    }
                }
            }
           
        }
        //搜索
        public static File findByName(File file, string path)
        {
            if (file != null && file.SubFiles != null)
            {
                for (int index = 0; index < file.SubFiles.Count(); index++)
                {
                    string[] array = path.Split('/');
                    string filename = file.SubFiles[index].Name;
                    if (filename.Equals(array[1]))
                    {
                        if (array.Count() == 2)
                        {
                            if (filename.Equals(array[0]))
                            {
                                return file.SubFiles[index];
                            }
                        }
                        //继续深入查找
                        else if (array.Count() > 2)
                        {
                            return findByName(file.SubFiles[index], path.Substring(path.IndexOf('/') + 1));
                        }
                    }
                }
            }
           
            return file;
        }
        //删除目录
        public static void deleteCategory(int id)
        {
            for (int index = 0; index < block_number; index++)
            {
                if (Files[index] != null)
                {
                    byte[] result = fileToBytes(Files[index]);
                    byte[] bytes = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        bytes[i] = result[40 + i];
                    }
                    if (id == Transform.getInt(bytes))
                    {
                        Files[index] = null;
                        break;
                    }
                }
            }
            save();   //重新写入
        }
        //保存
        public static void save()
        {
            FileStream fs = new FileStream("CategoryDisk.dat", FileMode.Create, FileAccess.Write);
            for (int index =0; index < root.Count; index++)
            {
                saveFile(fs, root[index]);
            }
            fs.Close();
        }
        //保存
        public static void saveFile(FileStream fs, File file)
        {
            if(file != null)
            {
                byte[] result = fileToBytes(file);
                fs.Write(result, 0, 192);
                if (file.SubFiles != null && file.SubFiles.Count != 0)
                {
                    for (int i = 0; i < file.SubFiles.Count; i++)
                    {
                        saveFile(fs, file.SubFiles[i]);
                    }
                }
            }
        }
        //文件类型转为字节
        public static byte[] fileToBytes(File file)
        {
            byte[] result = new byte[192];
            //name
            byte[] name = Transform.getBytes(file.Name);
            if (name.Count() < 40)
            {
                for (int i = 0; i < name.Count(); i++)
                {
                    result[i] = name[i];
                }
            }
            //id
            byte[] id = Transform.getBytes(file.Id);
            if (id.Count() < 5)
            {
                for (int i = 0; i < id.Count(); i++)
                {
                    result[40 + i] = id[i];
                }
            }
            //type
            int type_int = 0;
            if (file.Type.Equals("Folder"))
            {
                type_int = 1;
            }
            
            byte[] type = Transform.getBytes(type_int);
            if (type.Count() < 5)
            {
                for (int i = 0; i < type.Count(); i++)
                {
                    result[44 + i] = type[i];
                }
            }
            //authority
            byte[] authority = Transform.getBytes((int)file.Authority);
            if (authority.Count() < 5)
            {
                for (int i = 0; i < authority.Count(); i++)
                {
                    result[48 + i] = authority[i];
                }
            }
            //path
            byte[] path = Transform.getBytes(file.Path);
            if (path.Count() < 101)
            {
                for (int i = 0; i < path.Count(); i++)
                {
                    result[52 + i] = path[i];
                }
            }
            //date
            byte[] date = Transform.getBytes(file.Date);
            if (date.Count() < 20)
            {
                for (int i = 0; i < date.Count(); i++)
                {
                    result[152 + i] = date[i];
                }
            }
            //disk_start
            byte[] disk_start = Transform.getBytes(file.Disk_start);
            if (disk_start.Count() < 5)
            {
                for (int i = 0; i < disk_start.Count(); i++)
                {
                    result[172 + i] = disk_start[i];
                }
            }
            //file_number
            byte[] file_number = Transform.getBytes(file.File_number);
            if (file_number.Count() < 5)
            {
                for (int i = 0; i < file_number.Count(); i++)
                {
                    result[176 + i] = file_number[i];
                }
            }
            //folder_number
            byte[] folder_number = Transform.getBytes(file.Folder_number);
            if (folder_number.Count() < 5)
            {
                for (int i = 0; i < folder_number.Count(); i++)
                {
                    result[180 + i] = folder_number[i];
                }
            }
            //occupation_space
            byte[] occupation_space = Transform.getBytes(file.Occupation_space);
            if (occupation_space.Count() < 5)
            {
                for (int i = 0; i < occupation_space.Count(); i++)
                {
                    result[184 + i] = occupation_space[i];
                }
            }
            //size
            byte[] size = Transform.getBytes(file.Size);
            if (size.Count() < 5)
            {
                for (int i = 0; i < size.Count(); i++)
                {
                    result[188 + i] = size[i];
                }
            }
            return result;
        }
        //字节转为文件
        public static File bytesToFile(byte[] result)
        {
            File file = new File();
            //name
            byte[] bytes = new byte[40];
            for (int i = 0; i < 40; i++)
            {
                bytes[i] = result[i];
            }
            string str = Transform.getString(bytes);
            string res = str.Split('\0')[0];

            file.Name = res;
            //id
            bytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                bytes[i] = result[40 + i];
            }
            file.Id = Transform.getInt(bytes);

            //type
            bytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                bytes[i] = result[44 + i];
            }
            int int_type = Transform.getInt(bytes);
            if(int_type == 0)
            {
                file.Type = "File";
            }
            else
            {
                file.Type = "Folder";
            }
            //authority
            bytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                bytes[i] = result[48 + i];
            }
            file.Authority = (File.FileAuthority)Transform.getInt(bytes);

            //path
            bytes = new byte[100];
            for (int i = 0; i < 100; i++)
            {
                bytes[i] = result[52 + i];
            }

            str = Transform.getString(bytes);
            res = str.Split('\0')[0];

            file.Path = res;
            //date
            bytes = new byte[20];
            for (int i = 0; i < 20; i++)
            {
                bytes[i] = result[152 + i];
            }
            str = Transform.getString(bytes);
            res = str.Split('\0')[0];

            file.Date = res;
            //disk_start
            bytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                bytes[i] = result[172 + i];
            }
            file.Disk_start = Transform.getInt(bytes);
            //file_number
            bytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                bytes[i] = result[176 + i];
            }
            file.File_number = Transform.getInt(bytes);
            //folder_number
            bytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                bytes[i] = result[180 + i];
            }
            file.Folder_number = Transform.getInt(bytes);
            //occupation_space
            bytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                bytes[i] = result[184 + i];
            }
            file.Occupation_space = Transform.getInt(bytes);
            //size
            bytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                bytes[i] = result[188 + i];
            }
            file.Size = Transform.getInt(bytes);
            return file;
        }
    }

}
