using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class ImageDeleteHelper
    {

        public static void RemoveImage(string path , IWebHostEnvironment _webHostEnvironment)
        {
            // Combine the wwwroot path with the file name
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, path.Substring(2));

            // Check if the file exists before attempting to delete
            if (System.IO.File.Exists(filePath))
            {
                // Delete the file
                System.IO.File.Delete(filePath);
            }
        }
    }
}
