using Microsoft.AspNetCore.Mvc;
using MiodOdStaniula.Controllers;
using MiodOdStaniula.Services.Interfaces;
namespace MiodOdStaniula;

[ApiController]
[Route("api/[controller]")]
public class MediaController : ControllerBase
{
    private readonly IImageService _imageService;

    public MediaController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpDelete("{id}")]
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
            return BadRequest($"Error uploading files. Reason: {result.ErrorMessage}");
        }
    }
}