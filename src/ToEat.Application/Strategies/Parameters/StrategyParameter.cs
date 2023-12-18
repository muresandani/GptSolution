using Azure.AI.OpenAI;

namespace ToEat.Application.Strategies.Parameters;

public class StrategyParameter : IStrategyParameter
{
    private readonly int _conversationId;
    private readonly ChatChoice _modelResponse;

    public StrategyParameter(int conversationId, ChatChoice modelResponse)
    {
        _conversationId = conversationId;
        _modelResponse = modelResponse;
    }

    public int GetConversationId()
    {
        return _conversationId;
    }

    public ChatChoice GetModelResponse()
    {
        return _modelResponse;
    }
}