using Application.Common.Models;

namespace Domain.Entities;

public abstract class ChatEvent
{
    public abstract ChatEventType EventType { get; }
    public abstract string GenerateMessage();
    public DateTime OccuredAt { get; set; }
    public string UserName { get; set; }
}