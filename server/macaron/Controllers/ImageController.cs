using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Macaron.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/images")]
    public class ImageController : Controller
    {

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task Upload(IFormFile file, string fileName)
        {
            if(file.Length > 0)
            {
                using (var stream = new FileStream(Path.Combine("assets", "images", fileName), FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
        }
    }
}
