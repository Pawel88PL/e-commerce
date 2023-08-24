using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class FileUploadService : IFileUploadService
    {
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return string.Empty;
            }

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, file.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/images/{file.FileName}";
        }

        public async Task<List<string>> UploadFilesAsync(List<IFormFile> files)
        {
            List<string> uploadedFilesPaths = new List<string>();
            foreach (var file in files)
            {
                var filePath = await UploadFileAsync(file);
                if (!string.IsNullOrEmpty(filePath))
                {
                    uploadedFilesPaths.Add(filePath);
                }
            }
            return uploadedFilesPaths;
        }
    }
}
