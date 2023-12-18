namespace ToEat.Domain.Models;

public class Conversation
{
    public int Id { get; set; }

    public List<Message> Messages { get; set; }
    
    public Conversation()
    {
        Messages = new List<Message>();
    }

}