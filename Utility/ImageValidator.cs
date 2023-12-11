using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class ImageValidator
    {
        public static bool Control(IEnumerable<IFormFile> files)
        {
            if (files == null || (files != null && files.Count() == 0)) return true;
            string[] available_extensions = new string[] { ".jpeg", ".jpg", ".png" };
            foreach (var file in files)
            {
                // getting extension of file
               var extensiong = Path.GetExtension(file.FileName);
                if (!available_extensions.Contains(extensiong))
                {
                    return false;
                }
            }
            return true;
        }

    }
}
