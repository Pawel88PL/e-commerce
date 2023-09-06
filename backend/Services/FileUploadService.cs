using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly DbStoreContext _context;

        public FileUploadService(DbStoreContext context)
        {
            _context = context;
        }

        public async Task<(bool IsSuccess, List<string>? FilePaths, string? ErrorMessage)> UploadFilesAsync(List<IFormFile> files)
        {
            if (files == null || !files.Any())
            {
                return (false, null, "Brak jakichkolwiek zdjęć.");
            }

            var uploadedPaths = new List<string>();
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var fileNumber = 1;

                while (File.Exists(Path.Combine(uploadPath, $"{fileName}{extension}")))
                {
                    fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}({fileNumber++})";
                }

                var filePath = Path.Combine(uploadPath, $"{fileName}{extension}");
                
                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    uploadedPaths.Add($"/images/{fileName}{extension}");
                }
                catch (Exception ex)
                {
                    return (false, null, ex.Message);
                }
            }
            return (true, uploadedPaths, null);
        }
    }
}