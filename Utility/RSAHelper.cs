using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Utility
{
    /// <summary>
	/// 引用：https://github.com/stulzq/DotnetCore.RSA/blob/master/DotnetCore.RSA/RSAHelper.cs
    /// 此算法用于.net core平台，.net平台未试
	/// </summary>
	public class RSAHelper
    {
        #region 使用私钥签名

        /// <summary>
        /// 使用私钥签名,并返回base64格式的签名
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <returns></returns>
        public static string Sign(string data, string privatePemKey)
        {
            var rsa = RSACryptoServiceProvider.Create();
            rsa.ImportParameters(GetRSAParametersFromFromPrivatePem(privatePemKey));
            return Convert.ToBase64String(rsa.SignData(Encoding.UTF8.GetBytes(data), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
        }

        #endregion

        #region 使用公钥验证签名

        /// <summary>
        /// 使用公钥验证签名
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="sign">签名的base64格式数据</param>
        /// <returns></returns>
        public static bool Verify(string data, string sign, string publicPemKey)
        {
            var rsa = RSACryptoServiceProvider.Create();
            rsa.ImportParameters(GetRSAParametersFromFromPublicPem(publicPemKey));
            return rsa.VerifyData(Encoding.UTF8.GetBytes(data), Convert.FromBase64String(sign), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }

        #endregion

        #region 加密


        /// <summary>
        /// 根据pem格式的key和明文，加密。此方法用的是BouncyCastle，因此公钥/私钥都能用于加密/解密
        /// </summary>
        /// <param name="pemKey">pem格式的key，可以为publicKey或是privateKey</param>
        /// <param name="inBuf">明文的byte[]</param>
        /// <returns></returns>
        public static byte[] EncryptByPemKey(string pemKey, byte[] inBuf)
        {
            var pemReader = new PemReader(new StringReader(pemKey));
            var rsaEngine = new RsaEngine();
            var pemObject = pemReader.ReadObject();
            if (pemObject is AsymmetricKeyParameter publicPara)
            {
                //如果pemKey为公钥
                rsaEngine.Init(true, publicPara);

            }
            else if (pemObject is AsymmetricCipherKeyPair keyPair)
            {
                //如果pemKey为私钥
                rsaEngine.Init(true, keyPair.Private);

            }
            return rsaEngine.ProcessBlock(inBuf, 0, inBuf.Length);
        }

        /// <summary>
        /// 根据密钥加密明文，并返回bs64格式的密文
        /// </summary>
        /// <param name="pemKey">密钥，可以是公钥或私钥</param>
        /// <param name="plainText">明文</param>
        /// <returns>bs64格式的密文</returns>
        public static string EncryptByPemKey(string pemKey, string plainText)
        {
            return Convert.ToBase64String(EncryptByPemKey(pemKey, Encoding.UTF8.GetBytes(plainText)));
        }

        /// <summary>
        /// 根据pem格式的publicKey加密，此用的是RSACryptoServiceProvider，微软考虑到安全性，约定只有公钥能进行加密
        /// </summary>
        /// <param name="publicPemKey"></param>
        /// <param name="inBuf"></param>
        /// <returns></returns>
        public static byte[] EncryptByPublicPemKey(string publicPemKey, byte[] inBuf)
        {
            var para = GetRSAParametersFromFromPublicPem(publicPemKey);
            var rsa = RSACryptoServiceProvider.Create();
            rsa.ImportParameters(para);
            return rsa.Encrypt(inBuf, RSAEncryptionPadding.Pkcs1);//大多数用的是pkcs1方式，考虑和其它系统或平台的兼容，用此方式
        }

        /// <summary>
        /// 根据公钥加密明文，并返回bs64格式的密文
        /// </summary>
        /// <param name="privatePemKey">公钥</param>
        /// <param name="cipherText">明文</param>
        /// <returns>bs64格式的密文</returns>
        public static string EncryptByPublicPemKey(string publicPemKey, string plainText)
        {
            return Convert.ToBase64String(EncryptByPublicPemKey(publicPemKey, Encoding.UTF8.GetBytes(plainText)));
        }
        #endregion


        #region 解密


        /// <summary>
        /// 根据pem格式的key和明文，解密。此方法用的是BouncyCastle，因此公钥/私钥都能用于加密/解密
        /// </summary>
        /// <param name="pemKey">pem格式的key，可以为publicKey或是privateKey</param>
        /// <param name="inBuf">密文的byte[]</param>
        /// <returns></returns>
        public static byte[] DecryptByPemKey(string pemKey, byte[] inBuf)
        {
            var pemReader = new PemReader(new StringReader(pemKey));
            var rsaEngine = new RsaEngine();
            var pemObject = pemReader.ReadObject();
            if (pemObject is AsymmetricKeyParameter publicPara)
            {
                //如果pemKey为公钥
                rsaEngine.Init(false, publicPara);

            }
            else if (pemObject is AsymmetricCipherKeyPair keyPair)
            {
                //如果pemKey为私钥
                rsaEngine.Init(false, keyPair.Private);

            }
            return rsaEngine.ProcessBlock(inBuf, 0, inBuf.Length);
        }

        /// <summary>
        /// 根据密钥解密bs64格式的密文
        /// </summary>
        /// <param name="pemKey">密钥，可以是公钥或私钥</param>
        /// <param name="cipherText">bs64格式的密文</param>
        /// <returns>明文</returns>
        public static string DecryptByPemKey(string pemKey, string cipherText)
        {
            return Encoding.UTF8.GetString(DecryptByPemKey(pemKey, Convert.FromBase64String(cipherText)));
        }



        /// <summary>
        /// 根据pem格式的privateKey加密，此用的是RSACryptoServiceProvider，微软考虑到安全性，约定只有私钥能进行加密
        /// </summary>
        /// <param name="publicPemKey"></param>
        /// <param name="inBuf"></param>
        /// <returns></returns>
        public static byte[] DecryptByPrivatePemKey(string privatePemKey, byte[] inBuf)
        {
            var para = GetRSAParametersFromFromPrivatePem(privatePemKey);
            var rsa = RSACryptoServiceProvider.Create();
            rsa.ImportParameters(para);
            return rsa.Decrypt(inBuf, RSAEncryptionPadding.Pkcs1);//大多数用的是pkcs1方式，考虑和其它系统或平台的兼容，用此方式
        }

        /// <summary>
        /// 根据私钥解密出bs64格式的密文
        /// </summary>
        /// <param name="privatePemKey">私钥</param>
        /// <param name="cipherText">密文</param>
        /// <returns></returns>
        public static string DecryptByPrivatePemKey(string privatePemKey, string cipherText)
        {
            return Encoding.UTF8.GetString(DecryptByPrivatePemKey(privatePemKey, Convert.FromBase64String(cipherText)));
        }
        #endregion


        #region 生成rsa公私钥对
        /// <summary>
        /// 生成pem格式的公私钥
        /// </summary>
        /// <param name="strength"></param>
        /// <returns></returns>
        public static (string publicKey, string privateKey) GeneratePemRsaKeys(int strength = 2048)
        {
            var gen = new RsaKeyPairGenerator();
            gen.Init(new KeyGenerationParameters(new SecureRandom(), strength));
            var pair = gen.GenerateKeyPair();
            var pubPemW = new PemWriter(new StringWriter());
            var priPemW = new PemWriter(new StringWriter());
            pubPemW.Writer.Flush(); pubPemW.WriteObject(pair.Public);
            priPemW.Writer.Flush(); priPemW.WriteObject(pair.Private);
            var publicKey = pubPemW.Writer.ToString();
            var privateKey = priPemW.Writer.ToString();
            return (publicKey, privateKey);
        }

        #endregion

        #region 根据pem格式的key生成RSAParameters

        /// <summary>
        /// 根据pem字符串得到私钥
        /// </summary>
        /// <param name="privatePem"></param>
        /// <returns></returns>
        public static RSAParameters GetRSAParametersFromFromPrivatePem(string privatePem)
        {
            using (var privateKeyTextReader = new StringReader(privatePem))
            {
                var readObject = new PemReader(privateKeyTextReader).ReadObject();
                if (readObject is AsymmetricCipherKeyPair keyPair)
                {
                    var privateKeyParams = ((RsaPrivateCrtKeyParameters)keyPair.Private);
                    var parms = new RSAParameters
                    {
                        Modulus = privateKeyParams.Modulus.ToByteArrayUnsigned(),
                        P = privateKeyParams.P.ToByteArrayUnsigned(),
                        Q = privateKeyParams.Q.ToByteArrayUnsigned(),
                        DP = privateKeyParams.DP.ToByteArrayUnsigned(),
                        DQ = privateKeyParams.DQ.ToByteArrayUnsigned(),
                        InverseQ = privateKeyParams.QInv.ToByteArrayUnsigned(),
                        D = privateKeyParams.Exponent.ToByteArrayUnsigned(),
                        Exponent = privateKeyParams.PublicExponent.ToByteArrayUnsigned()
                    };
                    return parms;
                }
                else
                {
                    throw new Exception("传入的pem不是私钥");
                }
            }
        }

        /// <summary>
        /// 根据pem字符串得到公钥
        /// </summary>
        /// <param name="publicPem"></param>
        /// <returns></returns>
        public static RSAParameters GetRSAParametersFromFromPublicPem(string publicPem)
        {
            using (var publicKeyTextReader = new StringReader(publicPem))
            {
                var readObject = new PemReader(publicKeyTextReader).ReadObject();
                if (readObject is RsaKeyParameters publicKeyParam)
                {
                    var parms = new RSAParameters
                    {
                        Modulus = publicKeyParam.Modulus.ToByteArrayUnsigned(),
                        Exponent = publicKeyParam.Exponent.ToByteArrayUnsigned()
                    };
                    return parms;
                }
                else
                {
                    throw new Exception("传入的pem不是公钥");
                }

            }
        }
        #endregion
    }

    /// <summary>
    /// RSA算法类型
    /// </summary>
    public enum RSAType
    {
        /// <summary>
        /// SHA1
        /// </summary>
        RSA = 0,
        /// <summary>
        /// RSA2 密钥长度至少为2048
        /// SHA256
        /// </summary>
        RSA2
    }
}
