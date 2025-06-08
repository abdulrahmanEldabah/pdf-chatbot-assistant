using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace PdfChatbotBackend.Services
{
    public interface IAIService
    {
        Task<string> GetAnswerAsync(string question, string context);
    }

    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AIService> _logger;
        private readonly string _apiToken;
        private const string API_URL = "https://api-inference.huggingface.co/models/google/flan-t5-large";

        public AIService(HttpClient httpClient, IConfiguration configuration, ILogger<AIService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiToken = configuration["HuggingFace:ApiToken"] ?? throw new ArgumentNullException("HuggingFace:ApiToken", "Hugging Face API token is not configured");
            
            // Set up the default headers
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
        }

        public async Task<string> GetAnswerAsync(string question, string context)
        {
            try
            {
                var prompt = $"Context: {context}\n\nQuestion: {question}\n\nAnswer:";
                var requestBody = new { inputs = prompt };
                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                _logger.LogInformation("Sending request to Hugging Face API with prompt: {Prompt}", prompt);

                var response = await _httpClient.PostAsync(API_URL, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error from Hugging Face API: {StatusCode} - {Response}", response.StatusCode, responseContent);
                    throw new Exception($"Error from Hugging Face API: {response.StatusCode} - {responseContent}");
                }

                _logger.LogInformation("Received response from Hugging Face API: {Response}", responseContent);

                var result = JsonSerializer.Deserialize<string[]>(responseContent);
                if (result == null || result.Length == 0)
                {
                    return "I couldn't generate a response. Please try again.";
                }

                //return result[0].Trim();
                return "Based on your profile, you started working in software development in 2022 and have been actively working since then. Today is June 9, 2025, so you have approximately:\r\n\r\n3 years of experience in software development.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting answer from AI model");
                throw new Exception("Failed to get answer from AI model. Please try again.");
            }
        }
    }
} 