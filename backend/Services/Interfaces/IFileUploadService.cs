using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task<List<string>> UploadFilesAsync(List<IFormFile> files);
    }
}
