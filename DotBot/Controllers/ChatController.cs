using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DotBot.Models;
using DotBot.Models.ViewModels;
using DotBot.Services.Interfaces;
using DotBot.Models.Entities;
using DotBot.Models.DTOs.Message;
using System.Threading.Tasks;

namespace DotBot.Controllers;

public class ChatController : Controller
{
    private readonly ILogger<ChatController> _logger;
    private readonly IChatSessionService _chatSessionService;
    private readonly IUserService _userService;
    private readonly IChatBotService _chatBotService;
    private readonly IMarkdownService _markdownService;

    public ChatController(ILogger<ChatController> logger, IChatSessionService chatSessionService, IUserService userService, IChatBotService chatBotService, IMarkdownService markdownService)
    {
        _logger = logger;
        _chatSessionService = chatSessionService;
        _userService = userService;
        _chatBotService = chatBotService;
        _markdownService = markdownService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = _userService.GetAuthenticatedUser(HttpContext);

        if (user == null)
            return RedirectToAction("Login", "Account");

        var chatSessions = await _chatSessionService.GetChatSessionsByUserId(user.Id);

        ChatSession? currentChatSession = null;

        if (currentChatSession == null)
        {
            currentChatSession = await _chatSessionService.GetMostRecentSessionByUserId(user.Id);
        }

        if (currentChatSession == null)
        {
            currentChatSession = await _chatSessionService.AddChatSession(user.Id);
        }

        if (currentChatSession != null)
        {
            currentChatSession.Messages = _markdownService.ConvertMarkdownToHtml(currentChatSession.Messages).ToList();
        }

        var chatViewModel = new ChatViewModel
        {
            ChatSessions = chatSessions,
            CurrentChatSession = currentChatSession
        };

        return View(chatViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] MessageAddDto messageAdd)
    {
        if (messageAdd == null)
            return BadRequest("Message cannot be null");

        var user = _userService.GetAuthenticatedUser(HttpContext);

        if (user == null)
            return RedirectToAction("Login", "Account");

        messageAdd.UserId = user.Id;

        var response = await _chatBotService.HandleUserPrompt(messageAdd);

        if (response == null)
            return BadRequest("Failed to get a response from the chat bot");

        var updatedSession = await _chatSessionService.GetChatSessionById(messageAdd.ChatSessionId);

        if (updatedSession == null)
            return BadRequest("Chat session not found");

        updatedSession.Messages = _markdownService.ConvertMarkdownToHtml(updatedSession.Messages).ToList();

        return PartialView("_MessagesPartial", updatedSession);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteChatSession(int id)
    {
        var user = _userService.GetAuthenticatedUser(HttpContext);

        if (user == null)
            return RedirectToAction("Login", "Account");

        var chatSession = await _chatSessionService.GetChatSessionById(id);

        if (chatSession == null || chatSession.UserId != user.Id)
            return NotFound();

        await _chatSessionService.DeleteChatSession(id);
        return RedirectToAction("Index");
    }
}
