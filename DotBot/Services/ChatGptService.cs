using DotBot.Models.DTOs.Message;
using DotBot.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;

namespace DotBot.Services
{
    /// <summary>
    /// Service for interacting with the ChatGPT API (gpt-4o model) to get AI-generated responses.
    /// </summary>
    public class ChatGptService : IChatIAService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _endpoint;

        private readonly string? _SystemPrompt;
        private readonly ILogger<ChatGptService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatGptService"/> class.
        /// </summary>
        /// <param name="configuration">Configuration containing the API key and endpoint URL for ChatGPT.</param>
        /// <exception cref="ArgumentNullException">Thrown if API key or endpoint is not found in configuration.</exception>
        public ChatGptService(IConfiguration configuration, ILogger<ChatGptService> logger)
        {
            _apiKey = configuration["ChatGpt:ApiKey"] ?? throw new ArgumentNullException("API Key is not set");
            _endpoint = configuration["ChatGpt:Endpoint"] ?? throw new ArgumentNullException("Endpoint is not set");
            _SystemPrompt = configuration["Prompt:churnAnalysisPrompt"];
            _httpClient = new HttpClient();
            _logger = logger;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        /// <summary>
        /// Sends a list of messages to the ChatGPT API and retrieves the assistant's reply.
        /// </summary>
        /// <param name="messages">The message history including user and assistant roles.</param>
        /// <returns>The generated response from ChatGPT, or null if the request fails.</returns>
        public async Task<string?> GetIAResponse(IEnumerable<ChatMessage> messages)
        {
            var fullMessages = new List<object>
            {
                new
                {
                    role = "system",
                    content = new
                    {
                        text = _SystemPrompt ?? "You are a helpful assistant."
                    }
                }
            };

            fullMessages.AddRange(messages.Select(m => new
            {
                role = m.Role,
                content = new
                {
                    text = m.Content
                }
            }));

            var requestBody = new
            {
                model = "gpt-4o",
                messages = fullMessages
            };

            var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Failed to get response from Gemini AI: {response.StatusCode} - {response.ReasonPhrase}");
                return null;
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var responseJson = System.Text.Json.JsonDocument.Parse(responseBody);
            var message = responseJson.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return message ?? throw new InvalidOperationException("No response received from the AI service.");

        }
    }
}