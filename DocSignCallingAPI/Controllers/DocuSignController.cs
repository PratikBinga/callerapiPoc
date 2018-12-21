using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace DocSignCallingAPI.Controllers
{
    public class DocuSignController : Controller
    {
        //Hosted web API REST Service base url  
        string Baseurl = "http://192.168.95.1:5555/";
        // GET: DocuSign
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendDocumentforCallerAPI(Models.Recipient recipient, HttpPostedFileBase UploadDocument)
        {
            Models.Recipient recipientModel = new Models.Recipient();

            byte[] fileBytes;
            using (Stream inputStream = UploadDocument.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                fileBytes = memoryStream.ToArray();
            }

            var DocumentBase64 = System.Convert.ToBase64String(fileBytes);
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.PostAsync("api/",);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = Res.Content.ReadAsStringAsync().Result;
                }
            }

            return View(Response);
        }
        [HttpPost]
        public ActionResult ReciveSignedDoc(string data, string email)
        {
            var addr = new System.Net.Mail.MailAddress(email);
            string directorypath = Server.MapPath("~/App_Data/" + "Files/");
            if (!Directory.Exists(directorypath))
            {
                Directory.CreateDirectory(directorypath);
            }
            var serverpath = directorypath + addr.DisplayName.Trim() + ".pdf";
            System.IO.File.WriteAllBytes(serverpath, Convert.FromBase64String(data));
            return View(serverpath);
        }
    }
}