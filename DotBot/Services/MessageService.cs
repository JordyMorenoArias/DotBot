using DotBot.Models.DTOs.Message;
using DotBot.Models.Entities;
using DotBot.Repositories;
using DotBot.Repositories.Interfaces;
using DotBot.Services.Interfaces;

namespace DotBot.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IChatSessionService _chatSessionService;

        public MessageService(IMessageRepository messageRepository, IChatSessionService chatSessionService)
        {
            _messageRepository = messageRepository;
            _chatSessionService = chatSessionService;
        }

        public async Task<Message?> GetMessageById(int id)
        {
            return await _messageRepository.GetMessageById(id);
        }

        public async Task<bool> AddMessage(MessageAddDto messageAdd)
        {
            var chatSession = await _chatSessionService.GetChatSessionById(messageAdd.ChatSessionId);

            if (chatSession == null)
                throw new ArgumentException("Chat session not found");

            var message = new Message
            {
                ChatSessionId = messageAdd.ChatSessionId,
                Role = messageAdd.Role,
                Content = messageAdd.Content,
            };

            return await _messageRepository.AddMessage(message);
        }

        public async Task<bool> UpdateMessage(Message message)
        {
            var result = await _messageRepository.GetMessageById(message.Id);

            if (result == null)
                throw new ArgumentException("Message not found");

            return await _messageRepository.UpdateMessage(message);
        }

        public async Task<bool> DeleteMessage(int id)
        {
            var message = await _messageRepository.GetMessageById(id);

            if (message == null)
                throw new ArgumentException("Message not found");

            return await _messageRepository.DeleteMessage(id);
        }
    }
}
