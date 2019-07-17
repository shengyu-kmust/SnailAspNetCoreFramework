using System;
using System.Text;
using Utility;
using Xunit;
using System.Security.Cryptography;
namespace UtilityTest
{
    public class RSAHelperTest
    {
        public string privateKey = @"MIICXAIBAAKBgQCLhHXo597qRqByhvr2VulAPm7pnTh2gDqXP90ULGlNwAV51qUd
h5dBvtztELTSmuibGxwDL0hkPUKwh/P2aOkVuzsv+FvnKsEDzjPfxbsQADl1Dyba
fn91Ng2PTTagskEOlFPwAiG/cPTG/KkYft2w99GHMIQ2Zqt1URicVzy+/QIDAQAB
AoGAAoNV7kLauUAvXoGhTnICCxA6KdN4vF0H+8+ENMmfq2Y22+zhp8eKQ+kYrEDS
CIxV0zS8VEwF0VGE1Xk+xM2QdlNbO9MP+AXPHHYqhEqDAhVmfsIKsZfVcuGLAeX7
BXbpFKpJSuExw92AaRRHfZqtuGhMq7cq0lgxEo0S9lqg1BECQQCmvxK/CNOK7gtU
fcJizzxjt9ptkJCEaCnLnw52brlwugQ8IXB9UgCYqxoLcAKlJgSJBNYo0gYDQaVy
lLEiKLqNAkEA1jJB5M+jvctbnies9hBOGSMJKB/e9h/KO9xNdQ9DUqNFNAttSYEg
P6Qn6QiIkGbeHr+UhIW8Vwi2o6PqYWeyMQJAeB819gD5xB/wcZGXM29vZbbQ2BVI
xtnzIgkXdiIV3StkWZ0NWHp2i4TXYl3yyd41bi/Zx9ZlFnt4IK8VXDJTEQJBAJrR
PpFshzRibPkWQkykIl7G1RJ8XsJU3e6AYDfw7T8opZdlfvt26mE1fGdR9Ksyvu9I
l8dlhmj98ky66Gi487ECQA4gogfEZPufI89KoFmXY/omkS9CQq4E/2qAwj1jMeAY
69g+M1xInjouvSyaWQtZvBeCBX8VD9FQkzYkA2iR2BA=";
        public string publicKey = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCLhHXo597qRqByhvr2VulAPm7p
nTh2gDqXP90ULGlNwAV51qUdh5dBvtztELTSmuibGxwDL0hkPUKwh/P2aOkVuzsv
+FvnKsEDzjPfxbsQADl1Dybafn91Ng2PTTagskEOlFPwAiG/cPTG/KkYft2w99GH
MIQ2Zqt1URicVzy+/QIDAQAB";
        public RSAHelper rsa;
        public RSAHelperTest()
        {
            rsa = new RSAHelper(RSAType.RSA, Encoding.UTF8, privateKey, publicKey);
        }

        [Fact]
        public void EncryptAndDecrypt()
        {
            try
            {
                var enStr = rsa.Encrypt("zhoujing");
                var deStr = rsa.Decrypt(enStr);
            }
            catch (Exception ex)
            {
            }
        }

        [Fact]
        public void SignAndVerify()
        {
            try
            {
                var signStr = rsa.Sign("zhoujing");
                var verify = rsa.Verify("zhoujing", signStr);
            }
            catch (Exception ex)
            {
            }
        }

        [Fact]
        public void Rsa()
        {
            try
            {
                var rsa = RSACryptoServiceProvider.Create();
                var rsaPara = rsa.ExportParameters(true);
                var enStr = Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes("zhoujing"), RSAEncryptionPadding.Pkcs1));
                var deStr = Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String(enStr), RSAEncryptionPadding.Pkcs1));
                var sign = rsa.SignData(Encoding.UTF8.GetBytes("zhoujing"), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
                var isVerify = rsa.VerifyData(Encoding.UTF8.GetBytes("zhoujing"), sign, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            }
            catch (Exception ex)
            {
            }
        }

      

        [Fact]
        public void Rsa2()
        {
            try
            {
                var priRsa = RSACryptoServiceProvider.Create(RSAHelper.CreateRSAParametersFromPrivateKey(privateKey));
                var pubRsa = RSACryptoServiceProvider.Create(RSAHelper.CreateRSAParametersFromPublicKey(publicKey));
                var enStr = Convert.ToBase64String(pubRsa.Encrypt(Encoding.UTF8.GetBytes("zhoujing"), RSAEncryptionPadding.Pkcs1));
                var deStr = Encoding.UTF8.GetString(priRsa.Decrypt(Convert.FromBase64String(enStr), RSAEncryptionPadding.Pkcs1));
                var sign = priRsa.SignData(Encoding.UTF8.GetBytes("zhoujing"), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
                var isVerify = pubRsa.VerifyData(Encoding.UTF8.GetBytes("zhoujing"), sign, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            }
            catch (Exception ex)
            {
            }
        }

        [Fact]
        public void Rsa3()
        {
            try
            {

                var enStr = rsa.Encrypt("zhoujing");
                var deStr = rsa.Decrypt(enStr);
                var sign = rsa.Sign("zhoujing");
                var isVerify = rsa.Verify("zhoujing", sign);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
