using ERSEntity.Entity;
using ERSUtil;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSBLL.FileManager
{
    public class FileManagerBLL
    {
        public FileManagerBLL()
        {
        }

        public FileManagerEntity Save(FileManagerEntity objFileManager)
        {
            FileManagerEntity result = objFileManager;

            if (result == null)
                result = new FileManagerEntity();

            result.MessageList = new List<string>();

            objFileManager.Path = ("" + objFileManager.Path).Trim();
            objFileManager.Filename = ("" + objFileManager.Filename).Trim();

            if (string.IsNullOrEmpty(objFileManager.Path))
                result.MessageList.Add("File Path " + MessageUTIL.SUFF_ERRMSG_INPUT_REQUIRED);
            if (string.IsNullOrEmpty(objFileManager.Filename))
                result.MessageList.Add("Filename " + MessageUTIL.SUFF_ERRMSG_INPUT_REQUIRED);
            if (objFileManager.Byte == null)
                result.MessageList.Add("File " + MessageUTIL.SUFF_ERRMSG_INPUT_REQUIRED);

            #region Check if PDF is not corrupted
            try
            {
                StringBuilder text = new StringBuilder();

                if (File.Exists(System.IO.Path.Combine(objFileManager.Path, objFileManager.Filename)))
                {
                    PdfReader pdfReader = new PdfReader(System.IO.Path.Combine(objFileManager.Path, objFileManager.Filename));

                    for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                    {
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                        currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                        text.Append(currentText);
                    }
                    pdfReader.Close();
                }
            }
            catch (Exception ex)
            {
                result.MessageList.Add(MessageUTIL.ERROR_PDF_FILE);
            }
            #endregion

            if (result.MessageList.Count() == 0)
            {
                try
                {

                    if (!Directory.Exists(objFileManager.Path))
                        Directory.CreateDirectory(objFileManager.Path);

                    using (FileStream fs = new FileStream(System.IO.Path.Combine(objFileManager.Path, objFileManager.Filename), FileMode.Create))
                    {
                        fs.Write(objFileManager.Byte, 0, objFileManager.Byte.Length);
                        fs.Flush();
                    }

                    result.IsSuccess = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                objFileManager = null;

               
            }

            return result;
        }

        public void Delete(FileManagerEntity objFileManager)
        {
          
            try
            {
                File.Delete(System.IO.Path.Combine(objFileManager.Path, objFileManager.Filename));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
