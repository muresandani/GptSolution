using Azure.AI.OpenAI;
using ToEat.Application.Strategies.Parameters;
using ToEat.Application.Strategies.Results;
namespace ToEat.Application.Strategies;
public interface IResponseHandlingStrategy
{
    bool CanHandle(string responseType);
    Task<ChatChoice> Handle(IStrategyParameter response);
}
