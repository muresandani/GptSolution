using Azure;
using Azure.AI.OpenAI;

using UserConversation = ToEat.Domain.Models.Conversation;
using ToEat.Application.Functions;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ToEat.Application.Services;
public class GptIntegrationService
{
    private readonly OpenAIClient _openAIClient;

    private readonly FunctionRepository _functionRepository;
    public GptIntegrationService(IConfiguration configuration, FunctionRepository functionRepository)
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
            if (message.Role == "function_result")
            {
                var deserializedMessage = JsonSerializer.Deserialize<Dictionary<string, string>>(message.Text);
                ChatMessage functionResultMessage = new ChatMessage();
                functionResultMessage.Name = deserializedMessage["name"];
                functionResultMessage.Role = ChatRole.Function;
                functionResultMessage.Content = deserializedMessage["content"];
                messages.Add(functionResultMessage);
            }
            if (message.Role == "function_call")
            {
                var deserializedMessage = JsonSerializer.Deserialize<Dictionary<string, string>>(message.Text);
                var functionCallMessage = new ChatMessage();
                functionCallMessage.Role = ChatRole.Assistant;
                functionCallMessage.FunctionCall = new FunctionCall(deserializedMessage["name"], deserializedMessage["arguments"]);
                
                messages.Add(functionCallMessage);
            }
        }
        return messages;
    }
}
