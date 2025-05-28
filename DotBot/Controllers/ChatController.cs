using Microsoft.AspNetCore.Mvc;
using DotBot.Models.ViewModels;
using DotBot.Services.Interfaces;
using DotBot.Models.DTOs.Message;
using DotBot.Services;
using DotBot.Models.DTOs.ChatSession;
namespace DotBot.Controllers;

/// <summary>
/// Controller for managing chat sessions and interactions.
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
public class ChatController : Controller
{
    private readonly IUserService _userService;
    private readonly IChatManagerService _chatManagerService;
    private readonly ILogger<ChatController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatController"/> class.
    /// </summary>
    /// <param name="userService">The user service.</param>
    /// <param name="chatManagerService">The chat manager service.</param>
    /// <param name="logger">The logger.</param>
    public ChatController(IUserService userService, IChatManagerService chatManagerService, ILogger<ChatController> logger)
    {
        _userService = userService;
        _chatManagerService = chatManagerService;
        _logger = logger;
    }

    /// <summary>
    /// Indexes the specified chat session identifier.
    /// </summary>
    /// <param name="chatSessionId">The chat session identifier.</param>
    /// <returns>
    /// A Task that represents the asynchronous operation. 
    /// The task result contains an IActionResult that either:
    /// - Redirects to login if user is not authenticated
    /// - Returns the chat view with the view model
    /// - Returns NotFound if an error occurs
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int chatSessionId)
    {
        var user = _userService.GetAuthenticatedUser(HttpContext);
        if (user == null)
            return RedirectToAction("Login", "Account");

        try
        {
            var viewModel = await _chatManagerService.GetChatViewModelAsync(chatSessionId, user);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading chat view model.");
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Sends the message.
    /// </summary>
    /// <param name="messageAdd">The message add.</param>
    /// <returns>
    /// A Task that represents the asynchronous operation.
    /// The task result contains an IActionResult that either:
    /// - Returns a JSON response with the chat bot's replies if successful
    /// - Returns BadRequest if model state is invalid or user is not authenticated
    /// - Returns BadRequest with error message if an exception occurs
    /// </returns>
    /// <exception cref="System.Exception">Thrown when there's an error processing the message.</exception>
    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] MessageAddDto messageAdd)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = _userService.GetAuthenticatedUser(HttpContext);

        if (user == null)
            return RedirectToAction("Login", "Account");

        try
        {
            var response = await _chatManagerService.HandleUserMessageAsync(messageAdd, user);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error processing user message.");
            return BadRequest(new { error = "An error occurred while processing your message. Please try again." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while sending message.");
            return BadRequest(new { error = "An unexpected error occurred while processing your message." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> GenerateTitle([FromBody] ChatTitleRequest request)
    {
        var user = _userService.GetAuthenticatedUser(HttpContext);

        if (user == null)
            return RedirectToAction("Login", "Account");

        try
        {
            var chatSession = await _chatManagerService.CreateTitleForChatSession(user.Id, request.ChatSessionId);
            return Ok(chatSession);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating title for chat session.");
            return BadRequest(new { error = "An error occurred while generating the title." });
        }
    }

    /// <summary>
    /// Deletes the chat session.
    /// </summary>
    /// <param name="chatSessionId">The chat session identifier.</param>
    /// <returns>
    /// A Task that represents the asynchronous operation.
    /// The task result contains an IActionResult that either:
    /// - Redirects to Index after successful deletion
    /// - Redirects to login if user is not authenticated
    /// - Returns NotFound if the chat session doesn't exist or an error occurs
    /// </returns>
    /// <exception cref="System.Exception">Thrown when there's an error deleting the chat session.</exception>
    [HttpGet]
    public async Task<IActionResult> DeleteChatSession([FromQuery] int chatSessionId)
    {
        var user = _userService.GetAuthenticatedUser(HttpContext);

        if (user == null)
            return RedirectToAction("Login", "Account");

        try
        {
            await _chatManagerService.DeleteChatSessionAsync(chatSessionId, user);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting chat session.");
            return NotFound(ex.Message);
        }
    }
}