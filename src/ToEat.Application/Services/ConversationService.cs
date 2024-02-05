using Azure.AI.OpenAI;
using Microsoft.EntityFrameworkCore;
using ToEat.Domain.Data;
using ToEat.Domain.Models;

namespace ToEat.Application.Services;

public class ConversationService
{
    private readonly ToEatContext _context;
    private readonly GptIntegrationService _gptIntegrationService;
    public ConversationService(ToEatContext context, GptIntegrationService gptIntegrationService)
    {
        _context = context;
        _gptIntegrationService = gptIntegrationService;
    }

    public async Task<Conversation> CreateNewConversation(int metaPromptElementId)
    {
        var metaPromptElement = await _context.MetaPromptElements.FindAsync(metaPromptElementId);
        if (metaPromptElement == null)
        {
            return null;
        }
        var conversation = new Conversation();
        conversation.Messages.Add(new Message { Text = metaPromptElement.Text, Role = "system" });
        _context.Conversations.Add(conversation);

        await _context.SaveChangesAsync();

        return conversation;
    }

    public async Task<Conversation> AddMessage(int conversationId, string text, string role = "user")
    {
        var conversation = await _context.Conversations.FindAsync(conversationId);
        if (conversation == null)
        {
            return null;
        }
        conversation.Messages.Add(new Message { Text = text, Role = role });
        await _context.SaveChangesAsync();

        return conversation;
    }

    public async Task<ChatChoice> GetAnswer(int conversationId)
    {
        var conversationWithMessages = _context.Conversations
            .Include(c => c.Messages)
            .FirstOrDefault(c => c.Id == conversationId);
        if (conversationWithMessages == null)
        {
            return null;
        }
        var answer = await _gptIntegrationService.GetAnswer(conversationWithMessages);

        return answer;
    }
}