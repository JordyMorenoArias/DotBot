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
        private readonly ILogger<GeminiService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeminiService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="System.ArgumentNullException">
        /// API Key is not set
        /// or
        /// Endpoint is not set
        /// </exception>
        public GeminiService(IConfiguration configuration, ILogger<GeminiService> logger)
        {
            _apiKey = configuration["Gemini:ApiKey"] ?? throw new ArgumentNullException("API Key is not set");
            _endpoint = configuration["Gemini:Endpoint"] ?? throw new ArgumentNullException("Endpoint is not set");
            _SystemPrompt = configuration["Prompt:churnAnalysisPrompt"];
            _httpClient = new HttpClient();
            _logger = logger;
        }

        /// <summary>
        /// Sends a list of chat messages to the Gemini AI model and retrieves the assistant's response.
        /// </summary>
        /// <param name="messages">A list of messages forming the conversation context.</param>
        /// <returns>The AI-generated response as a string, or null if the request fails.</returns>
        public async Task<string?> GetIAResponse(IEnumerable<ChatMessage> messages)
        {
            var fullMessages = new List<object>
            {
                new
                {
                    role = "user",
                    parts = new[] { new
                    {
                        text = _SystemPrompt ?? "You are a helpful assistant." } 
                    }
                }
            };

            fullMessages.AddRange(messages.Select(m => new
            {
                role = m.Role,
                parts = new[] { new 
                { 
                    text = m.Content } 
                }
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
                _logger.LogError($"Failed to get response from Gemini AI: {response.StatusCode} - {response.ReasonPhrase}");
                return null;
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(responseBody);

            var message = jsonDoc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString()!;

            return message ?? throw new InvalidOperationException("No response received from the Gemini AI.");
        }
    }
}