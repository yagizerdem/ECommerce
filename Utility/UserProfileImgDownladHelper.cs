using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class UserProfileImgDownladHelper
    {
        public static string SaveFile(IFormFile file, string foldername, IWebHostEnvironment webhostEnviroment)
        {
            string rootpath = webhostEnviroment.WebRootPath;
            string[] paths = { rootpath, "UserProfileImages", foldername };
            string folderPath = Path.Combine(paths);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }


            // Save the file to the specified path
            
                string uniqueFileName = file.FileName;
                string filePath = Path.Combine(folderPath, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            string result = Path.Combine("~\\UserProfileImages", foldername, uniqueFileName);
            return result;
        }
    }
}
