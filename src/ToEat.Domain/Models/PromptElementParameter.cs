namespace ToEat.Domain.Models;

// Represents the parameters that can be associated with each prompt element
public class PromptElementParameter
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public int PromptElementId { get; set; }
    public PromptElement PromptElement { get; set; }
}