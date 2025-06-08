namespace PdfChatbotBackend.Models;

public class PdfUploadResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string? ExtractedText { get; set; }
} 