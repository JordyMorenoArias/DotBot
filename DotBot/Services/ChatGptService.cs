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

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatGptService"/> class.
        /// </summary>
        /// <param name="configuration">Configuration containing the API key and endpoint URL for ChatGPT.</param>
        /// <exception cref="ArgumentNullException">Thrown if API key or endpoint is not found in configuration.</exception>
        public ChatGptService(IConfiguration configuration)
        {
            _apiKey = configuration["ChatGpt:ApiKey"] ?? throw new ArgumentNullException("API Key is not set");
            _endpoint = configuration["ChatGpt:Endpoint"] ?? throw new ArgumentNullException("Endpoint is not set");

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        /// <summary>
        /// Sends a list of messages to the ChatGPT API and retrieves the assistant's reply.
        /// </summary>
        /// <param name="messages">The message history including user and assistant roles.</param>
        /// <returns>The generated response from ChatGPT, or null if the request fails.</returns>
        public async Task<string?> GetChatGptResponse(IEnumerable<ChatMessage> messages)
        {
            var requestBody = new
            {
                model = "gpt-4o",
                messages = messages.Select(m => new
                {
                    role = m.Role,
                    content = m.Content
                }).ToList()
            };

            var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_endpoint, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var responseJson = System.Text.Json.JsonDocument.Parse(responseBody);
                var message = responseJson.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return message;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return null;
            }
        }
    }

}
