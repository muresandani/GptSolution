using Microsoft.AspNetCore.Mvc;
using ToEat.Domain.Models;
using ToEat.Application.Services;

namespace ToEat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly ChatCompletionService _chatService;
        private readonly ConversationService _conversationService;

        public ConversationController(ChatCompletionService chatService, ConversationService conversationService)
        {
            _chatService = chatService;
            _conversationService = conversationService;
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
            var response = await _conversationService.GetAnswer(conversationId);
            return Ok(response);
        }
    }
}