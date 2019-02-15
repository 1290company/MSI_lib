using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MSI_lib
{
  public class lib_RSA_PKI
    {
        
        public Tuple<string, string> GenerateRSAKeys()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            var publicKey = rsa.ToXmlString(false);
            var privateKey = rsa.ToXmlString(true);
            return Tuple.Create<string, string>(publicKey, privateKey);
        }

        public string EncryptString(string publicKey, string rawContent)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);
            var encryptString = Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(rawContent), false));
            return encryptString;
        }

        public string DecryptString(string privateKey, string encryptedContent)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(privateKey);
                var decryptString = Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String(encryptedContent), false));
                return decryptString;
            }catch (Exception Ex)
            {
                return "";
            }
        }

        private void EncryptFile(string publicKey, string rawFilePath, string encryptedFilePath)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);

            using (FileStream testDataStream = File.OpenRead(rawFilePath))
            using (FileStream encrytpStream = File.OpenWrite(encryptedFilePath))
            {
                var testDataByteArray = new byte[testDataStream.Length];
                testDataStream.Read(testDataByteArray, 0, testDataByteArray.Length);

                var encryptDataByteArray = rsa.Encrypt(testDataByteArray, false);

                encrytpStream.Write(encryptDataByteArray, 0, encryptDataByteArray.Length);
            }
        }

        private void DecryptFile(string privateKey, string encryptedFilePath, string decryptedFilePath)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);

            using (FileStream encrytpStream = File.OpenRead(encryptedFilePath))
            using (FileStream decrytpStream = File.OpenWrite(decryptedFilePath))
            {
                var encryptDataByteArray = new byte[encrytpStream.Length];
                encrytpStream.Read(encryptDataByteArray, 0, encryptDataByteArray.Length);

                var decryptDataByteArray = rsa.Decrypt(encryptDataByteArray, false);

                decrytpStream.Write(decryptDataByteArray, 0, decryptDataByteArray.Length);
            }
        }

        public class Key
        {
            public string Publickey { set; get; }
            public string Privatekey { set; get; }
        }
        public Tuple<string, string> GetKeyToken(string uuid)
        {
            Tuple<string, string> tp = GenerateRSAKeys();   //
            Key objKey = new Key() { Publickey = tp.Item1, Privatekey = tp.Item2 };   //private key 經由USER 密碼加密,ITEM2 進TABLE
            string PrivateKey = EncryptString(objKey.Publickey, uuid);
            byte[] bytes = System.Text.Encoding.GetEncoding("utf-8").GetBytes(PrivateKey);
            string tocken = Convert.ToBase64String(bytes); //tocken  給USER



            return Tuple.Create<string, string>(tocken, objKey.Privatekey);
        }
        public bool DecryptUUID(string Tocken, string Privatekey, string UUID)
        {

            byte[] rebytes = Convert.FromBase64String(Tocken);
            string Prive = System.Text.Encoding.GetEncoding("utf-8").GetString(rebytes);//Tocken 透過utf8還原
            string result = DecryptString(Privatekey, Prive);  //base64
            if (UUID == result)
            {
                return true;
            }
            return false;
        }
    }
}
