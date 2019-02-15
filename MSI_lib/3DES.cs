using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MSI_lib
{
    public class _3DES
    {
        //public string Encrypt(string pToEncrypt, string sKey = "saap1234")
        //{
        //    using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
        //    {
        //        byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
        //        des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
        //        des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
        //        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
        //        {
        //            cs.Write(inputByteArray, 0, inputByteArray.Length);
        //            cs.FlushFinalBlock();
        //            cs.Close();
        //        }
        //        string str = Convert.ToBase64String(ms.ToArray());
        //        ms.Close();
        //        return Strings.StrConv(str, VbStrConv.Wide, 0);
        //    }
        //}

        //public string Decrypt(string pToDecrypt, string sKey = "saap1234")
        //{
        //    pToDecrypt =  Strings.StrConv(pToDecrypt, VbStrConv.Narrow, 0);
        //    byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
        //    using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
        //    {
        //        des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
        //        des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
        //        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //        using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
        //        {
        //            cs.Write(inputByteArray, 0, inputByteArray.Length);
        //            cs.FlushFinalBlock();
        //            cs.Close();
        //        }
        //        string str = Encoding.UTF8.GetString(ms.ToArray());
        //        ms.Close();
        //        return str;
        //    }
        //}

        public string Encrypt(string pToEncrypt, string sKey = "saap1234")
        {
            StringBuilder sb = new StringBuilder();
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                foreach (byte b in ms.ToArray())
                {
                    sb.AppendFormat("{0:X2}", b);
                }
                string str = Convert.ToBase64String(ms.ToArray());
                str = sb.ToString();
                ms.Close();
                return str;
            }
        }

        public string Decrypt(string pToDecrypt, string sKey = "saap1234")
        {
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            // byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }
    }
}
