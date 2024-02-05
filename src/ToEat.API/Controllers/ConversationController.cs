using Microsoft.AspNetCore.Mvc;
using ToEat.Domain.Models;
using ToEat.Application.Services;

namespace ToEat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly ConversationService _conversationService;

        private readonly ChatService _chatService;

        public ConversationController(ConversationService conversationService, ChatService chatService)
        {
            _conversationService = conversationService;
            _chatService = chatService;
        }

        [HttpPost("new-conversation/{metaPromptElementId}")]
        public async Task<IActionResult> CreateConversation(int metaPromptElementId)
        {
            var response = await _conversationService.CreateNewConversation(metaPromptElementId);
            return Ok(response);
        }

        [HttpPost("message/{conversationId}")]
        public async Task<IActionResult> SendMessage(int conversationId, [FromBody] Message message)
        {
            var response = await _conversationService.AddMessage(conversationId, message.Text);
            return Ok(response);
        }

        [HttpPost("answer/{conversationId}")]
        public async Task<IActionResult> GetAnswer(int conversationId)
        {
            var response = await _chatService.GetAnswer(conversationId);
            return Ok(response);
        }
    }
}