using System;
using System.Security.Cryptography;
using System.Text;

namespace Collapsenav.Net.Tool
{
    public class MD5Tool
    {
        /// <summary>
        /// 解密
        /// </summary>
        public static string Decrypt(string md5)
        {
            throw new Exception("Are you kidding ?");
        }
        /// <summary>
        /// 加密
        /// </summary>
        public static string Encrypt(string msg)
        {
            using var md5 = MD5.Create();

            var result = md5.ComputeHash(Encoding.UTF8.GetBytes(msg));
            return BitConverter.ToString(result).Replace("-", "");
        }
    }
}