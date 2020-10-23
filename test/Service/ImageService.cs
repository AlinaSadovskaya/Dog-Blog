using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace test.Service
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IConfiguration _configuration;
        public ImageService(IWebHostEnvironment appEnvironment, IConfiguration configuration)
        {
            _configuration = configuration;
            _appEnvironment = appEnvironment;
        }
        public async Task<string> SaveImageAsync(IFormFile uploadedFile, int flag)
        {
            if (uploadedFile != null)
            {
                string path = "";
                if (flag == 0)
                { 
                    path = _configuration["ImagePathDogs"] + uploadedFile.FileName;
                }
                else
                {
                    path = _configuration["ImagePathPosts"] + uploadedFile.FileName;
                }
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                return path;
            }
            return "";
        }
    }
}
