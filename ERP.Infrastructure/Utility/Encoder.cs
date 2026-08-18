using ERP.Domain.Interface.Utility;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ERP.Infrastructure.Utility
{
    public class Encoder: IEncoder
    {
        public string EncodeToMd5(string text)
        {
            using MD5 md5 = MD5.Create();

            var originalBytes = Encoding.UTF8.GetBytes(text);
            var encodedBytes = md5.ComputeHash(originalBytes);

            return Convert.ToHexString(encodedBytes);
        }
        public bool CompareMd5Text(string md5Text, string secondParam)  
        {
            secondParam = EncodeToMd5(secondParam);
            return md5Text == secondParam;
        }
    }
}
