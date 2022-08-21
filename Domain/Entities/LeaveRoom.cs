using Application.Common.Models;

namespace Domain.Entities;

public class LeaveRoom : ChatEvent
{
    public override ChatEventType EventType => ChatEventType.LeaveRoom;
    
    public override string GenerateMessage()
        => $"{UserName} leaves";
}