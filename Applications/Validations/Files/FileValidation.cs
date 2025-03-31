using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Validations.Files
{
    public class FileValidation
    {
        private readonly string[] permittedExtensions = { ".txt", ".pdf", ".docx", ".jpg", ".png" };

        public bool IsValidFileExtension(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return !string.IsNullOrEmpty(ext) && permittedExtensions.Contains(ext);
        }


    }
}

