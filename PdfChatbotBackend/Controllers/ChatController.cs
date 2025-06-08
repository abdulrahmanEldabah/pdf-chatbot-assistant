using Microsoft.AspNetCore.Mvc;
using PdfChatbotBackend.Models;
using PdfChatbotBackend.Services;

namespace PdfChatbotBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly PdfService _pdfService;
    private readonly ILogger<ChatController> _logger;

    public ChatController(
        IChatService chatService,
        PdfService pdfService,
        ILogger<ChatController> logger)
    {
        _chatService = chatService;
        _pdfService = pdfService;
        _logger = logger;
    }

    [HttpPost("ask")]
    public async Task<ActionResult<ChatResponse>> AskQuestion([FromBody] ChatRequest request)
    {
        try
        {
            if (request == null)
            {
                _logger.LogWarning("Received null request");
                return BadRequest(new ChatResponse
                {
                    Success = false,
                    Message = "Request body cannot be null."
                });
            }

            if (string.IsNullOrWhiteSpace(request.Question))
            {
                _logger.LogWarning("Received empty question");
                return BadRequest(new ChatResponse
                {
                    Success = false,
                    Message = "Question cannot be empty."
                });
            }

            _logger.LogInformation("Received question: {Question}", request.Question);

            var pdfContent = _pdfService.GetCurrentPdfContent();
            if (string.IsNullOrEmpty(pdfContent))
            {
                _logger.LogWarning("No PDF content available");
                return BadRequest(new ChatResponse
                {
                    Success = false,
                    Message = "No PDF has been uploaded yet. Please upload a PDF first."
                });
            }

            var response = await _chatService.ProcessQuestionAsync(request.Question, pdfContent);
            
            return Ok(new ChatResponse
            {
                Success = true,
                Message = response
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing question: {Question}", request?.Question);
            return StatusCode(500, new ChatResponse
            {
                Success = false,
                Message = "An error occurred while processing your question. Please try again."
            });
        }
    }
} 