using DinkToPdf;
using System;
using Xunit;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IO;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        public string puk = "-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA0/VhtKwY2l0UtYWZMdcQ\n49FiRYCYtCDlrAEmIYfIV1J23G9Fp8NzGsF5GYeg8Qj+Tt/vER1zuNgx5nIiTWwd\nzuGKZUYHp/qI6V2b+6SyE554JCFop1DGE5f+XvB6CMXmcPWVe1jFObE6hJIVCom6\nK1ItlKOboRc7+Wmhg6Q1UTI+L7PTmu/sIGUW70+gOnpwxexCTXmFnPnuQzi8YR+q\n+jlCNwEssKvVn5xrMAkvWRgpfu6Sqbc0Ym4ZcvmPZCc9RNvC/NsoZg1+Qv3m08UM\ndvvbIW6qgHBIJihIct4nVGKDoa+nzYJpoVJNTBZy6el8g5qqMAHxCcDpb7TF4tec\ntQIDAQAB\n-----END PUBLIC KEY-----\n";
        public string prk = "-----BEGIN RSA PRIVATE KEY-----\nMIIEowIBAAKCAQEA0/VhtKwY2l0UtYWZMdcQ49FiRYCYtCDlrAEmIYfIV1J23G9F\np8NzGsF5GYeg8Qj+Tt/vER1zuNgx5nIiTWwdzuGKZUYHp/qI6V2b+6SyE554JCFo\np1DGE5f+XvB6CMXmcPWVe1jFObE6hJIVCom6K1ItlKOboRc7+Wmhg6Q1UTI+L7PT\nmu/sIGUW70+gOnpwxexCTXmFnPnuQzi8YR+q+jlCNwEssKvVn5xrMAkvWRgpfu6S\nqbc0Ym4ZcvmPZCc9RNvC/NsoZg1+Qv3m08UMdvvbIW6qgHBIJihIct4nVGKDoa+n\nzYJpoVJNTBZy6el8g5qqMAHxCcDpb7TF4tectQIDAQABAoIBAB4MfJSLjV5vsb7m\nOMirD7bseT0XNQ7cVxMiepBoWoueokEv+TUGbWwOn6dx5ewl9T+jWZYYkc5TwAdG\niPxm5nbs3Jzs56r1xrEm1zU+rQxA/BFLNzX5Koueka5N808JF1lOR6nb9OHv8TbY\nKul6iw0XMmUPKMIWtCbwxbgO8IwZDCX+Bp2h0IDFDZLc4M6AxkLDpCrFds9Up1x1\n3EjWtjowmITVllVY7ldl0dI7oG9j5RG0gRpYBpMAbE/gGUmnLybV0ndcD9vRCvVi\nl8E6CrHv74IvD947r785KzaWC7PPelBvL0P7tpqgypfkLh8/Wq2zyV4NWA4gH5z6\nCzWiTuUCgYEA+ktQpAsSlLQuUV8PpgDmCkYFGlHrQcCLYBPUydxrhCTpz8mASS+Z\nhEgGL1TacQQlJcBWbl0jozwDpSqP/KkpbOlKtG2B/ZS6nlpAhHk2aUvrpq2gBs/i\nGpIkTa5LqgykB0updByhELAJrQ+q5N3YUj2Qc4tU17bSaF49EpZoWNsCgYEA2MpY\nM+W1qSFkCGIV1GImacN+l6VSws5PyV3zJnAC18HcxvM+1pVX+0+XhdYEimSXMYyt\n1OvqQLFlYDAnaaCcFjuh/nazbTmd2qJ2ikinKYUXRhxkQPUJyKbratW7H3biG+aq\nplYssLDvbdJfblI7mYF1x9PM+bUEcam86OYrTa8CgYEA64Cx/6frc7+VVTr8nlV6\nHXCEnJ83nEZu3ZfLP6QGBfA1jy0pZ7NB3xVlvGM6pdwgl8TBjlZUkeKGC4JguHry\nX2eiwuHGzbKDZTvGON9UMv8cW1hCmiY/uICJaA33Y6lBLRwj9Px7EBiTAiMbyPxz\n7e2/XPhcfBupHkqNbE/ZHsECgYBgkIEJXOC2HXF5andeyvlhUvsogTOEInHXuEl+\nbZZWlLvKwr7SxmMSRjmwG8yZ+ISARbGIji0h8+K8HbbwjeH45UcbGbSCio+Nu6Ah\nqIsNK4Nab3sYlA1vmypxxBI9ya6dzvlkbi4p5iZAPNzVPzvc/JWCFQ3QjJkWKA1F\nkIDpcQKBgGtVub8u//x3PS3gQuv/tuUO5CRYCfX/wXSKmPGFzmQjvtRVi1KBnCyW\n77IOVga1onFiszS/EWPtExGRK6peHdmOrdRh7P+Noyj5Sk+BmlXHlhyNZEtg0BDu\nu+QtAXQ4eVL4wc8x5d04M4spSnXOYseG49F5PwOiCnhXK2uD2nUC\n-----END RSA PRIVATE KEY-----\n";
 
        [Fact]
        public void Test1()
        {
            try
            {
                var a = new System.Security.Cryptography.RSACryptoServiceProvider();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new string('a', 128)));
                var creds = new SigningCredentials(key, SecurityAlgorithms.Sha256);

                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, "Bob")
            };

                var token = new JwtSecurityToken(
                    issuer: "issuer.contoso.com",
                    audience: "audience.contoso.com",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                var tokenText = new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
            }
        }

        [Fact]
        public void BuildToken()
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var rsaKey = new RsaSecurityKey(RsaKeyConverter.PrivateKeyFromPem(prk));
                var rsaPubKey = new RsaSecurityKey(RsaKeyConverter.PublicKeyFromPem(puk));
                var creds = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256);
                var claims = new[]
                {
                new Claim("name", "shengyu")
            };
                var token = new JwtSecurityToken(
                        issuer: "issuer.contoso.com",
                        audience: "audience.contoso.com",
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: creds);
                var str = handler.WriteToken(token);
                var token2=handler.ReadToken(str);
                handler.ValidateToken(str, new TokenValidationParameters() {
                    IssuerSigningKey= rsaPubKey,
                    ValidateAudience=false,
                    ValidateIssuer=false
                }, out SecurityToken validatedToken);
            }
            catch (Exception ex)
            {
            }
        }

        [Fact]
        public void ValidateMMToken()
        {
            try
            {
                var mmPubKey = "-----BEGIN PUBLIC KEY-----\nMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDCKNFlpLE2X6KFpj8EznkhvGbtjsRfVn5r7hCLw+5GMEdv9HAo8RE7GYq5iRLvzHUX2kCTwkmSnW2V/U45rm0L7DVKP+4LbxXxe5XwM+7VbmPQ++NjgiyyZ3yIMQQ3vYsKXyCbUiIRokqkn/5XGiC6tBmOoamiiDKt74+597hHtwIDAQAB\n-----END PUBLIC KEY-----\n";
                var handler = new JwtSecurityTokenHandler();
                var rsaPubKey = new RsaSecurityKey(RsaKeyConverter.PublicKeyFromPem(mmPubKey));
                var tokenStr = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJvcnBtcy5ybGFpci5uZXQiLCJleHAiOjE1NjI4MjgyODgsImlzcyI6ImltLnJsYWlyLm5ldCIsInN1YiI6Im9ycG1zIiwidXNlcm5hbWUiOiIifQ.jjHoaGeprk2xqFavlBzO1u6diniFjswXeLhQgxScaoUnYRB17ni7YYIrhYUrHcOmfFY9pvxDsnWV7OUHmmdqTf7xQhIivICdzcVpRTVMJjKAyogYqPZhN6_u8b5vxCNEtwpOVae4acCowrrls6nTxMz700Zdg8Tudz0IJ44n1jU";
                var token = handler.ReadJwtToken(tokenStr);
                handler.ValidateToken(tokenStr, new TokenValidationParameters()
                {
                    IssuerSigningKey = rsaPubKey,
                    ValidateAudience = false,
                    ValidateIssuer = false
                }, out SecurityToken validatedToken);
            }
            catch (Exception ex)
            {
            }
        }
    }

    

    public class RsaKeyConverter
    {
        //
        // 摘要:
        //     /// 根据pem字符串得到私钥 ///
        //
        // 参数:
        //   pem:
        public static RSACryptoServiceProvider PrivateKeyFromPem(string pem)
        {
            using (StringReader reader = new StringReader(pem))
            {
                RsaPrivateCrtKeyParameters rsaPrivateCrtKeyParameters = (RsaPrivateCrtKeyParameters)((AsymmetricCipherKeyPair)new PemReader(reader).ReadObject()).Private;
                RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
                RSAParameters parameters = new RSAParameters
                {
                    Modulus = rsaPrivateCrtKeyParameters.Modulus.ToByteArrayUnsigned(),
                    P = rsaPrivateCrtKeyParameters.P.ToByteArrayUnsigned(),
                    Q = rsaPrivateCrtKeyParameters.Q.ToByteArrayUnsigned(),
                    DP = rsaPrivateCrtKeyParameters.DP.ToByteArrayUnsigned(),
                    DQ = rsaPrivateCrtKeyParameters.DQ.ToByteArrayUnsigned(),
                    InverseQ = rsaPrivateCrtKeyParameters.QInv.ToByteArrayUnsigned(),
                    D = rsaPrivateCrtKeyParameters.Exponent.ToByteArrayUnsigned(),
                    Exponent = rsaPrivateCrtKeyParameters.PublicExponent.ToByteArrayUnsigned()
                };
                rSACryptoServiceProvider.ImportParameters(parameters);
                return rSACryptoServiceProvider;
            }
        }

        //
        // 摘要:
        //     /// 根据pem字符串得到公钥 ///
        //
        // 参数:
        //   pem:
        public static RSACryptoServiceProvider PublicKeyFromPem(string pem)
        {
            using (StringReader reader = new StringReader(pem))
            {
                RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)new PemReader(reader).ReadObject();
                RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
                RSAParameters parameters = new RSAParameters
                {
                    Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned(),
                    Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned()
                };
                rSACryptoServiceProvider.ImportParameters(parameters);
                return rSACryptoServiceProvider;
            }
        }
    }
}
