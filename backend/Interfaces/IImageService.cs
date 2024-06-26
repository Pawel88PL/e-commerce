using backend.Services;

namespace backend.Interfaces
{
    public interface IImageService
    {
        Task<ServiceResult<bool>> DeleteImageAsync(int id); 
        Task<(bool IsSuccess, List<string>? FilePaths, string? ErrorMessage)> UploadImageAsync(List<IFormFile> files);
    }
}
