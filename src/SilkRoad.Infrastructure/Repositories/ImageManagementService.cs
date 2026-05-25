using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using SilkRoad.Core.Services;

namespace SilkRoad.Infrastructure;

internal class ImageManagementService : IImageManagementService
{
    private readonly IFileProvider _fileProvider;

    public ImageManagementService(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public void DeleteImagesAsync(string src)
    {
        string wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        string imagePath = Path.Combine(wwwrootPath, src.TrimStart('/'));
        if (File.Exists(imagePath))
        {
            File.Delete(imagePath);
        }
    }

    public async Task<List<string>> UploadImagesAsync(IFormFileCollection imageFiles, string src)
    {
        List<string> uploadedImageUrls = new List<string>();
        string uploadPath = Path.Combine("wwwroot", "Images", src);

        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }
        foreach (IFormFile imageFile in imageFiles)
        {
            if (imageFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }
                uploadedImageUrls.Add($"/Images/{src}/{fileName}");
            }
        }

        return uploadedImageUrls;
    }
}
