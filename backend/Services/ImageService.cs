using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class ImageService : IImageService
    {
        private readonly DbStoreContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(DbStoreContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ServiceResult<bool>> DeleteImageAsync(int id)
        {
            try
            {
                var image = await _context.ProductImages!.FindAsync(id);
                if (image == null)
                {
                    return new ServiceResult<bool>
                    {
                        Success = false,
                        ErrorMessage = "Nie znaleziono zdjęcia!"
                    };
                }
                else
                {
                    var fileName = Path.GetFileName(image.ImagePath);
                    if (string.IsNullOrEmpty(fileName))
                    {
                        return new ServiceResult<bool>
                        {
                            Success = false,
                            ErrorMessage = "Nieprawidłowa ścieżka zdjęcia!"
                        };
                    }

                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
                    
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                    _context.ProductImages.Remove(image);
                    await _context.SaveChangesAsync();
                    return new ServiceResult<bool> { Data = true, Success = true };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>
                {
                    Success = false,
                    ErrorMessage = $"Wystąpił błąd podczas usuwania zdjęcia produktu: {ex.Message}!"
                };
            }
        }

        public async Task<(bool IsSuccess, List<string>? FilePaths, string? ErrorMessage)> UploadImageAsync(List<IFormFile> files)
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