using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using System.Web.Helpers;
using ERSEntity.Entity;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using ERSUtil;
using Microsoft.Build.Tasks;
using Microsoft.Reporting.WebForms;
using Warning = Microsoft.Reporting.WebForms.Warning;

namespace ERS.Controllers
{
    public class CommonController : SecurityController
    {
        public ResultEntity _resultEntity = new ResultEntity();
        public ReportParameterEntity _objReportParameter = null;

        public Exception _exception = null;


        public sealed class ValidateHeaderAntiForgeryToken : FilterAttribute, IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationContext filterContext)
            {
                if (filterContext == null)
                {
                    throw new ArgumentNullException("filterContext");
                }

                var httpContext = filterContext.HttpContext;
                var cookie = httpContext.Request.Cookies[AntiForgeryConfig.CookieName];
                AntiForgery.Validate(cookie != null ? cookie.Value : null, httpContext.Request.Headers["__RequestVerificationToken"]);
            }

        }
        public string QRGenerate(string encoded)
        {
            string QRCode = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(encoded, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    bitMap.Save(ms, ImageFormat.Png);
                    QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }

            return QRCode;
        }

        public virtual ActionResult DownloadForm(int id)
        {
            string mimeType = "";
            string filename = "";

            filename = Util.GetFormFilename(id);

            string file = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(Util.GetFormFilePath(id)), filename);

            if (string.IsNullOrEmpty(filename))
            {
                TempData["ErrorMessage"] = "File does not exists.";
                return RedirectPermanent("~/Home/ErrorPage");
            }
            else
            {
                mimeType = MimeMapping.GetMimeMapping(file);
            }

            return File(file, mimeType, filename);
        }

        // You can over ride this if you need to implement your own logic
        public virtual ActionResult DownloadReport(string id)
        {
            Warning[] warnings = null;
            string[] streams = null;
            byte[] renderedBytes = null;
            string mimeType = null;
            LocalReport lr = null;
            ReportDataSource rd = null;
            string encoding = null;
            string fileNameExtension = null;
            string outputFormat = null;
            string path = null;
            string deviceInfo = null;
            Exception error = null;
            List<ReportParameter> reportParameters = null;

            try
            {
                _objReportParameter = (ReportParameterEntity)Session[id];

                if (_objReportParameter == null)
                    throw new Exception("Oops report cannot be downloaded. File is missing please try again.");

                path = Path.Combine(_objReportParameter.ReportPath, _objReportParameter.ReportFilename);

                if (System.IO.File.Exists(path))
                {
                    try
                    {
                        lr = new LocalReport();
                        rd = new ReportDataSource(_objReportParameter.DataSet, _objReportParameter.Data);
                        lr.DataSources.Add(rd);
                        lr.ReportPath = path;
                        lr.EnableExternalImages = true;

                        reportParameters = (List<ReportParameter>)_objReportParameter.ReportParameters;
                        if (reportParameters != null && reportParameters.Count() > 0)
                        {
                            lr.SetParameters(reportParameters);
                        }

                        lr.Refresh();

                        outputFormat = "EXCEL";//_objReportParameter.FileType.ToString();
                        deviceInfo = "<DeviceInfo><OutputFormat>" + outputFormat + "</OutputFormat>";


                        deviceInfo += "</DeviceInfo>";


                        //lr.ReportPath = path;
                        renderedBytes = lr.Render(
                            outputFormat,
                            deviceInfo,
                            out mimeType,
                            out encoding,
                            out fileNameExtension,
                            out streams,
                            out warnings);
                        Response.AddHeader("content-disposition", "attachment; filename=" + _objReportParameter.Filename + "." + fileNameExtension);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                    Session[id] = null;
                    Session.Remove(id);
                }
                else
                {
                    throw new Exception("Oops report cannot be downloaded. File is missing please try again.");
                }
            }
            catch (Exception ex)
            {
                error = ex;
            }

            _objReportParameter = null;

            rd = null;
            lr = null;
            warnings = null;
            reportParameters = null;
            streams = null;
            outputFormat = null;
            encoding = null;
            fileNameExtension = null;
            outputFormat = null;
            path = null;
            deviceInfo = null;
            reportParameters = null;

            if (error != null)
            {
                TempData["ErrorMessage"] = "" + error.Message;
                return RedirectPermanent("~/Home/ErrorPage");
            }

            return File(renderedBytes, mimeType);
        }
    }
}