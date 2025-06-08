using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using PdfChatbotBackend.Models;
using System.Text;

namespace PdfChatbotBackend.Services;

public class PdfService
{
    private readonly ILogger<PdfService> _logger;
    private string _currentPdfContent = string.Empty;

    public PdfService(ILogger<PdfService> logger)
    {
        _logger = logger;
    }

    public PdfUploadResponse ProcessPdf(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return new PdfUploadResponse
                {
                    Success = false,
                    Message = "No file was uploaded."
                };
            }

            if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
            {
                return new PdfUploadResponse
                {
                    Success = false,
                    Message = "Only PDF files are allowed."
                };
            }

            using var stream = file.OpenReadStream();
            using var pdfReader = new PdfReader(stream);
            using var pdfDocument = new PdfDocument(pdfReader);
            
            var text = new StringBuilder();
            var pageCount = pdfDocument.GetNumberOfPages();

            for (int i = 1; i <= pageCount; i++)
            {
                var page = pdfDocument.GetPage(i);
                var strategy = new SimpleTextExtractionStrategy();
                var currentText = PdfTextExtractor.GetTextFromPage(page, strategy);
                text.AppendLine(currentText);
            }

            _currentPdfContent = text.ToString();

            return new PdfUploadResponse
            {
                Success = true,
                Message = "PDF processed successfully.",
                PageCount = pageCount,
                FileName = file.FileName
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing PDF file");
            return new PdfUploadResponse
            {
                Success = false,
                Message = "An error occurred while processing the PDF."
            };
        }
    }

    public string GetCurrentPdfContent()
    {
        return _currentPdfContent;
    }
} 