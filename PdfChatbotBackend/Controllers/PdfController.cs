using Microsoft.AspNetCore.Mvc;
using PdfChatbotBackend.Models;
using PdfChatbotBackend.Services;

namespace PdfChatbotBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PdfController : ControllerBase
{
    private readonly PdfService _pdfService;
    private readonly ILogger<PdfController> _logger;

    public PdfController(PdfService pdfService, ILogger<PdfController> logger)
    {
        _pdfService = pdfService;
        _logger = logger;
    }

    [HttpPost("upload")]
    public async Task<ActionResult<PdfUploadResponse>> UploadPdf(IFormFile file)
    {
        try
        {
            _logger.LogInformation("Received PDF upload request for file: {FileName}", file.FileName);
            
            var response = _pdfService.ProcessPdf(file);
            
            if (!response.Success)
            {
                _logger.LogWarning("PDF processing failed: {Message}", response.Message);
                return BadRequest(response);
            }

            _logger.LogInformation("PDF processed successfully. Pages: {PageCount}", response.PageCount);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing PDF upload");
            return StatusCode(500, new PdfUploadResponse
            {
                Success = false,
                Message = "An error occurred while processing the PDF."
            });
        }
    }
} 