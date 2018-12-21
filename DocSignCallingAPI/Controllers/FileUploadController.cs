using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 


namespace DocSignCallingAPI.Controllers
{
    public class FileUploadController : System.Web.Mvc.Controller
    {
        // GET: FileUpload
        public System.Web.Mvc.ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Http post to handle file uploads
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public async Task<IActionResult> UploadFile(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            var filePath = "";

            foreach (var formFile in files)
            {
                filePath = Path.Combine(
                       Directory.GetCurrentDirectory(), "UploadedFiles",
                       formFile.FileName);

                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return Ok(new { count = files.Count, size, filePath });
        }


    }
}