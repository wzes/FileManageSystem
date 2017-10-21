using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManageSystem
{
    public static class DiskManage
    {
        public static int disk_size = 5 * 1024 * 1024;              //磁盘大小
        public static int block_size = 512;                         //每一块大小
        public static int block_number = disk_size / block_size;    //每一块大小
        public static int[] bitMap = new int[block_number];         //位向量
        public static Block[] blocks = new Block[block_number];

        //从文件恢复数据到内存
        public static void init()
        {
            readDisk();
        }
        //恢复磁盘数据到内存
        public static void readDisk()
        {
            FileStream fs = new FileStream("myDisk.dat", FileMode.OpenOrCreate, FileAccess.Read);
            int p = 0;
            for (int index = 0; index < block_number; index++)
            {
                byte[] result = new byte[512];

                if(fs.Read(result, p, 512) != 0)
                {
                    CategoryManage.total_size_occupation += 512;
                    byte[] bytes = new byte[4];
                    Block block = new Block();
                    for (int i = 0; i < 4; i++)
                    {
                        bytes[i] = result[i];
                    }
                    int head_id = Transform.getInt(bytes);
                    block.content = result;
                    bitMap[head_id] = 1;
                    blocks[head_id] = block;
                }
                else
                {
                    break;
                }
            }
            fs.Close();
        }
        //保存内存
        public static void save()
        {
            FileStream fs = new FileStream("myDisk.dat", FileMode.Truncate, FileAccess.Write);
            for (int index = 0; index < block_number; index++)
            {
                if(bitMap[index] == 1)
                {
                    byte[] result = new byte[512];
                    for (int i = 0; i < 512; i++)
                    {
                        result[i] = blocks[index].content[i];
                    }
                    fs.Write(result, 0, 512);
                }
            }
            fs.Close();
        }
        //存字符串文件
        public static int saveContent(string str, ref int Size)
        {
            return saveContent(Transform.getBytes(str), ref Size);
        }
        //存二进制文件
        public static int saveContent(byte[] bytes, ref int Size)
        {
            int size = bytes.Count();
            Size = size;
            int block_number = size / (block_size - 8) + 1;
            int block_id = getSpace();
            int next_block_id = 0;
            int first = block_id;
            int bp = 0;           //文件指针
            for (int index = 0; index < block_number; index++)
            {
                Block block = new Block();
                bitMap[block_id] = 1;
                byte[] result = Transform.getBytes(block_id);
                block.content = new byte[512];
                for (int i = 0; i < 4; i++)
                {
                    block.content[i] = result[i];
                }
                int p = 4;
                while (p < 508)
                {
                    if (bp >= size)
                    {
                        break;
                    }
                    block.content[p] = bytes[bp];
                    p++;
                    bp++;
                }
                if(index < block_number - 1)
                {
                    next_block_id = getSpace();
                    byte[] next = Transform.getBytes(next_block_id);
                    //最后四位放下一指针
                    for (int i = 0; i < 4; i++)
                    {
                        block.content[508 + i] = next[i];
                    }
                }
                else
                {
                    byte[] next = Transform.getBytes(-1);
                    //最后四位放-1 表示结束
                    for (int i = 0; i < 4; i++)
                    {
                        block.content[508 + i] = next[i];
                    }
                }
                blocks[block_id] = block;
                block_id = next_block_id;

            }
            return first;
        }
        //删除文件
        public static void deleteFile(int block_id)
        {
            if (block_id == -1)
            {
                return;
            }
            else
            {
                int first_id = block_id;
                bitMap[first_id] = 0;    //设为可用
                byte[] next = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    next[i] = blocks[block_id].content[508 + i];
                }
                int next_block_id = Transform.getInt(next);
                while (next_block_id != -1)
                {
                    bitMap[next_block_id] = 0;    //设为可用
                    for (int i = 0; i < 4; i++)
                    {
                        next[i] = blocks[next_block_id].content[508 + i];
                    }
                    next_block_id = Transform.getInt(next);
                }
            }
            
        }
        //清空文件
        public static void clearDisk()
        {
            FileStream fs = new FileStream("myDisk.dat", FileMode.Truncate, FileAccess.Write);
            fs.Close();
        }
       
        //获取文件内容
        public static string getStrContent(int block_id, int size)
        {
            byte[] content = new byte[size];
            if (size != 0)
            {
                int first_id = block_id;
                int p = 0;
                for (int i = 0; i < 504; i++)
                {
                    if (i < size)
                    {
                        content[p] = blocks[first_id].content[i + 4];
                        p++;
                    }
                    else
                    {
                        break;
                    }
                }
                byte[] next = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    next[i] = blocks[block_id].content[508 + i];
                }
                int next_block_id = Transform.getInt(next);
                while (next_block_id != -1)
                {
                    for (int i = 0; i < 504; i++)
                    {
                        if (p < size)
                        {
                            content[p] = blocks[next_block_id].content[i + 4];
                            p++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        next[i] = blocks[next_block_id].content[508 + i];
                    }
                    next_block_id = Transform.getInt(next);
                }
            }
            
            return Transform.getString(content);
        }
        //取得一个空闲块
        public static int getSpace()
        {
            for(int index = 0; index < bitMap.Count(); index++)
            {
                if(bitMap[index] == 0)
                {
                    return index;
                }
            }
            return -1;
        }
        //获取头部块
        public static int getFirstBlockId(byte[] bytes)
        {
            byte[] result = new byte[4]; 
            for(int i = 0;i < 4; i++)
            {
                result[i] = bytes[i];
            }
            return Transform.getInt(result);
        }
        //获取尾部块
        public static int getEndBlockId(byte[] bytes)
        {
            byte[] result = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                result[i] = bytes[508 + i];
            }
            return Transform.getInt(result);
        }
       
    }
    
}
