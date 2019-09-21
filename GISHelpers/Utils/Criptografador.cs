using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GISHelpers.Utils
{
    public class Criptografador
    {
        private const string Key1 = "G&i0S.0IsL1fe4everAnd4TW";
        private const string Key2 = "G&0iKey4n00bs";

        private static TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
        private static MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();

        private static byte[] MD5Hash(string value)
        {
            return MD5.ComputeHash(Encoding.UTF8.GetBytes(value));
        }

        public static string Criptografar(string entry, int keyNumber)
        {
            string key;

            if (keyNumber == 1)
                key = Key1;
            else if
                (keyNumber == 2) key = Key2;
            else
                throw new InvalidOperationException("Tipo de criptografia não reconhecida.");

            DES.Key = MD5Hash(key);
            DES.Mode = CipherMode.ECB;

            byte[] buffer = Encoding.UTF8.GetBytes(entry);

            return Convert.ToBase64String(DES.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));
        }

        public static string Descriptografar(string entry, int keyNumber)
        {
            string key;

            if (keyNumber == 1)
                key = Key1;
            else if
                (keyNumber == 2) key = Key2;
            else
                throw new InvalidOperationException("Tipo de criptografia não reconhecida.");

            DES.Key = MD5Hash(key);
            DES.Mode = CipherMode.ECB;

            byte[] buffer = Convert.FromBase64String(entry);

            return Encoding.UTF8.GetString(DES.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
        }

    }
}
