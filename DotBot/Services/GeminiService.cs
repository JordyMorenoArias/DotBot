using DotBot.Models.DTOs.Message;
using DotBot.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace DotBot.Services
{
    /// <summary>
    /// Service for interacting with the Gemini AI API to get chat responses.
    /// </summary>
    public class GeminiService : IChatIAService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _endpoint;
        private readonly string? _SystemPrompt;

        public GeminiService(IConfiguration configuration)
        {
            _apiKey = configuration["Gemini:ApiKey"] ?? throw new ArgumentNullException("API Key is not set");
            _endpoint = configuration["Gemini:Endpoint"] ?? throw new ArgumentNullException("Endpoint is not set");
            _SystemPrompt = configuration["Prompt:churnAnalysisPrompt"];
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Sends a list of chat messages to the Gemini AI model and retrieves the assistant's response.
        /// </summary>
        /// <param name="messages">A list of messages forming the conversation context.</param>
        /// <returns>The AI-generated response as a string, or null if the request fails.</returns>
        public async Task<string?> GetChatGptResponse(IEnumerable<ChatMessage> messages)
        {
            var fullMessages = new List<object>
            {
                new
                {
                    role = "user",
                    parts = new[] { new { text = _SystemPrompt } }
                }
            };

            fullMessages.AddRange(messages.Select(m => new
            {
                role = m.Role,
                parts = new[] { new { text = m.Content } }
            }));

            var requestBody = new
            {
                model = "gemini-2.0-flash",
                contents = fullMessages
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var requestUrl = $"{_endpoint}?key={_apiKey}";

            var response = await _httpClient.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return null;
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(responseBody);

            return jsonDoc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();
        }
    }
}