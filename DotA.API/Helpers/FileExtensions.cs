using System.IO;
using Microsoft.AspNetCore.Http;

namespace DotA.API.Helpers
{
    public static class FileExtensions
    {
        public static byte[] ToByteArray(this IFormFile formFile)
        {
            using var ms = new MemoryStream();
            formFile.CopyTo(ms);
            return ms.ToArray();
        }
    }
}