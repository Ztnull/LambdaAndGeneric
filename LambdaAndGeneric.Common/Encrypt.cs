using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LambdaAndGeneric.Common
{
    /// <summary>
    /// 非对称不可逆加密+文件流MD5值
    /// </summary>
    public class MD5Encrypt
    {
        #region MD5
        /// <summary>
        /// MD5加密,和动网上的16/32位MD5加密结果相同,
        /// 使用的UTF8编码
        /// </summary>
        /// <param name="source">待加密字串</param>
        /// <param name="length">16或32值之一,其它则采用.net默认MD5加密算法</param>
        /// <returns>加密后的字串</returns>
        public static string Encrypt(string source, int length = 32)//默认参数
        {
            if (string.IsNullOrEmpty(source)) return string.Empty;
            HashAlgorithm provider = CryptoConfig.CreateFromName("MD5") as HashAlgorithm;
            byte[] bytes = Encoding.UTF8.GetBytes(source);//这里需要区别编码的
            byte[] hashValue = provider.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            switch (length)
            {
                case 16://16位密文是32位密文的9到24位字符
                    for (int i = 4; i < 12; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
                case 32:
                    for (int i = 0; i < 16; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
                default:
                    for (int i = 0; i < hashValue.Length; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
            }
            return sb.ToString();
        }
        #endregion MD5

        #region MD5摘要+ 文件的加密，获取文件的MD5值
        /// <summary>
        /// 文件的加密，获取文件的MD5值
        /// 获取文件的MD5摘要
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string AbstractFile(string fileName)
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        #endregion
    }

    /// <summary>
    /// 非对称可逆加密
    /// </summary>
    public class RSANewEncrypt
    { 

        /// <summary>
        /// 获取加密/解密对
        /// Encrypt   Decrypt
        /// </summary>
        /// <returns>Encrypt   Decrypt</returns>
        public static KeyValuePair<string, string> GetKeyPair()
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            string publicKey = RSA.ToXmlString(false);
            string privateKey = RSA.ToXmlString(true);
            return new KeyValuePair<string, string>(publicKey, privateKey);
        }

        /// <summary>
        /// 加密：内容+加密key
        /// </summary>
        /// <param name="content"></param>
        /// <param name="encryptKey">加密key</param>
        /// <returns></returns>
        public static string Encrypt(string content, string encryptKey)
        {


            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();


            rsa.FromXmlString(encryptKey);
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] DataToEncrypt = ByteConverter.GetBytes(content);
            byte[] resultBytes = rsa.Encrypt(DataToEncrypt, false);
            return Convert.ToBase64String(resultBytes);
        }

        /// <summary>
        /// 解密  内容+解密key
        /// </summary>
        /// <param name="content"></param>
        /// <param name="decryptKey">解密key</param>
        /// <returns></returns>
        public static string Decrypt(string content, string decryptKey)
        {
            byte[] dataToDecrypt = Convert.FromBase64String(content);
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.FromXmlString(decryptKey);
            byte[] resultBytes = RSA.Decrypt(dataToDecrypt, false);
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            return ByteConverter.GetString(resultBytes);
        }


        /// <summary>
        /// 可以合并在一起的，，每次产生一组新的密钥
        /// </summary>
        /// <param name="content"></param>
        /// <param name="encryptKey">加密key</param>
        /// <param name="decryptKey">解密key</param>
        /// <returns>加密后结果</returns>
        private static string Encrypt(string content, out string publicKey, out string privateKey)
        {
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
            publicKey = rsaProvider.ToXmlString(false);
            privateKey = rsaProvider.ToXmlString(true);

            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] DataToEncrypt = ByteConverter.GetBytes(content);
            byte[] resultBytes = rsaProvider.Encrypt(DataToEncrypt, false);
            return Convert.ToBase64String(resultBytes);
        }
    }



    /// <summary>
    /// 生成：私钥、公钥储存到本地，进行加密和解密
    /// 加密钥匙和解密钥匙以XML形式存储到本地文件
    /// </summary>
    public class RSAUtil
    {
        /// <summary>
        /// 创建加密钥，解密钥
        /// </summary>
        public void CreateRSAKey()
        {

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            RSAParameters keys = rsa.ExportParameters(true);

            //公钥
            String pkxml = "<root>\n<Modulus>" + ToHexString(keys.Modulus) + "</Modulus>";
            pkxml += "\n<Exponent>" + ToHexString(keys.Exponent) + "</Exponent>\n</root>";

            //私钥
            String psxml = "<root>\n<Modulus>" + ToHexString(keys.Modulus) + "</Modulus>";
            psxml += "\n<Exponent>" + ToHexString(keys.Exponent) + "</Exponent>";
            psxml += "\n<D>" + ToHexString(keys.D) + "</D>";
            psxml += "\n<DP>" + ToHexString(keys.DP) + "</DP>";
            psxml += "\n<P>" + ToHexString(keys.P) + "</P>";
            psxml += "\n<Q>" + ToHexString(keys.Q) + "</Q>";
            psxml += "\n<DQ>" + ToHexString(keys.DQ) + "</DQ>";
            psxml += "\n<InverseQ>" + ToHexString(keys.InverseQ) + "</InverseQ>\n</root>";

            SaveToFile("publickey.xml", pkxml);
            SaveToFile("privatekey.xml", psxml);

        }

        /// <summary>
        /// 根据路径，获取解密数据
        /// </summary>
        /// <param name="privateKeyFile">解密文件路径</param>
        /// <returns></returns>
        public RSACryptoServiceProvider CreateRSADEEncryptProvider(String privateKeyFile)
        {
            RSAParameters parameters1;
            parameters1 = new RSAParameters();

            using (StreamReader reader1 = new StreamReader(privateKeyFile))
            {
                XmlDocument document1 = new XmlDocument();
                document1.LoadXml(reader1.ReadToEnd());
                XmlElement element1 = (XmlElement)document1.SelectSingleNode("root");
                parameters1.Modulus = ReadChild(element1, "Modulus");
                parameters1.Exponent = ReadChild(element1, "Exponent");
                parameters1.D = ReadChild(element1, "D");
                parameters1.DP = ReadChild(element1, "DP");
                parameters1.DQ = ReadChild(element1, "DQ");
                parameters1.P = ReadChild(element1, "P");
                parameters1.Q = ReadChild(element1, "Q");
                parameters1.InverseQ = ReadChild(element1, "InverseQ");
                CspParameters parameters2 = new CspParameters();
                parameters2.Flags = CspProviderFlags.UseMachineKeyStore;
                RSACryptoServiceProvider provider1 = new RSACryptoServiceProvider(parameters2);
                provider1.ImportParameters(parameters1);
                reader1.Close();
                return provider1;
            }

        }
        /// <summary>
        /// 根据路径，获取加密数据
        /// </summary>
        /// <param name="publicKeyFile">加密文件路径</param>
        /// <returns></returns>
        public RSACryptoServiceProvider CreateRSAEncryptProvider(String publicKeyFile)
        {
            RSAParameters parameters1;
            parameters1 = new RSAParameters();
            using (StreamReader reader1 = new StreamReader(publicKeyFile))
            {
                XmlDocument document1 = new XmlDocument();
                document1.LoadXml(reader1.ReadToEnd());
                XmlElement element1 = (XmlElement)document1.SelectSingleNode("root");
                parameters1.Modulus = ReadChild(element1, "Modulus");
                parameters1.Exponent = ReadChild(element1, "Exponent");
                CspParameters parameters2 = new CspParameters();
                parameters2.Flags = CspProviderFlags.UseMachineKeyStore;
                RSACryptoServiceProvider provider1 = new RSACryptoServiceProvider(parameters2);
                provider1.ImportParameters(parameters1);
                return provider1;
            }
        }

        /// <summary>
        /// 根据传入的节点 读取XML子节点
        /// </summary>
        /// <param name="parent">父节点名称</param>
        /// <param name="name">当前查询的节点名称</param>
        /// <returns></returns>
        public byte[] ReadChild(XmlElement parent, string name)
        {
            XmlElement element1 = (XmlElement)parent.SelectSingleNode(name);
            return hexToBytes(element1.InnerText);
        }

        /// <summary>
        /// 将byte字节转化为字符
        /// </summary>
        /// <param name="bytes">转化的字节数组</param>
        /// <returns></returns>
        public string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                foreach (var item in bytes)
                {
                    strB.Append(item.ToString("X2"));
                }

                hexString = strB.ToString();
            }
            return hexString;
        }

        /// <summary>
        /// 将字符转化为字节形式
        /// </summary>
        /// <param name="src">转化的内容</param>
        /// <returns>返回一个byte数组</returns>
        public byte[] hexToBytes(String src)
        {
            int l = src.Length / 2;
            String str;
            byte[] ret = new byte[l];

            for (int i = 0; i < l; i++)
            {
                str = src.Substring(i * 2, 2);
                ret[i] = Convert.ToByte(str, 16);
            }
            return ret;
        }

        /// <summary>
        /// 保存秘钥的XML
        /// </summary>
        /// <param name="filename">文件名称及路径</param>
        /// <param name="data">写入值</param>
        public void SaveToFile(String filename, String data)
        {
            using (System.IO.StreamWriter sw = System.IO.File.CreateText(filename))
            {
                sw.WriteLine(data);
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str">加密内容</param>
        /// <returns>返回加密之后的字符串</returns>
        public string EnCrypt(string str)
        {
            RSACryptoServiceProvider rsaencrype = CreateRSAEncryptProvider("publickey.xml");
            String text = str;
            byte[] data = new UnicodeEncoding().GetBytes(text);
            byte[] endata = rsaencrype.Encrypt(data, true);
            return ToHexString(endata);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="hexstr">解密内容</param>
        /// <returns>解密结果</returns>
        public string DoEncrypt(string hexstr)
        {
            RSACryptoServiceProvider rsadeencrypt = CreateRSADEEncryptProvider("privatekey.xml");

            byte[] miwen = hexToBytes(hexstr);

            byte[] dedata = rsadeencrypt.Decrypt(miwen, true);

            return System.Text.UnicodeEncoding.Unicode.GetString(dedata);
        }
    }
}
