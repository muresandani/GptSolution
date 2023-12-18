using Azure.AI.OpenAI;
using ToEat.Application.Functions.Parameters;
using ToEat.Application.Functions;
using ToEat.Application.Services;
using ToEat.Application.Strategies.Results;
using ToEat.Application.Strategies.Parameters;
using Azure;

namespace ToEat.Application.Strategies;
public class FunctionCallStrategy : IResponseHandlingStrategy
{
    private readonly FunctionRepository _functionRepository;

    private readonly ConversationService _conversationService;
    
    public FunctionCallStrategy(FunctionRepository functionRepository, ConversationService conversationService)
    {
        _functionRepository = functionRepository;

    }
    
    public bool CanHandle(string responseType)
    {
        return responseType == "function_call";
    }
    public async Task<IStrategyResult> Handle(IStrategyParameter parameter)
    {
        var response = parameter.GetModelResponse();
        var functionCall = response.Message.FunctionCall;
        string functionName = functionCall.Name;
        FunctionArgument parameters = new FunctionArgument(functionCall.Arguments);
        var result =_functionRepository.GetFunction(functionName).Execute(parameters);
        
        return await _conversationService.GetAnswer(parameter.GetConversationId());
    }
}
