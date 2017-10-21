using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManageSystem
{
    public static class Transform
    {
        //字节转字符串
        public static string getString(byte[] bytes)
        {
            return System.Text.Encoding.Default.GetString(bytes);
        }
        //字符串转字节
        public static byte[] getBytes(string str)
        {
            return System.Text.Encoding.Default.GetBytes(str);
        }
        //int转字节
        public static byte[] getBytes(int i)
        {
            return BitConverter.GetBytes(i);
        }
        //字节转int
        public static int getInt(byte[] bytes)
        {
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
