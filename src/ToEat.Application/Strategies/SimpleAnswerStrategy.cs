using Azure.AI.OpenAI;
using ToEat.Application.Strategies.Parameters;
using ToEat.Application.Strategies.Results;

namespace ToEat.Application.Strategies
{
    public class SimpleAnswerStrategy : IResponseHandlingStrategy
    {
        public bool CanHandle(string responseType)
        {
            return responseType == "stop";
        }

        public async Task<IStrategyResult> Handle(IStrategyParameter parameter)
        {
            ChatChoice response = parameter.GetModelResponse();

            Console.WriteLine(response.Message);

            return await Task.FromResult<IStrategyResult>(new StrategyResult
            {
                _Message = response.Message.ToString()
            });
        }
    }
}