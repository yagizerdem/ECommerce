using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Utility
{
    public class ImageDownloadHelper
    {
         
        public static List<string> SaveFile(List<IFormFile> files , string foldername , IWebHostEnvironment webhostEnviroment)
        {
            string rootpath = webhostEnviroment.WebRootPath;
            string[] paths = { rootpath, "Images", foldername };
            string folderPath = Path.Combine(paths);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            List<string> result = new List<string>(); // retunr paths 

            // Save the file to the specified path
            foreach (IFormFile file in files)
            {
                string uniqueFileName = file.FileName;
                string filePath = Path.Combine(folderPath, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                result.Add(Path.Combine("~\\Images", foldername, uniqueFileName));   
            }
            return result;
        }


    }
}
