using ChatChoice = Azure.AI.OpenAI.ChatChoice;
using ToEat.Application.Strategies;
using ToEat.Application.Strategies.Results;
using ToEat.Application.Strategies.Parameters;

namespace ToEat.Application.Services;
public class ResponseHandlingService
{
    private readonly IEnumerable<IResponseHandlingStrategy> _strategies;

    public ResponseHandlingService(IEnumerable<IResponseHandlingStrategy> strategies)
    {
        _strategies = strategies;
    }

    public async Task<IStrategyResult> HandleResponse(StrategyParameter parameter)
    {
        ChatChoice response = parameter.GetModelResponse();
        Azure.AI.OpenAI.CompletionsFinishReason? finishReason = response.FinishReason;
        if (!finishReason.HasValue)
        {
            throw new Exception($"No finish reason found");
        }
        var strategy = _strategies.FirstOrDefault(s => s.CanHandle(finishReason.ToString()));
        
        if (strategy == null)
        {
            throw new Exception($"No strategy found for response type {finishReason.ToString()}");
        }
        return await strategy.Handle(parameter);
    }
}