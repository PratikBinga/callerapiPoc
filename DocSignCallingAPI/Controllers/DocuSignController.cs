using DocSignCallingAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DocSignCallingAPI.Controllers
{
    public class DocuSignController : Controller
    {
        //Hosted web API REST Service base url  
        string Baseurl = "https://localhost:44349/";
        // GET: DocuSign
        public ActionResult SendDocumentforSign()



        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SendDocumentforSign(Recipient recipient, HttpPostedFileBase UploadDocument)
        {

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

            DocuSignRequestData inputModel = new DocuSignRequestData();
            inputModel.fileData = DocumentBase64;
            inputModel.recipients = recipient.Email;

            using (var client = new HttpClient())
            {
                // as of now the url is hardcoded we can get it from above call of geturl.
                // client.PostAsync()
                var res = await client.PostAsync("https://localhost:44349/api/DocuSignPost/SendDocumentforSign", new StringContent(JsonConvert.SerializeObject(inputModel), Encoding.UTF8, "application/json"));

                try
                {
                    res.EnsureSuccessStatusCode();
                    //res.Result.EnsureSuccessStatusCode();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

            }

            //using (var client = new HttpClient())
            //{
            //    //Passing service base url  
            //    client.BaseAddress = new Uri(Baseurl);

            //    client.DefaultRequestHeaders.Clear();
            //    //Define request data format  
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
            //   // HttpResponseMessage Res = await client.PostAsync("https://localhost:44349/api/DocuSignPost");

            //    //Checking the response is successful or not which is sent using HttpClient  
            //    if (Res.IsSuccessStatusCode)
            //    {
            //        //Storing the response details recieved from web api   
            //        var Response = Res.Content.ReadAsStringAsync().Result;
            //    }
            //}

            return View();
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