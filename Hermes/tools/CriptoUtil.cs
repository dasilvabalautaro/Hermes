using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Hermes.tools
{
    class CriptoUtil
    {        
        UnicodeEncoding ByteConverter = new UnicodeEncoding();
        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
        RSAParameters privateKey = new RSAParameters();
        RSAParameters publicKey = new RSAParameters();
                
        byte[] plaintext;
        byte[] encryptedtext;

        public CriptoUtil()
        {
            RSA.FromXmlString(Constants.keyRSA);
            privateKey = RSA
                .ExportParameters(true);
            publicKey = RSA
                    .ExportParameters(false);
                          
        }

        private byte[] encryption(byte[] Data, 
            RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    encryptedData = RSA.Encrypt(Data, DoOAEPPadding);
                }
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private byte[] decryption(byte[] Data, 
            RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    decryptedData = RSA.Decrypt(Data, DoOAEPPadding);
                }
                return decryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        public string encript(string strInput)
        {
            plaintext = ByteConverter.GetBytes(strInput);
            encryptedtext = encryption(plaintext,
                publicKey, false);

            return Convert.ToBase64String(encryptedtext);
        }
        public string desencript(string strInput)
        {
            byte[] inputBytes = Convert.FromBase64String(strInput);
            byte[] decryptedtex = decryption(inputBytes,
                privateKey, false);
            return ByteConverter.GetString(decryptedtex);
        }
    }
}
