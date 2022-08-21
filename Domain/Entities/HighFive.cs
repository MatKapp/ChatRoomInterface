using Application.Common.Models;

namespace Domain.Entities;

public class HighFive : ChatEvent
{
    public string ReceiverName { get; set; }
    public override ChatEventType EventType => ChatEventType.HighFive;
    
    public override string GenerateMessage()
        => $"{UserName} high-fives {ReceiverName}";
}