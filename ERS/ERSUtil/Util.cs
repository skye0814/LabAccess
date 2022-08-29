using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;


namespace ERSUtil
{
    public static class Util
    {
        public static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            // Create an RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings  
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream  
                                // and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();

                            }
                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }

        public static string GetFormFilename(int form)
        {
            // Filename of form
            string result = "";
            FormUtil uploadPathUtil = (FormUtil)form;

            switch (uploadPathUtil)
            {
                case FormUtil.StudentRegistration:
                    result = "StudentRegistration.xlsx";
                    break;
               
                default:
                    break;
            }

            return result;
        }

        public static string GetFormFilePath(int form)
        {
            // Path where contains all forms
            string result = "";
            FormUtil uploadPathUtil = (FormUtil)form;

            switch (uploadPathUtil)
            {
                case FormUtil.StudentRegistration:
                    result = "~/Files/Forms/StudentRegistration";
                    break;
                default:
                    break;
            }

            return result;
        }

        public static string GetUploadFilePath(int uploadPath)
        {
            string result = "";
            UploadPathUtil uploadPathUtil = (UploadPathUtil)uploadPath;

            switch (uploadPathUtil)
            {
                case UploadPathUtil.StudentRegistration:
                    result = "~/Files/Uploads/StudentRegistration";
                    break;
                
                default:
                    break;
            }

            return result;
        }

        public static string GetFormUploadFilename(FormUtil formFilename, string filename)
        {
            string result = "";
            string guid = "";

            result = DateTime.Now.ToString("yyyyMMddHHmmss");
            guid = Guid.NewGuid().ToString("N").Substring(0, 4);

            switch (formFilename)
            {
                case FormUtil.StudentRegistration:
                    result = "StudentRegistration" + result + guid;
                    break;
            }

            result = result.ToUpper() + "_" + filename;

            return result;
        }

        public static string GetUploadSessionId()
        {
            string result = "";
            result = "U" + Guid.NewGuid().ToString("N").Substring(7, 23);
            result = ("" + result).ToUpper();
            return result;
        }

        public static string GetReportSessionId()
        {
            string result = "";
            result = "R" + Guid.NewGuid().ToString("N").Substring(7, 23);
            result = ("" + result).ToUpper();
            return result;
        }

    }
}
