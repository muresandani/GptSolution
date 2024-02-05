using Azure;
using Azure.AI.OpenAI;
using ToEat.Application.Strategies.Parameters;
using ToEat.Application.Strategies.Results;

namespace ToEat.Application.Services;

public class ChatService 
{
    private readonly ConversationService _conversationService;

    private readonly ResponseHandlingService _responseHandlingService;

    public ChatService(ConversationService conversationService, ResponseHandlingService responseHandlingService)
    {
        _conversationService = conversationService;
        _responseHandlingService = responseHandlingService;
    }

    public async Task<ChatChoice> GetAnswer(int conversationId)
    {
        ChatChoice answer = await _conversationService.GetAnswer(conversationId);
        StrategyParameter strategyParameter = new StrategyParameter(conversationId, answer);
        return await _responseHandlingService.HandleResponse(strategyParameter);
    }
}