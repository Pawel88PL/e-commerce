using backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("media")]
    public class MediaController : ControllerBase
    {
        private readonly IImageService _imageService;

        public MediaController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var result = await _imageService.DeleteImageAsync(id);
            if (!result.Success)
            {
                return NotFound(new { message = result.ErrorMessage });
            }
            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> images)
        {
            if (images == null || images.Count == 0)
                return BadRequest("No files received");

            var result = await _imageService.UploadImageAsync(images);

            if (result.IsSuccess)
            {
                return Ok(new { files = result.FilePaths });
            }
            else
            {
                return BadRequest($"Wysąpił błąd podczas udostępniania zdjęcia. Treśc błędu: {result.ErrorMessage}");
            }
        }
    }
}