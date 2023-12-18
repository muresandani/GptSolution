using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;
using System.Text;

using UserConversation = ToEat.Domain.Models.Conversation;
using ToEat.Application.Strategies;
using ToEat.Application.Functions;
using Microsoft.Extensions.Configuration;

namespace ToEat.Application.Services;
public class ChatCompletionService
{
    private readonly OpenAIClient _openAIClient;

    private readonly FunctionRepository _functionRepository;
    public ChatCompletionService(IConfiguration configuration, FunctionRepository functionRepository)
    {
        _openAIClient = new OpenAIClient(configuration["OpenAI:ApiKey"], new OpenAIClientOptions());
        _functionRepository = functionRepository;
    }

    public async Task<ChatChoice> GetAnswer(UserConversation userConversation)
    {
        var chatCompletionsOptions = new ChatCompletionsOptions()
        {
            DeploymentName = "gpt-4-1106-preview", 
        };

        foreach (var message in ToOpenAiConversation(userConversation))
        {
            chatCompletionsOptions.Messages.Add(message);
        }

        foreach (var function in _functionRepository.GetFunctions())
        {
            chatCompletionsOptions.Functions.Add(function.GetFunctionDefinition());
        }

        Response<ChatCompletions> response = _openAIClient.GetChatCompletions(chatCompletionsOptions);
        ChatCompletions chatCompletions = response.Value;

        IReadOnlyList<ChatChoice> choices = chatCompletions.Choices;
        Azure.AI.OpenAI.CompletionsFinishReason? test = choices.First().FinishReason;
        return choices.First();        
    }

    private static List<ChatMessage> ToOpenAiConversation(UserConversation conversation)
    {
        var messages = new List<ChatMessage>();
        foreach (var message in conversation.Messages)
        {
            if (message.Role == "user")
            {
                messages.Add(new ChatMessage(ChatRole.User, message.Text));
            }
            if (message.Role == "system")
            {
                messages.Add(new ChatMessage(ChatRole.System, message.Text));
            }
            if (message.Role == "assistant")
            {
                messages.Add(new ChatMessage(ChatRole.Assistant, message.Text));
            }
            if (message.Role == "function")
            {
                messages.Add(new ChatMessage(ChatRole.Function, message.Text));
            }
            if (message.Role == "function_call")
            {
                var functionCallMessage = new ChatMessage();
                functionCallMessage.FunctionCall = new FunctionCall("addInventoryItem", "{\"name\": \"test\", \"quantity\": 1}");
                messages.Add(functionCallMessage);
            }
        }
        return messages;
    }
}
