namespace ToEat.Domain.Models;

public class Message
{
    public int Id { get; set; }

    public string Text { get; set; }

    public string Role { get; set; }

    public int ConversationId { get; set; }
}