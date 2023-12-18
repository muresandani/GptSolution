using Azure.AI.OpenAI;
using ToEat.Domain.Data;
using ToEat.Domain.Models;
using ToEat.Application.Functions.Parameters;
using System.Text.Json;
using ToEat.Application.Functions.Responses;

namespace ToEat.Application.Functions;

public class AddInventoryItemFunction: IFunction
{
    ToEatContext _context;
    public AddInventoryItemFunction(ToEatContext context)
    {
        _context = context;
    }
    public IFunctionResponse Execute(IFunctionArgument data)
    {
        var arguments = data.GetArguments();
        foreach (var argument in arguments.items)
        {
            var inventoryItem = new InventoryItem
            {
                Name = argument.name,
                Quantity = argument.quantity,
                MeasurementUnit = argument.unit,
                Inventory = _context.Inventories.Find(1)
            };
            _context.Add(inventoryItem);

        }
        return new FunctionResponse(){
                _Message = "Added items to inventory"
            };
    }

    public FunctionDefinition GetFunctionDefinition()
    {
        return new FunctionDefinition()
        {
            Name = "AddInventoryItems",
            Description = "Add multiple items to a user's inventory",
            Parameters = BinaryData.FromObjectAsJson(
            new
            {
                Type = "object",
                Properties = new
                {
                    items = new
                    {
                        Type = "array",
                        Items = new
                        {
                            Type = "object",
                            Properties = new
                            {
                                name = new
                                {
                                    Type = "string",
                                    Description = "The name of the product",
                                },
                                quantity = new
                                {
                                    Type = "string",
                                    Description = "The quantity of the given product",
                                },
                                unit = new
                                {
                                    Type = "string",
                                    Description = "The unit of measurement for the given product",
                                    Enum = new[] { "gm", "ml", "pc" },
                                }
                            },
                            Required = new[] { "name", "quantity", "unit" },
                        }
                    }
                },
                Required = new[] { "items" },
            },
            new System.Text.Json.JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
        };
    }
}