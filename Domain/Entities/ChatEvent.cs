namespace Domain.Entities;

public abstract class ChatEvent
{
    public DateTime OccuredAt { get; set; }
    public string UserName { get; set; }
}