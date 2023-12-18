using ToEat.Application.Strategies;

namespace ToEat.Application.Strategies;
public class StrategyRepository
{
    private readonly IEnumerable<IResponseHandlingStrategy> _strategies;

    public StrategyRepository(IEnumerable<IResponseHandlingStrategy> strategies)
    {
        _strategies = strategies;
    }
}