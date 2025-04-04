using DotBot.Models.DTOs.ChatSession;
using DotBot.Models.Entities;
using DotBot.Repositories.Interfaces;
using DotBot.Services.Interfaces;

namespace DotBot.Services
{
    public class ChatSessionService : IChatSessionService
    {
        private readonly IChatSessionRepository _chatSessionRepository;
        private readonly IUserService _userService;

        public ChatSessionService(IChatSessionRepository chatSessionRepository, IUserService userService)
        {
            _chatSessionRepository = chatSessionRepository;
            _userService = userService;
        }

        public async Task<ChatSession?> GetChatSessionById(int id)
        {
            return await _chatSessionRepository.GetChatSessionById(id);
        }

        public async Task<IEnumerable<ChatSession>> GetChatSessionsByUserId(int userId)
        {
            return await _chatSessionRepository.GetChatSessionsByUserId(userId);
        }

        public async Task<bool> AddChatSession(int userId)
        {
            var user = await _userService.GetUserById(userId);

            if (user == null)
                throw new ArgumentException("User not found");

            var chatSession = new ChatSession
            {
                UserId = userId
            };

            return await _chatSessionRepository.AddChatSession(chatSession);
        }

        public async Task<bool> UpdateChatSessionTitle(ChatSessionUpdateDto chatSessionUpdate)
        {
            var user = await _userService.GetUserById(chatSessionUpdate.UserId);

            if (user == null)
                throw new ArgumentException("User not found");

            var chatSession = await _chatSessionRepository.GetChatSessionById(chatSessionUpdate.Id);

            if (chatSession == null)
                throw new ArgumentException("Chat session not found");

            if (chatSessionUpdate.NewTitle != null)
                chatSession.Title = chatSessionUpdate.NewTitle;

            return await _chatSessionRepository.UpdateChatSession(chatSession);
        }

        public async Task<bool> DeleteChatSession(int id)
        {
            var chatSession = await _chatSessionRepository.GetChatSessionById(id);

            if (chatSession == null)
                throw new ArgumentException("Chat session not found");

            return await _chatSessionRepository.DeleteChatSession(id);
        }

        public async Task<bool> DeleteChatSessionsByUserId(int userId)
        {
            var user = await _userService.GetUserById(userId);

            if (user == null)
                throw new ArgumentException("User not found");

            return await _chatSessionRepository.DeleteChatSessionsByUserId(userId);
        }
    }
}
