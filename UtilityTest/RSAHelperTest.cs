using Org.BouncyCastle.OpenSsl;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Utility;
using Xunit;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;

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
        public string privateKeyPem = @"-----BEGIN RSA PRIVATE KEY-----
MIICWwIBAAKBgQCmRrRnPotdRCXK8E8Lzzd48vgjleUjZJRGwtYv0HE9uHsZhBzf
UncJ2YGYMQ97IHle41cD4FMcg3W/JVg0FvHojyEj/tKCIH5ADaUbHhRrixAfDEjC
Kg4SITUWxSVyfhijv1GTmX2Dx4wgcL56j/slpuUJfBUKaamcGVEmV1F7XwIDAQAB
AoGAKXEJ2YmfFnm7qZ7HNLxKqRx1d/kOCQoyYoiA9Z3P+4AVPkDNKWPWQ2Awiov/
vcJUPbAPqempDTw+hot6NlFZrSN4kQT3myWvnYw/ruHJ0XMKMaix1oO6jrIxEa+C
TF7lPPCDxuG7WQMq+U2rmJ7jE+Ipw+EgPXaN1s/Kf0db2zECQQDgkW6cLzK54YKR
b46P0HLZqY2H99VjtPIlSYf9xlo9HfviIx9fPPctyHCr8swq6C6ScUooThO9pdbK
awmtaBOjAkEAvYyY/lJvwrmnI0so6olB3yO6gMgtGlZ0MrdOcs0WqvcFr3fgAGpy
WzLcvVpEoIKbN7i1CCtqy480x3cRod6VFQJAOXCZhTOBWxA2cHLDWT+tEMWQoPWg
TDeNNEJhmWSx0i4oLkhjjt2uL7S0NRcOZ+8pcmWt3S9TV0/i57WHLSaQ1wJABUFV
qI9ui86L5L2bt8zwZ5hc/l8OaRGGjTVp1mL7QugwXyoKqthIrWCeoB1Vk8GrPgM/
+acCgfxJcVJKydsa9QJAK6SA/bDNk2+Bn+edrrHQKcEsrQYiaa9PbQ0Z2VqeRxNO
fJZuRoT2uHsj7kJN0x/pIpvB/gRhwMAvoJWW5EgcpA==
-----END RSA PRIVATE KEY-----";
        public string publicKeyPem = @"-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCmRrRnPotdRCXK8E8Lzzd48vgj
leUjZJRGwtYv0HE9uHsZhBzfUncJ2YGYMQ97IHle41cD4FMcg3W/JVg0FvHojyEj
/tKCIH5ADaUbHhRrixAfDEjCKg4SITUWxSVyfhijv1GTmX2Dx4wgcL56j/slpuUJ
fBUKaamcGVEmV1F7XwIDAQAB
-----END PUBLIC KEY-----";
        public RSAHelperTest()
        {
        }

        #region rsa public key and private key
        [Fact]
        public void KeyTest1()
        {

            try
            {
                var keys = RSAHelper.GeneratePemRsaKeys();
                var text = privateKeyPem;
                var isv=RSAHelper.Verify(text, RSAHelper.Sign(text, keys.privateKey), keys.publicKey);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}
