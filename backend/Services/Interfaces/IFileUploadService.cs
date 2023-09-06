using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface IFileUploadService
    {
        Task<(bool IsSuccess, List<string>? FilePaths, string? ErrorMessage)> UploadFilesAsync(List<IFormFile> files);
    }
}
