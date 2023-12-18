using Azure.AI.OpenAI;
using ToEat.Application.Functions.Parameters;
using ToEat.Application.Functions.Responses;

namespace ToEat.Application.Functions;

public interface IFunction
{
    IFunctionResponse Execute(IFunctionArgument parameter);
    FunctionDefinition GetFunctionDefinition();
}
