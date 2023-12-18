using System;
using System.Collections.Generic;

namespace ToEat.Domain.Models;
// Abstract base class for all prompt elements
public abstract class PromptElement
{
    public int Id { get; set; }
    public string Text { get; set; }
    public List<PromptElementParameter> Parameters { get; set; }

    protected PromptElement()
    {
        Parameters = new List<PromptElementParameter>();
    }
}
