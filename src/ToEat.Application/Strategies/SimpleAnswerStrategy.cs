using Azure.AI.OpenAI;
using ToEat.Application.Strategies.Parameters;
using ToEat.Application.Strategies.Results;
using ToEat.Domain.Data;

namespace ToEat.Application.Strategies
{
    public class SimpleAnswerStrategy : IResponseHandlingStrategy
    {
        private ToEatContext _context;
        public SimpleAnswerStrategy(ToEatContext context)
        {
            _context = context;
        }
        public bool CanHandle(string responseType)
        {
            return responseType == "stop";
        }

        public async Task<ChatChoice> Handle(IStrategyParameter parameter)
        {
            var response = parameter.GetModelResponse();
            var conversation = _context.Conversations.Find(parameter.GetConversationId());
            conversation.Messages.Add(new Domain.Models.Message { Text = response.Message.Content, Role = "assistant" });

            _context.SaveChanges();
            return parameter.GetModelResponse();
        }
    }
}