using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ERSUtil
{
    public class EncryptionUTIL
    {
        private byte[] initVec;
        private byte[] encKey;

        internal byte[] IV
        {
            get { return initVec; }
            set { initVec = value; }
        }
        internal byte[] Key
        {
            get { return encKey; }
        }

        internal ICryptoTransform EncryptServiceProvider(byte[] bytesKey, byte[] initVec)
        {
            using (Rijndael rijndael = new RijndaelManaged())
            {
                rijndael.Mode = CipherMode.CBC;
                // Test to see if a key was provided
                if (null == bytesKey)
                {
                    encKey = rijndael.Key;
                }
                else
                {
                    rijndael.Key = bytesKey;
                    encKey = rijndael.Key;
                }
                // See if the client provided an IV
                if (null == initVec)
                { //Yes, have the alg create one
                    initVec = rijndael.IV;
                }
                else
                { //No, give it to the alg.
                    rijndael.IV = initVec;
                }
                return rijndael.CreateEncryptor();
            }
        }

        private byte[] Encrypt(byte[] bytesData, byte[] bytesKey, byte[] initVec, ref string r_strErrMsg)
        {
            //Set up the stream that will hold the encrypted data.
            using (MemoryStream memStreamEncryptedData = new MemoryStream())
            {
                //Pass in the initialization vector.            
                IV = initVec;

                ICryptoTransform transform = EncryptServiceProvider(bytesKey, initVec);
                CryptoStream encStream = new CryptoStream(memStreamEncryptedData, transform, CryptoStreamMode.Write);
                try
                {
                    //Encrypt the data, write it to the memory stream.
                    encStream.Write(bytesData, 0, bytesData.Length);
                }
                catch (Exception ex)
                {
                    r_strErrMsg = ex.Message.ToString();
                }
                //Set the IV and key for the client to retrieve
                encKey = Key;
                encStream.FlushFinalBlock();

                //Send the data back.
                return memStreamEncryptedData.ToArray();
            }
        }//end Encrypt       

        internal ICryptoTransform DecryptServiceProvider(byte[] bytesKey, byte[] initVec)
        {
            using (Rijndael rijndael = new RijndaelManaged())
            {
                rijndael.Mode = CipherMode.CBC;
                return rijndael.CreateDecryptor(bytesKey, initVec);
            }
        }

        private byte[] Decrypt(byte[] bytesData, byte[] bytesKey, byte[] initVec, ref string r_strErrMsg)
        {
            //Set up the memory stream for the decrypted data.
            using (MemoryStream memStreamDecryptedData = new MemoryStream())
            {

                //Pass in the initialization vector.            
                IV = initVec;

                ICryptoTransform transform = DecryptServiceProvider(bytesKey, initVec);
                CryptoStream decStream = new CryptoStream(memStreamDecryptedData, transform, CryptoStreamMode.Write);
                try
                {
                    decStream.Write(bytesData, 0, bytesData.Length);
                }
                catch (Exception ex)
                {
                    r_strErrMsg = ex.Message.ToString();
                }

                decStream.FlushFinalBlock();

                // Send the data back.
                return memStreamDecryptedData.ToArray();
            }
        } //end Decrypt

        /// <summary>
        /// Public function to encrypt a plain text using symetric algorithm
        /// </summary>
        /// <param name="Value">String value to be encrypted</param>
        /// <param name="ErrorMsg">Passing by reference that return an error string</param>
        /// <returns>String result (encrypted) based on the plain text value</returns>
        public string Encode(string Value, ref string ErrorMsg)
        {
            string strErrMsg = "";

            // Init variables.
            byte[] IV = null;
            byte[] cipherText = null;
            byte[] key = null;

            try
            { //Try to encrypt.
                //Create the encryptor.
                EncryptionUTIL enc = new EncryptionUTIL();

                byte[] plainText = Encoding.ASCII.GetBytes(Value);

                //Rijndael
                key = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["keybytes"].ToString());// Encoding.ASCII.GetBytes("abcdef0123456789");
                IV = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["ivbytes"].ToString());//Encoding.ASCII.GetBytes("abcdef0123456789");


              
                // Perform the encryption.
                cipherText = enc.Encrypt(plainText, key, IV, ref strErrMsg);
                // Retrieve the intialization vector and key. You will need it 
                // for decryption.

                return Convert.ToBase64String(cipherText);
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                return "";
            }
        }

        /// <summary>
        /// Public function to decrypt the decrpted value using symetric algorithm
        /// </summary>
        /// <param name="Value">String value to be decrypted</param>
        /// <param name="ErrorMsg">Passing by reference that return an error string</param>
        /// <returns>String result (plain text) based on the encrypted value</returns>
        public string Decode(string Value, ref string ErrorMsg)
        {
            string strErrMsg = "";

            // Init variables.
            byte[] IV = null;
            byte[] key = null;

            try
            {
                byte[] cipherText = Convert.FromBase64String(Value);

                //Rijndael
                key = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["keybytes"].ToString());// Encoding.ASCII.GetBytes("abcdef0123456789");
                IV = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["ivbytes"].ToString());//Encoding.ASCII.GetBytes("abcdef0123456789");


                //Try to decrypt.
                //Set up your decryption, give it the algorithm and initialization vector.
                EncryptionUTIL dec = new EncryptionUTIL();
                dec.IV = IV;
                // Go ahead and decrypt.

                byte[] plainText = dec.Decrypt(cipherText, key, IV, ref strErrMsg);
                // Look at your plain text.
                return Encoding.ASCII.GetString(plainText);
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                return "";
            }
        }

        /// <summary>
        /// Public function to encrypt the entire file using symetric algorithm
        /// </summary>
        /// <param name="inputFile">String value of the physical file to be encrypted</param>
        /// <param name="outputFile">String value of the physical result</param>
        /// <param name="errorMsg">Passing by reference that return an error string</param>
        public void EncryptFile(string inputFile, string outputFile, ref string errorMsg)
        {

            try
            {
                string cryptFile = outputFile;
                using (FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create))
                {
                    using (RijndaelManaged RMCrypto = new RijndaelManaged())
                    {
                        byte[] key = Encoding.ASCII.GetBytes("abcdef0123456789");
                        byte[] IV = Encoding.ASCII.GetBytes("init vec is big.");

                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateEncryptor(key, IV),
                            CryptoStreamMode.Write);

                        using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                        {
                            int data;
                            while ((data = fsIn.ReadByte()) != -1)
                                cs.WriteByte((byte)data);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }
        }

        /// <summary>
        /// Public function to descrypt the entire file using symetric algorithm
        /// </summary>
        /// <param name="inputFile">String value of the physical file to be decrypted</param>
        /// <param name="outputFile">String value of the physical result</param>
        /// <param name="errorMsg">Passing by reference that return an error string</param>
        public void DecryptFile(string inputFile, string outputFile, ref string errorMsg)
        {
            try
            {
                using (FileStream fsCrypt = new FileStream(inputFile, FileMode.OpenOrCreate, FileAccess.Read))
                {

                    using (RijndaelManaged RMCrypto = new RijndaelManaged())
                    {

                        byte[] key = Encoding.ASCII.GetBytes("abcdef0123456789");
                        byte[] IV = Encoding.ASCII.GetBytes("init vec is big.");

                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateDecryptor(key, IV),
                            CryptoStreamMode.Read);

                        using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
                        {
                            int data;
                            while ((data = cs.ReadByte()) != -1)
                                fsOut.WriteByte((byte)data);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }
        }

        /// <summary>
        /// Public function to encrypt the entire file using symetric algorithm
        /// </summary>
        /// <param name="inputFile">String value of the physical file to be encrypted</param>
        /// <param name="outputFile">String value of the physical result</param>
        /// <param name="errorMsg">Passing by reference that return an error string</param>
        public void EncryptText(string inputFile, string outputFile, ref string errorMsg)
        {
            try
            {

                using (RijndaelManaged RMCrypto = new RijndaelManaged())
                {
                    byte[] key = Encoding.ASCII.GetBytes("abcdef0123456789");
                    byte[] IV = Encoding.ASCII.GetBytes("init vec is big.");


                    using (FileStream fsCrypt = new FileStream(outputFile, FileMode.Create))
                    {
                        fsCrypt.Position = 0;

                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateEncryptor(key, IV),
                            CryptoStreamMode.Write);

                        byte[] byteArray = Encoding.ASCII.GetBytes(inputFile);

                        cs.Write(byteArray, 0, byteArray.Length);
                        cs.FlushFinalBlock();

                    }
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }
        }

        /// <summary>
        /// Public function to descrypt the entire file using symetric algorithm
        /// </summary>
        /// <param name="inputFile">String value of the physical file to be decrypted</param>
        /// <param name="outputFile">String value of the physical result</param>
        /// <param name="errorMsg">Passing by reference that return an error string</param>
        public string DecryptText(string inputFile, ref string errorMsg)
        {
            string strTest = String.Empty;
            try
            {

                using (RijndaelManaged RMCrypto = new RijndaelManaged())
                {

                    byte[] key = Encoding.ASCII.GetBytes("abcdef0123456789");
                    byte[] IV = Encoding.ASCII.GetBytes("init vec is big.");

                    using (FileStream fsCrypt = File.Open(inputFile, FileMode.OpenOrCreate, FileAccess.Read))
                    {
                        CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(key, IV), CryptoStreamMode.Read);
                        using (MemoryStream fsOut = new MemoryStream())
                        {
                            cs.CopyTo(fsOut);
                            strTest = Encoding.UTF8.GetString(fsOut.ToArray());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }
            return strTest;
        }

        [Flags]
        public enum HashCrypt { MD5 = 0, SHA1 };

        public string ComputeHash(string plainText, HashCrypt hashAlgorithm)
        {
            // Convert plain text into a byte array.
            byte[] bs = Encoding.UTF8.GetBytes(plainText);

            // Because we support multiple hashing algorithms, we must define
            // hash object as a common (abstract) base class. We will specify the
            // actual hashing algorithm class later during object creation.
            HashAlgorithm hash;

            // Initialize appropriate hashing algorithm class.
            switch (hashAlgorithm)
            {
                case HashCrypt.SHA1:
                    hash = new SHA1Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            // Compute hash value of our plain text
            try
            {
                bs = hash.ComputeHash(bs);

                StringBuilder s = new StringBuilder();

                foreach (byte b in bs)
                {
                    s.Append(b.ToString("x2").ToLower());
                }
                return s.ToString();
            }
            finally
            {
                hash.Dispose();
            }
        }

        public String ShaEncryption(string plaintext, ref string ErrorMsg)
        {
            try
            {
                byte[] byteValue = Encoding.UTF8.GetBytes(plaintext);
                byte[] byteResult;
                string result = "";
                using (SHA512 encrypt = new SHA512Managed())
                {
                    byteResult = encrypt.ComputeHash(byteValue);
                    foreach (byte x in byteResult)
                    {
                        result += String.Format("{0:x2}", x);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                return "";
            }
        }
    }
}
