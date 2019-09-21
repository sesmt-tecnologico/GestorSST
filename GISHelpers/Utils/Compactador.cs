using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISHelpers.Utils
{
    public class Compactador
    {
        public static string Compactar(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var ms = new MemoryStream();
            using (var stream = new GZipStream(ms, CompressionMode.Compress, true))
                stream.Write(buffer, 0, buffer.Length);

            ms.Position = 0;

            var rawData = new byte[ms.Length];
            ms.Read(rawData, 0, rawData.Length);

            var compressedData = new byte[rawData.Length + 4];
            Buffer.BlockCopy(rawData, 0, compressedData, 4, rawData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, compressedData, 0, 4);
            return Convert.ToBase64String(compressedData);
        }

        public static string Descompactar(string compressedText)
        {
            byte[] compressedData = Convert.FromBase64String(compressedText);
            using (var ms = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(compressedData, 0);
                ms.Write(compressedData, 4, compressedData.Length - 4);

                var buffer = new byte[dataLength];

                ms.Position = 0;
                using (var stream = new GZipStream(ms, CompressionMode.Decompress))
                    stream.Read(buffer, 0, buffer.Length);

                return Encoding.UTF8.GetString(buffer);
            }
        }

    }
}
