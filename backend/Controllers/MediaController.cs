using Microsoft.AspNetCore.Mvc;
using MiodOdStaniula.Controllers;
using MiodOdStaniula.Services.Interfaces;
namespace MiodOdStaniula;

[ApiController]
[Route("api/[controller]")]
public class MediaController : ControllerBase
{
    private readonly IFileUploadService _fileUploadService;

    public MediaController(IFileUploadService fileUploadService)
    {
        _fileUploadService = fileUploadService;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> images)
    {
        if (images == null || images.Count == 0)
            return BadRequest("No files received");

        var result = await _fileUploadService.UploadFilesAsync(images);

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