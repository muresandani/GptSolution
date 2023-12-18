using Azure.AI.OpenAI;
namespace ToEat.Application.Strategies.Parameters;
public interface IStrategyParameter
{
    int GetConversationId();
    ChatChoice GetModelResponse();
}
