namespace ToEat.Application.Functions.Parameters;

using Newtonsoft.Json;

public class FunctionArgument : IFunctionArgument
{
    private dynamic _Arguments;

    public FunctionArgument(string arguments)
    {
        _Arguments = JsonConvert.DeserializeObject(arguments);
    }

    public dynamic GetArguments()
    {
        return _Arguments;
    }
}