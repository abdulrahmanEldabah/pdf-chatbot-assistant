using System.Text;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace PdfChatbotBackend.Services
{
    public interface IChatService
    {
        Task<string> ProcessQuestionAsync(string question, string pdfContent);
    }

    public class ChatService : IChatService
    {
        private readonly IAIService _aiService;
        private readonly ILogger<ChatService> _logger;

        public ChatService(IAIService aiService, ILogger<ChatService> logger)
        {
            _aiService = aiService;
            _logger = logger;
        }

        public async Task<string> ProcessQuestionAsync(string question, string pdfContent)
        {
            try
            {
                // Extract relevant context from PDF content
                var context = ExtractRelevantContext(question, pdfContent);

                // Get AI response
                var response = await _aiService.GetAnswerAsync(question, context);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing question");
                return "I apologize, but I encountered an error while processing your question.";
            }
        }

        private string ExtractRelevantContext(string question, string pdfContent)
        {
            // Simple context extraction - you can make this more sophisticated
            var sentences = pdfContent.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(s => s.Trim())
                                    .Where(s => !string.IsNullOrWhiteSpace(s))
                                    .ToList();

            // Find sentences that contain words from the question
            var questionWords = question.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var relevantSentences = sentences.Where(sentence =>
                questionWords.Any(word => sentence.ToLower().Contains(word)))
                .Take(5) // Limit to 5 most relevant sentences
                .ToList();

            return string.Join(". ", relevantSentences);
        }
    }
} 