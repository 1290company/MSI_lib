using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Wil50n.Tool
{
    public class GenBatchNo
    {
        private static RNGCryptoServiceProvider rngp = new RNGCryptoServiceProvider();
        private static byte[] rb = new byte[4];
        static int Next(int max)
        {
            rngp.GetBytes(rb);
            int value = BitConverter.ToInt32(rb, 0);
            value = value % (max + 1);
            if (value < 0) value = -value;
            return value;
        }
        static int Next(int min, int max)
        {
            int value = Next(max - min) + min;
            return value;
        }

        public static String GetBatchNo(int min = 10, int max = 10)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            char[] chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            int length = Next(min, max);
            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[Next(chars.Length - 1)]);
            }
            return sb.ToString();
        }
    }
}
