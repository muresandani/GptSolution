using Azure.AI.OpenAI;
using ToEat.Application.Functions.Parameters;
using ToEat.Application.Functions;
using ToEat.Application.Services;
using ToEat.Application.Strategies.Parameters;
using System.Text.Json;
using ToEat.Domain.Data;
using ToEat.Domain.Models;

namespace ToEat.Application.Strategies;
public class FunctionCallStrategy : IResponseHandlingStrategy
{
    private readonly FunctionRepository _functionRepository;

    private readonly ConversationService _conversationService;
    
    private readonly ToEatContext _context;
    public FunctionCallStrategy(FunctionRepository functionRepository, ConversationService conversationService, ToEatContext context)
    {
        _functionRepository = functionRepository;
        _conversationService = conversationService;
        _context = context;
    }
    
    public bool CanHandle(string responseType)
    {
        return responseType == "function_call";
    }

    public async Task<ChatChoice> Handle(IStrategyParameter parameter)
    {
        var response = parameter.GetModelResponse();
        var conversation = _context.Conversations.Find(parameter.GetConversationId());

        LogModelResponse(response, conversation);

        var functionCall = response.Message.FunctionCall;
        string functionName = functionCall.Name;
        FunctionArgument parameters = new FunctionArgument(functionCall.Arguments);
        var result =_functionRepository.GetFunction(functionName).Execute(parameters);
        LogFunctionResult(conversation, result._Message, functionName);

        return await _conversationService.GetAnswer(parameter.GetConversationId());
    }

    private void LogModelResponse(ChatChoice response, Conversation conversation)
    {
        var functionCall = new Dictionary<string, string>
        {
            { "name", response.Message.FunctionCall.Name },
            { "arguments", response.Message.FunctionCall.Arguments }
        };

        string functionCallJson = JsonSerializer.Serialize(functionCall);
        conversation.Messages.Add(new Message { Text = functionCallJson, Role = "function_call" });

        _context.SaveChanges();
    }

    private void LogFunctionResult(Conversation conversation, string result, string functionName)
    {
        var functionResult = new Dictionary<string, string>
        {
            { "name", functionName },
            { "content", result}
        };

        string functionResultJson = JsonSerializer.Serialize(functionResult);
        conversation.Messages.Add(new Message { Text = functionResultJson, Role = "function_result" });
        _context.SaveChanges();
    }
}
