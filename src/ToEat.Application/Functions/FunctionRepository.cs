using ToEat.Application.Functions;

namespace ToEat.Application.Functions;
public class FunctionRepository
{
    private readonly IEnumerable<IFunction> _functions;

    public FunctionRepository(IEnumerable<IFunction> functions)
    {
        _functions = functions;
    }

    public IEnumerable<IFunction> GetFunctions()
    {
        return _functions;
    }

    public IFunction GetFunction(string functionName)
    {
        return _functions.FirstOrDefault(f => f.GetFunctionDefinition().Name == functionName);
    }
}