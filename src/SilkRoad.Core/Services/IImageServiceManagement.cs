using Microsoft.AspNetCore.Http;

namespace SilkRoad.Core.Services;


public interface IImageManagementService
{
    void DeleteImagesAsync(string src);
    Task<List<string>> UploadImagesAsync(IFormFileCollection imageFiles,string src);
}